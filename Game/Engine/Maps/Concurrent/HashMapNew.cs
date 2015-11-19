using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps.Concurrent
{
    //using TBinKey = IO.Common.Vector;

    class HashMapNew<TKey, TVal>
    {
        ConcurrentDictionary<TKey, HashBin> binDict { get; } = new ConcurrentDictionary<TKey, HashBin>();

        IObjectMapper<TVal, TKey> objectMapper { get; }


        public HashMapNew(IObjectMapper<TVal, TKey> mapper)
        {
            objectMapper = mapper;
        }



        HashBin getOrCreate(TKey id)
        {
            var b = binDict.GetOrAdd(id, (_) => new HashBin(objectMapper, id));
            b.SetObjectMovedHandler(onObjectMovedToAnotherBin);
            return b;
        }

        void onObjectMovedToAnotherBin(TKey newBin, TVal obj)
        {
            var bin = getOrCreate(newBin);
            bin.Add(obj);
        }


        public void Add(TVal obj)
        {
            var bin = getOrCreate(objectMapper.GetBinId(obj));
            bin.Add(obj);
        }

        public void AddRange(IEnumerable<TVal> objs)
        {
            foreach(var group in objs.GroupBy(obj => objectMapper.GetBinId(obj)))
            {
                var bin = getOrCreate(group.Key);
                foreach (var obj in group)
                    bin.Add(obj);
            }
        }

        public IEnumerable<TVal> GetBinContents(TKey binId)
        {
            HashBin bin = null;
            binDict.TryGetValue(binId, out bin);
            return bin?.Objects;
        }

        /// <summary>
        /// Updates all bins (thus all objects) in parallel.  
        /// </summary>
        public void Update(int msElapsed)
        {
            //Parallel.ForEach(binDict, (kvp) => kvp.Value.UpdateObjects(msElapsed));
            
            Parallel.ForEach(binDict, (kvp) => kvp.Value.UpdateObjects(msElapsed));
        }

        class HashBin
        {

            volatile Action<TKey, TVal> ObjectMovedToAnotherBin;

            volatile ConcurrentQueue<TVal> objects = new ConcurrentQueue<TVal>();


            IObjectMapper<TVal, TKey> objectMapper { get; }

            public TKey Id { get; }

            public IEnumerable<TVal> Objects { get { return objects; } }


            public HashBin(IObjectMapper<TVal, TKey> mapper, TKey id)
            {
                Id = id;
                objectMapper = mapper;
            }


            /// <summary>
            /// Adds the given object to this bin. 
            /// </summary>
            public void Add(TVal obj)
            {
                Debug.Assert(objectMapper.GetBinId(obj).Equals(Id));
                objects.Enqueue(obj);
            }

            public void SetObjectMovedHandler(Action<TKey, TVal> onObjectMovedToAnotherBin)
            {
                ObjectMovedToAnotherBin = onObjectMovedToAnotherBin;
            }

            /// <summary>
            /// Calls obj.Update. Checks the object for removal, 
            /// otherwise checks if the object moved out, 
            /// eventually raising an event to move it to another bin. 
            /// 
            /// Is not thread safe. Also creates a new queue every frame. 
            /// </summary>
            public void UpdateObjects(int msElapsed)
            {
                var oldObjects = objects;
                objects = new ConcurrentQueue<TVal>();

                foreach(var obj in oldObjects)
                {
                    //update object
                    objectMapper.Update(obj, msElapsed);

                    //check for removal
                    if(objectMapper.ShouldRemove(obj))
                        continue;
                    
                    //check for bin change
                    var newBinId = objectMapper.GetBinId(obj);
                    if (!newBinId.Equals(Id))
                        ObjectMovedToAnotherBin(newBinId, obj);
                    else
                        objects.Enqueue(obj);
                }
            }

        }
    }

    interface IObjectMapper<TVal, TKey>
    {
        /// <summary>
        /// Returns the id of the bin an object belongs to. 
        /// </summary>
        TKey GetBinId(TVal obj);

        /// <summary>
        /// Updates the state of an object based on the time elapsed since the last update. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        void Update(TVal obj, int msElapsed);

        /// <summary>
        /// Gets whether an object should be removed from the map as soon as possible. 
        /// </summary>
        bool ShouldRemove(TVal obj);
    }
}

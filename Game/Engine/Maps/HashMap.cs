using Engine.Objects.Entities;
using IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Engine.Maps
{
    /// <summary>
    /// Maps objects of type <typeparamref name="TVal"/> to bins of type <typeparamref name="TKey"/>. 
    /// Supports parallel updates and handles objects add/remove/update via the <see cref="IObjectMapper{TVal, TKey}"/> interface. 
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    class HashMap<TKey, TVal> : IEnumerable<TVal>
    {
        #region Delegates
        /// <summary>
        /// A method executed when an item changes its bin. 
        /// </summary>
        /// <param name="obj">The object that changes bins. </param>
        /// <param name="oldBin">The old bin of the object. </param>
        /// <param name="newBin">The new bin of the object. </param>
        public delegate void BinChangeDelegate(TVal obj, TKey oldBin, TKey newBin);

        /// <summary>
        /// A method executed when something happens to an item in the map. 
        /// </summary>
        /// <param name="obj">The object that triggered the event. </param>
        public delegate void ItemDelegate(TVal obj);
        #endregion


        readonly ConcurrentDictionary<TKey, HashBin> binDict;

        readonly IObjectMapper<TVal, TKey> objectMapper;

        /// <summary>
        /// The event raised whenever an item is added to this HashMap. 
        /// </summary>
        public event ItemDelegate ItemAdded;

        /// <summary>
        /// The event raised whenever an item is removed from this HashMap. 
        /// </summary>
        public event ItemDelegate ItemRemoved;

        /// <summary>
        /// The event raised whenever an item changes a bin inside this HashMap. 
        /// </summary>
        public event BinChangeDelegate ItemBinChanged;


        /// <summary>
        /// Gets a snapshot of all currently initialized bins. 
        /// </summary>
        internal IEnumerable<HashBin> Bins => binDict.Values;



        public HashMap(IObjectMapper<TVal, TKey> mapper)
        {
            binDict = new ConcurrentDictionary<TKey, HashBin>();
            objectMapper = mapper;
        }


        /// <summary>
        /// Gets or creates the bin with the given bin id. 
        /// </summary>
        /// <param name="id">The id of the bin to get or create. </param>
        HashBin getOrCreate(TKey id)
        {
            var b = binDict.GetOrAdd(id, (_) => new HashBin(objectMapper, id, onObjectBinChange, onObjectRemoved));
            return b;
        }

        void onObjectRemoved(TVal obj)
        {
            ItemRemoved?.Invoke(obj);
        }

        void onObjectBinChange(TVal obj, TKey oldBin, TKey newBin)
        {
            //move the item
            var bin = getOrCreate(newBin);
            bin.Add(obj);

            //raise the event
            ItemBinChanged?.Invoke(obj, oldBin, newBin);
        }

        /// <summary>
        /// Adds the given item to this HashMap. 
        /// </summary>
        /// <param name="obj">The item to add to the HashMap. </param>
        public void Add(TVal obj)
        {
            var bin = getOrCreate(objectMapper.GetBinId(obj));
            bin.Add(obj);
            ItemAdded?.Invoke(obj);
        }

        /// <summary>
        /// Adds the given collection of items to this HashMap. 
        /// </summary>
        /// <param name="objs">The items to add to the HashMap. </param>
        public void AddRange(IEnumerable<TVal> objs)
        {
            foreach (var group in objs.GroupBy(obj => objectMapper.GetBinId(obj)))
            {
                var bin = getOrCreate(group.Key);
                foreach (var obj in group)
                    bin.Add(obj);
            }
        }

        /// <summary>
        /// Gets all objects in the specified bin. 
        /// </summary>
        /// <param name="binId">The bin identifier.</param>
        public IEnumerable<TVal> BinQuery(TKey binId)
        {
            HashBin bin = null;
            binDict.TryGetValue(binId, out bin);
            return bin?.Objects ?? Enumerable.Empty<TVal>();
        }

        volatile int msElapsed;

        /// <summary>
        /// Updates all bins (and all objects in them) in parallel.  
        /// </summary>
        public void Update(int msElapsed)
        {
            this.msElapsed = msElapsed;
            //foreach (var b in binDict)
            //    b.Value.Update(msElapsed);
            Parallel.ForEach(binDict, loopBinItem);
        }

        void loopBinItem(KeyValuePair<TKey, HashBin> item)
        {
            item.Value.Update(msElapsed);
        }

        public IEnumerator<TVal> GetEnumerator() => 
            binDict.SelectMany(b => b.Value.Objects).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        internal class HashBin
        {
            readonly BinChangeDelegate ObjectBinChanged;
            readonly ItemDelegate ObjectRemoved;

            /// <summary>
            /// All the values kept in the hashbin. 
            /// </summary>
            volatile ConcurrentQueue<TVal> objects = new ConcurrentQueue<TVal>();

            volatile ConcurrentQueue<TVal> newObjects = new ConcurrentQueue<TVal>();

            IObjectMapper<TVal, TKey> objectMapper { get; }

            /// <summary>
            /// Gets the ID of this bin. 
            /// </summary>
            public TKey Id { get; }

            /// <summary>
            /// Gets all objects currently in the bin. 
            /// </summary>
            public IEnumerable<TVal> Objects => objects;


            public HashBin(IObjectMapper<TVal, TKey> mapper, TKey id, 
                BinChangeDelegate onObjectMovedToAnotherBin,
                ItemDelegate onObjectRemoved)
            {
                objectMapper = mapper;
                Id = id;

                ObjectBinChanged = onObjectMovedToAnotherBin;
                ObjectRemoved = onObjectRemoved;
            }


            /// <summary>
            /// Adds the given object to this bin. 
            /// </summary>
            public void Add(TVal obj)
            {
                Debug.Assert(objectMapper.GetBinId(obj).Equals(Id));
                newObjects.Enqueue(obj);
            }

            /// <summary>
            /// Calls obj.Update. Checks the object for removal, 
            /// otherwise checks if the object moved out, 
            /// eventually raising an event to move it to another bin. 
            /// 
            /// Is not thread safe. Also creates a new queue every frame. 
            /// </summary>
            public void Update(int msElapsed)
            {
                foreach (var obj in objects)
                {
                    //update object state
                    objectMapper.Update(obj, msElapsed);

                    //check for removal
                    if (objectMapper.ShouldRemove(obj))
                    {
                        ObjectRemoved(obj);
                        continue;
                    }

                    //raise bin changed event?
                    var newBinId = objectMapper.GetBinId(obj);
                    if (!newBinId.Equals(Id))
                    {
                        ObjectBinChanged(obj, Id, newBinId);
                        continue;
                    }

                    newObjects.Enqueue(obj);
                }

                objects = newObjects;
                newObjects = new ConcurrentQueue<TVal>();
            }

            public override string ToString() => $"HashBin @ {Id} ({objects.Count} items)";

            public override bool Equals(object obj) =>
                (obj is HashBin) && ((HashBin)obj).Id.Equals(Id);

            public override int GetHashCode() => Id.GetHashCode();
        }
    }

    /// <summary>
    /// Maps objects of type <typeparamref name="TVal"/> to values of type <typeparamref name="TKey"/>. 
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    interface IObjectMapper<TVal, TKey>
    {
        /// <summary>
        /// Returns the id of the bin an object belongs to. 
        /// </summary>
        TKey GetBinId(TVal obj);

        /// <summary>
        /// Updates the custom state of an object based on the time elapsed since the last update. 
        /// </summary>
        /// <param name="obj">The object that is to be updated. </param>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        void Update(TVal obj, int msElapsed);

        /// <summary>
        /// Gets whether an object should be removed from the map as soon as possible. 
        /// </summary>
        bool ShouldRemove(TVal obj);

        /// <summary>
        /// Returns whether this object keeps a bin alive by forcing it to update. 
        /// If all objects in a chunk return false, the chunk is marked as inactive. NYI. 
        /// </summary>
        /// <returns></returns>
        bool ForcesBinUpdates(TVal obj);
    }
}

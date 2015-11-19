using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using Engine.Events;

namespace Engine.Maps
{
    [Obsolete]
    internal class ObjectMap<T> : IEnumerable<T>
        where T : GameObject
    {
        //the underlying hashmap that stores the objects
        HashMap<T> map = new HashMap<T>(new Vector(Constants.ObjectMap.CellSize));

        //the units that are to be added on next update
        volatile List<T> pendingAdds = new List<T>();

        readonly object _addLock = new object();





        public void Add(T obj)
        {
            lock (_addLock)
                pendingAdds.Add(obj);
        }

        // process pending adds
        public void AddPendingObjects()
        {
            List<T> oldAddList;
            lock (_addLock)
                oldAddList = Interlocked.Exchange(ref pendingAdds, new List<T>());

            foreach (var obj in oldAddList)
                map.Add(obj, obj.Position);
        }

        /// <summary>
        /// Adds pending objects and removes dead guys.
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {

            foreach (var obj in map)
            {
                //update private -> public location
                obj.UpdateLocation();

                //update the object's location in the map. 
                map.Update(obj, obj.OldPosition, obj.Position);
            }

            // call object.Update
            foreach (var obj in map)
                obj.Update(msElapsed);

            //remove dead units
            var removedObjects = map.Where(obj => obj.IsDestroyed).ToArray();
            foreach (var obj in removedObjects)
            {
                map.Remove(obj, obj.Position);
            }
        }


        /// <summary>
        /// Executes a range query for the objects within the given rectangle. 
        /// </summary>
        /// <param name="pos">The bottom-left point of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>An enumeration of all objects within the rectangle. </returns>
        public IEnumerable<T> RangeQuery(Vector pos, Vector size)
        {
            return map.RangeQuery(pos, size);
        }


        public IEnumerable<T> RawQuery(Vector pos, Vector size)
        {
            return map.RawQuery(pos, size);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)map).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)map).GetEnumerator();
        }
    }
}

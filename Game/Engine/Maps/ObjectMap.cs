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

namespace Engine.Maps
{
    public class ObjectMap<T> : IEnumerable<T>
        where T : GameObject
    {
        //the underlying hashmap that stores the objects
        HashMap<T> map = new HashMap<T>(new Vector(7));

        //the units that are to be added on next update
        volatile List<T> pendingAdds = new List<T>();

        /// <summary>
        /// Raised whenever an object is added to the map. 
        /// </summary>
        public event Action<T> ObjectAdded;

        /// <summary>
        /// Raised whenever an object is removed from the map. 
        /// </summary>
        public event Action<T> ObjectRemoved;

        /// <summary>
        /// Raised whenever an object within the map is updated. 
        /// </summary>
        public event Action<T> ObjectUpdate;


        public void Add(T obj)
        {
            lock (pendingAdds)
                pendingAdds.Add(obj);
        }

        /// <summary>
        /// Adds pending objects and removes dead guys.
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {
            // add the pending objects
            var newAddList = new List<T>();
            var oldAddList = Interlocked.Exchange(ref pendingAdds, newAddList);
            foreach (var obj in oldAddList.ToArray())
            {
                Debug.Assert(obj != null) ;

                map.Add(obj, obj.Position);
                ObjectAdded?.Invoke(obj);
            }

            //check there are no destroyed units before calling GameObject.Update
            foreach (var obj in map)
            {
                Debug.Assert(!obj.MarkedForDestruction && !obj.IsDestroyed) ;

                obj.Update(msElapsed);
            }

            foreach (var obj in map)
            {
                ObjectUpdate?.Invoke(obj);
                obj.UpdateLocation();
                map.Update(obj, obj.OldPosition, obj.Position);
                obj.TryFinalise();
            }

            //remove dead units
            var deadObjs = map.Where(obj => obj.IsDestroyed).ToArray();
            foreach (var obj in deadObjs)
            {
                map.Remove(obj, obj.Position);
                ObjectRemoved?.Invoke(obj);
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

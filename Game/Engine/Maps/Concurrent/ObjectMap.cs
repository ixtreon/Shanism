using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps.Concurrent
{
    /// <summary>
    /// A map which keeps track of instances of a <see cref="GameObject"/>. 
    /// 
    /// Internally uses a <see cref="HashMap<T>"/>. 
    /// </summary>
    /// <typeparam name="T">The type of <see cref="GameObject"/> to track. </typeparam>
    [Obsolete]
    public class ObjectMap<T> : IEnumerable<T>
        where T : GameObject
    {

        /// <summary>
        /// The underlying HashMap which keeps track of the objects. 
        /// </summary>
        readonly HashMap<T> HashMap;

        /// <summary>
        /// The size of the cells used by the HashMap instance. 
        /// </summary>
        public readonly int CellSize;

        /// <summary>
        /// The event fired right after a GameObject is added to the map. 
        /// </summary>
        public event Action<T> Added;

        /// <summary>
        /// The event fired just before a GameObject is destroyed and is to be removed from the map. 
        /// </summary>
        public event Action<T> Destroyed;

        /// <summary>
        /// The event fired whenever a GameObject in the map changes its location. 
        /// </summary>
        public event Action<T> Moved;


        public ObjectMap(int cellSize = 5)
        {
            HashMap = new HashMap<T>(new Vector(CellSize));
            CellSize = cellSize;
        }


        /// <summary>
        /// Executes a range query for the objects within the given rectangle. 
        /// </summary>
        /// <param name="pos">The bottom-left point of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>An enumeration of all objects within the rectangle. </returns>
        public IEnumerable<T> RangeQuery(Vector pos, Vector size)
        {
            return HashMap.RangeQuery(pos, size);
        }

        /// <summary>
        /// Adds the specified item to the ObjectMap/ 
        /// </summary>
        /// <param name="item">The item to add. </param>
        public void Add(T item)
        {
            HashMap.Add(item, item.Position);

            if (Added != null)
                Added(item);
        }

        void item_LocationChanged(GameObject item)
        {
            HashMap.UpdateItem((T)item, item.OldPosition, item.Position);

            if (Moved != null)
                Moved((T)item);
        }

        void item_Destroyed(ScenarioObject item)
        {
            if (Destroyed != null)
                Destroyed((T)item);

            HashMap.TryRemove((T)item, ((T)item).Position);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var it in HashMap.Items)
                yield return it;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

using Ix.Math;
using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Range;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Engine.Systems
{

    /// <summary>
    /// Tracks mutual distances between entities in the game, using the MapSystem.
    /// </summary>
    sealed class UnitRangeSystem : UnitSystem
    {
        static readonly GenericComparer<RangeEvent> eventComparer
            = new GenericComparer<RangeEvent>((a, b) => a.Range.CompareTo(b.Range));

        static readonly int VectorSize = Vector<float>.Count;
        static readonly Vector<float> MoveEpsilon = new Vector<float>(1e-3f);


        readonly Unit owner;
        readonly IGameMap map;
        readonly SortedList<RangeEvent, bool> events = new SortedList<RangeEvent, bool>(eventComparer);

        readonly List<Entity> entities = new List<Entity>();
        readonly List<float> distances = new List<float>();
        readonly List<int> toRemove = new List<int>();
        readonly HashSet<uint> ids = new HashSet<uint>();

        readonly float[] xBuffer = new float[VectorSize];
        readonly float[] yBuffer = new float[VectorSize];
        readonly float[] dBuffer = new float[VectorSize];


        public IEnumerable<RangeEvent> SortedEvents => events.Keys;


        public UnitRangeSystem(Unit owner)
        {
            this.owner = owner;
            map = owner.Map;
        }


        public void AddEvent(RangeEvent e) => events.Add(e, true);

        public bool RemoveEvent(RangeEvent e) => events.Remove(e);

        internal void FireEvents()
        {
            var ownerPos = owner.Position;

            // if dead, leave this world
            if(owner.IsDead)
            {
                for(int i = entities.Count - 1; i >= 0; i--)
                {
                    var e = entities[i];
                    var d = ownerPos.DistanceToSquared(e.Position);
                    for(int j = 0; j < events.Count; j++)
                    {
                        var ev = events.Keys[j];

                        if(d > ev.RangeSquared)
                            break;
                        ev.Raise(e, RangeEventTriggerType.Leave);
                    }
                }

                entities.Clear();
                return;
            }

            if(events.Count == 0)
                return;

            var ownerX = new Vector<float>(ownerPos.X);
            var ownerY = new Vector<float>(ownerPos.Y);
            var maxEvent = events.Keys[events.Count - 1];
            var rangeSq = new Vector<float>(maxEvent.RangeSquared);

            addUnitsInRange(owner.Map, owner.Position, new Vector2(maxEvent.Range));

            toRemove.Clear();
            for(int i = 0; i < entities.Count; i += VectorSize)
            {
                // load nearby units into a SIMD buffer
                var nElems = loadBuffers(i, entities.Count - i);

                // calc squared distances
                var dx = new Vector<float>(xBuffer) - ownerX;
                var dy = new Vector<float>(yBuffer) - ownerY;
                var newDist = (dx * dx + dy * dy);

                // outputs
                var oldDist = new Vector<float>(dBuffer);

                //get min/max of cur/old dist
                var minDists = Vector.Min(newDist, oldDist);
                var maxDists = Vector.Max(newDist, oldDist);
                // relative move?
                var haveMoved = Vector.GreaterThan(Vector.Abs(newDist - oldDist), MoveEpsilon);

                // went too far?
                var tooFarAway = Vector.GreaterThan(newDist, rangeSq);

                // move towards or away from us (hackyyyyy)
                var ttys = Vector<int>.Zero - Vector.AsVectorInt32(Vector.GreaterThan(newDist, oldDist));

                // eval results
                for(int j = 0; j < nElems; j++)
                {
                    var id = i + j;
                    var other = entities[id];

                    if(haveMoved[j] == 0)
                        continue;

                    // loop thru events
                    // TODO: vectorize in case each unit has > 4 events or so :D :D
                    var tty = (RangeEventTriggerType)ttys[j];
                    var nEvents = events.Count;
                    var evId = 0;
                    RangeEvent ev;

                    var minDist = minDists[j];
                    while(evId < nEvents && events.Keys[evId].RangeSquared <= minDist)
                        evId++;

                    var maxDist = maxDists[j];
                    while(evId < nEvents && (ev = events.Keys[evId]).RangeSquared <= maxDist)
                    {
                        ev.Raise(other, tty);
                        evId++;
                    }

                    distances[id] = newDist[j];   // update dist

                    if(tooFarAway[j] != 0 || other.IsDestroyed)
                        toRemove.Add(id);             // mark for removal
                }
            }

            cleanUpEntities();
        }


        void addUnitsInRange(IGameMap map, Vector2 origin, Vector2 range)
        {
            var nUnitsBefore = entities.Count;
            var rect = new RectangleF(origin - range / 2, range);
            map.GetObjectsInRect(rect, entities);

            // update ID and Dist maps
            updateDistances(nUnitsBefore);
        }

        void updateDistances(int start)
        {
            for(int i = start; i < entities.Count; i++)
            {
                var e = entities[i];
                if(ids.Add(e.Id))
                    distances.Add(float.MaxValue);    // comes from infinitely far
                else
                    entities.RemoveAtFast(i--);
            }
        }

        int loadBuffers(int offset, int maxUnits)
        {
            var n = Math.Min(maxUnits, VectorSize);

            for(int i = 0; i < n; i++)
            {
                int entityId = offset + i;
                var entityPos = entities[entityId].Position;

                xBuffer[i] = entityPos.X;
                yBuffer[i] = entityPos.Y;
                dBuffer[i] = distances[entityId];
            }

            return n;
        }

        void cleanUpEntities()
        {
            // Important to have 'nearbyToRemove' sorted..
            // Iterates in reverse order as otherwise 
            // the values in 'nearbyToRemove' become invalid

            var lastId = entities.Count - 1;
            int nToRemove = toRemove.Count;

            for(int i = nToRemove - 1; i >= 0; i--, lastId--)
            {
                var id = toRemove[i];
                var e = entities[id];

                ids.Remove(e.Id);

                entities[id] = entities[lastId];
                distances[id] = distances[lastId];
            }

            var removeAt = entities.Count - nToRemove;
            entities.RemoveRange(removeAt, nToRemove);
            distances.RemoveRange(removeAt, nToRemove);
        }

    }
}

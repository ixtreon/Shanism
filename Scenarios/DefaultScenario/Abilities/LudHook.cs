
using Engine.Entities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;
using Engine.Entities;
using IO.Common;
using IO;
using Engine.Systems.Abilities;

namespace DefaultScenario.Abilities
{
    class LudHook : Ability
    {

        static Effect headProto = new Effect
        {
            ModelName = "spark",
            Scale = 0.6,
        };

        static Effect bodyProto = new Effect
        {
            ModelName = "spark",
            Scale = 0.3,
        };

        const double AoE = 0.2;
        const double HookSpeed = 10;  //in sq/sec
        const double FPS = 100;
        const int FramesPerSegment = 3;

        const double FrameDelay = 1 / FPS;
        const double DistancePerFrame = HookSpeed / FPS;
        const double DistancePerSegment = DistancePerFrame * FramesPerSegment;

        double Damage = 50;


        public LudHook()
        {
            TargetType = AbilityTargetType.PointTarget;

            Name = "LudHook";
            Description = "Hooks a targer";
            Cooldown = 1000;
            ManaCost = 10;
            CastRange = 5;
        }


        Effect hookHead;
        LinkedList<Effect> hookSegments = new LinkedList<Effect>();

        Unit hookedUnit;

        protected override async void OnCast(AbilityCastArgs e)
        {
            //init vars
            hookSegments.Clear();
            hookedUnit = null;

            hookHead = new Effect(headProto) { Position = Owner.Position };
            Map.Add(hookHead);
            
            var u = await extendHook(e.TargetLocation);

            if(u != null)
                Owner.DamageUnit(u, DamageType.Physical, Damage);

            retractHook();
        }

        async Task<Unit> extendHook(Vector targetLoc)
        {
            var sleepDuration = TimeSpan.FromSeconds(FrameDelay);
            var distTravelled = 0.0;
            //var segmentCounter = new Counter(FramesPerSegment);

            var tarAngle = Owner.Position.AngleTo(targetLoc);
            targetLoc = Owner.Position.PolarProjection(tarAngle, CastRange);
            
            while (targetLoc.DistanceTo(hookHead.Position) > 2 * DistancePerFrame)
            {

                //move head towards target
                moveTowards(hookHead, targetLoc, DistancePerFrame);
                distTravelled += DistancePerFrame;

                //drag hook behind head
                dragChain(hookHead.Position, hookSegments.Reverse(), DistancePerSegment);

                //create new segment
                do
                {
                    //check distance from first segment to caster
                    var firstPart = hookSegments?.First?.Value ?? hookHead;
                    var dist = firstPart.Position.DistanceTo(Owner.Position);
                    if (dist < DistancePerSegment)
                        break;

                    //if larger than treshold, extend the hook
                    var ang = firstPart.Position.AngleTo(Owner.Position);
                    var newPos = firstPart.Position.PolarProjection(ang, DistancePerSegment);

                    var hookPart = new Effect(bodyProto) { Position = newPos };
                    Map.Add(hookPart);
                    hookSegments.AddFirst(hookPart);
                }
                while (true);

                //check if a unit was caught
                hookedUnit = Map.GetUnitsInRange(hookHead.Position, AoE)
                    .FirstOrDefault(uu => uu != Owner && uu.Owner.IsEnemyOf(Owner));
                if (hookedUnit != null)
                {
                    hookHead.Position = hookedUnit.Position;
                    return hookedUnit;
                }

                await Task.Delay(sleepDuration);
            }

            return null;
        }

        async void retractHook()
        {
            //retract hook pieces
            while(hookSegments.Any())
            {
                //move closest part toward hero
                var hookPart = hookSegments.First();
                moveTowards(hookPart, Owner.Position, DistancePerFrame);

                //hide it if it's close enough
                if (hookPart.Position.DistanceTo(Owner.Position) < 2 * DistancePerFrame)
                {
                    hookSegments.RemoveFirst();
                    hookPart.Destroy();
                }

                //drag remaining segments behind hero
                dragChain(Owner.Position, hookSegments, DistancePerSegment);

                //drag head
                if(hookSegments.Any())
                    dragTowards(hookSegments.Last.Value.Position, hookHead, DistancePerSegment);

                //drag target
                if (hookedUnit != null)
                    hookedUnit.Position = hookHead.Position;

                await Task.Delay(TimeSpan.FromSeconds(FrameDelay));
            }

            //retract hook head
            hookHead.Destroy();
        }

        static void dragChain(Vector anchor, IEnumerable<GameObject> objs, double dist)
        {
            //segments
            var originPos = anchor;
            foreach(var hookPart in objs)
            {
                dragTowards(originPos, hookPart, dist);

                originPos = hookPart.Position;
            }

        }

        /// <summary>
        /// Makes sure the object is no less than X range away from the target.  
        /// </summary>
        static void dragTowards(Vector targetPos, GameObject obj, double maxDist)
        {
            var ang = targetPos.AngleTo(obj.Position);
            var objDist = targetPos.DistanceTo(obj.Position);
            var finalDist = Math.Min(objDist, maxDist);

            obj.Position = targetPos.PolarProjection(ang, finalDist);
        }

        /// <summary>
        /// Makes sure the target moves towards the anchor by (at most) X units. 
        /// </summary>
        static void moveTowards(GameObject dragged, Vector anchorPos, double maxDist)
        {
            var ang = anchorPos.AngleTo(dragged.Position);
            var objDist = anchorPos.DistanceTo(dragged.Position);
            var finalDist = Math.Max(objDist - maxDist, 0);

            dragged.Position = anchorPos.PolarProjection(ang, finalDist);
        }
    }
}

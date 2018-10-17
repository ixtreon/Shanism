using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using XnaVector = Microsoft.Xna.Framework.Vector2;

namespace Shanism.Engine.GameSystems
{
    class PhysicsSystem
    {
        readonly List<(Entity, Body)> bodies = new List<(Entity, Body)>();
        readonly World world;

        public PhysicsSystem()
        {
            world = new World(XnaVector.Zero);
        }

        public void Add(Entity e)
        {
            var radius = e.Scale / 2;
            var type = (e is Doodad) ? BodyType.Static : BodyType.Kinematic;
            var body = BodyFactory.CreateEllipse(world, radius, radius, 8, 1f, new XnaVector(e.Position.X, e.Position.Y), 0, type);

            bodies.Add((e, body));
        }

        public void Update(int msElapsed)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                var (e, b) = bodies[i];
                if (e is Unit u)
                {
                    var ang = u.MovementState.MoveDirection;
                    b.LinearVelocity = u.MoveSpeed * new XnaVector((float)Math.Cos(ang), (float)Math.Sin(ang));
                }

                b.Position = new XnaVector(e.Position.X, e.Position.Y);
            }

            world.Step((float)msElapsed / 1000);

        }

        void ResizeEllipse(Body b)
        {
            foreach (var f in b.FixtureList)
            {
                //f.Shape.Radius
            }
        }
    }

}

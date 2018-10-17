using Ix.Math;
using NUnit.Framework;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Engine.Tests
{
    [TestFixture]
    public class QuadTreeTests
    {
        const int BoardRadius = 100;
        const int BoardSize = BoardRadius * 2;
        const int BoardArea = BoardSize * BoardSize;

        static readonly RectangleF board = new RectangleF(-BoardRadius, -BoardRadius, BoardSize, BoardSize);


        [Test]
        public void QC_RectMoves([Range(0, 1)] int seed)
        {
            //const int seed = 42;
            const int NEntities = 1_000;
            const int NFrames = 100;
            const int NQueries = 10;

            var rnd = new Random(seed);

            var entities = makeEntities(rnd, NEntities);

            var tree = new TestTree(board, 1);
            for(int i = 0; i < entities.Count; i++)
                tree.Add(entities[i]);


            for(int i = 0; i < NFrames; i++)
            {
                for(int j = 0; j < entities.Count; j++)
                    entities[j].MoveTo(rnd.NextVector(board.Deflate(entities[j].Size / 2)));

                for(int j = NFrames % 3; j < entities.Count; j += 3)
                {
                    entities[j].IsAlive = false;

                    entities[j] = mkEnt(rnd);
                    tree.Add(entities[j]);
                }

                tree.Update(out var outNodes);
                Assert.IsEmpty(outNodes);

                query(entities, tree, rnd, NQueries);
            }
        }


        [Test]
        public void QC_Rect([Range(0, 1)] int seed)
        {
            //const int seed = 42;
            const int NEntities = 1_000;
            const int NQueries = 100;

            var rnd = new Random(seed);
            var entities = makeEntities(rnd, NEntities);
            var tree = new TestTree(board, 1);

            for(int i = 0; i < entities.Count; i++)
            {
                tree.AssertDebugCount();
                tree.Add(entities[i]);
            }

            query(entities, tree, rnd, NQueries);
        }

        [Test]
        public void QC_Circle()
        {
            const int NCases = 100;
            const int QueriesPerCase = 100;
            const int EntitiesPerQuery = 1_000;

            var rnd = new Random(1234);

            for(int asd = 0; asd < NCases; asd++)
            {
                var entities = makeEntities(rnd, EntitiesPerQuery);
                int queryNaive(Ellipse q) => entities.Count(e => e.Inside(ref q));

                var tree = new TestTree(board, 1);
                int queryTree(Ellipse q) { var l = new List<Entity>(); tree.QueryEllipse(q, l); return l.Count; }

                for(int i = 0; i < entities.Count; i++)
                {
                    tree.AssertDebugCount();
                    tree.Add(entities[i]);
                }

                for(int i = 0; i < QueriesPerCase; i++)
                {
                    var queryRect = ellipse(rnd, BoardArea);

                    var qNaive = queryNaive(queryRect);
                    var qTree = queryTree(queryRect);

                    Assert.AreEqual(qNaive, qTree);
                }

            }
        }

        void query(IEnumerable<Entity> entities, TestTree tree, Random rnd, int nQueries)
        {
            for(int i = 0; i < nQueries; i++)
            {
                var queryRect = rect(rnd, BoardArea);

                var qNaive = queryNaive(entities, queryRect);
                var qTree = queryTree(tree, queryRect);

                Assert.AreEqual(qNaive, qTree);
            }
        }

        int queryNaive(IEnumerable<Entity> entities, RectangleF q)
            => entities.Count(e => e.Inside(ref q));

        int queryTree(TestTree tree, RectangleF q)
        {
            var l = new List<Entity>();
            tree.QueryRect(q, l);
            return l.Count;
        }

        static Ellipse ellipse(Random rnd, float area)
        {
            var r = rect(rnd, area);
            return new Ellipse(r.Center, r.Size / 2);
        }

        static RectangleF rect(Random rnd, float area)
        {
            var queryArea = area * rnd.NextFloat(0.05f, 0.95f);
            var qw = (float)Math.Sqrt(queryArea / rnd.NextFloat(0.1f, 10f));
            var qh = queryArea / qw;
            var qSz = new Vector2(qw, qh);
            var qPos = board.Position + rnd.NextVector(board.Size - qSz);

            var q = new RectangleF(qPos, qSz);
            return q;
        }

        IList<Entity> makeEntities(Random rnd, int n)
        {
            var datas = new Entity[n];
            for(int i = 0; i < n; i++)
                datas[i] = mkEnt(rnd);
            return datas;
        }

        Entity mkEnt(Random rnd) => new Entity(rnd.NextVector(board.Deflate(0.5f)), 1);

        const int PerfQueries = 10_000;
        const int PerfEntities = 10_000;

        [Test]
        public void PerfTree()
        {
            var rnd = new Random(1234);
            var tree = new TestTree(board, 1);
            var entities = makeEntities(rnd, PerfEntities);

            for(int i = 0; i < entities.Count; i++)
                tree.Add(entities[i]);

            for(int i = 0; i < PerfQueries; i++)
            {
                var queryRect = rect(rnd, BoardArea);
                var l = new List<Entity>();
                tree.QueryRect(queryRect, l);
            }
        }
        [Test]
        public void PerfNaive()
        {
            var rnd = new Random(1234);
            var entities = makeEntities(rnd, PerfEntities);

            for(int i = 0; i < PerfQueries; i++)
            {
                var queryRect = rect(rnd, BoardArea);
                var l = new List<Entity>();
                for(int j = 0; j < entities.Count; j++)
                    if(entities[j].Inside(ref queryRect))
                        l.Add(entities[j]);
            }
        }

        [Test]
        public void Blah2()
        {
            var tree = new TestTree(new RectangleF(0, 0, 100, 100), 1, 1);

            tree.Add(new Entity(new Vector2(0.5f, 0.5f)));
            tree.Add(new Entity(new Vector2(50.499f, 0.5f)));


            tree.Add(new Entity(new Vector2(50.501f, 0.5f)));
            tree.Add(new Entity(new Vector2(55.5f, 1.5f)));
            tree.Add(new Entity(new Vector2(1.5f, 55.5f)));


            var result = new List<Entity>();
            tree.QueryRect(new RectangleF(0, 0, 50, 50), result);

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Blah3()
        {
            var tree = new TestTree(new RectangleF(0, 0, 100, 100), 1, 1);

            tree.Add(new Entity(new Vector2(0.5f, 0.5f)));
            tree.Add(new Entity(new Vector2(50, 1.5f)));
            tree.Add(new Entity(new Vector2(1.5f, 50)));

            var result = new List<Entity>();
            tree.QueryRect(new RectangleF(0, 0, 50, 50), result);

            Assert.AreEqual(3, result.Count);
        }


        class Entity
        {
            public float Size { get; }

            public bool IsAlive { get; set; } = true;

            public RectangleF BoundingBox { get; private set; }

            public Entity(Vector2 pos, float size = 1)
            {
                this.Size = size;
                MoveTo(pos);
            }


            public void MoveTo(Vector2 pos)
            {
                BoundingBox = new RectangleF(pos - new Vector2(Size / 2), new Vector2(Size));
            }

            public bool Inside(ref RectangleF rect)
                => rect.Intersect(BoundingBox).Area > 0;

            public bool Inside(ref Ellipse e)
                => e.Intersects(BoundingBox);

            public override string ToString()
                => BoundingBox.Position.ToString();
        }

        class TestTree : Ix.Collections.QuadTree<Entity>
        {
            public TestTree(RectangleF span, float minCellSize = 0, int maxItemsPerNode = 16, int initialNodeCapacity = 64)
                : base(span, minCellSize, maxItemsPerNode, initialNodeCapacity)
            {

            }

            protected override RectangleF GetBounds(Entity item) => item.BoundingBox;
            protected override bool GetIsAlive(Entity item) => item.IsAlive;

        }


    }
}

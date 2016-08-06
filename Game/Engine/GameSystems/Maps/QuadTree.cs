using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.GameSystems.Maps
{
    class QuadTree<T>
    {
        const int SplitTreshold = 16;
        public const int DefaultMinimumRange = 0;

        public struct Node : IEquatable<Node>
        {
            public readonly Vector Position;
            public readonly T Item;

            public Node(T item, Vector position)
            {
                Item = item;
                Position = position;
            }

            public bool Equals(Node other)
                => Item.Equals(other.Item)
                && Position.Equals(other.Position);
        }

        readonly Vector center, range;
        readonly double minimumRange;
        readonly bool canSplit;
        readonly List<Node> leafNodes = new List<Node>();

        bool isLeaf = true;
        int count = 0;
        QuadTree<T> botLeft, botRight, topLeft, topRight;


        public QuadTree(Vector center, Vector range, double minimumRange = DefaultMinimumRange)
        {
            if (range.X <= 0 || range.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(range));
            if (minimumRange < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumRange));

            this.center = center;
            this.range = range;
            this.minimumRange = minimumRange;
            this.canSplit = (range.X > minimumRange * 2) && (range.Y > minimumRange * 2);
        }

        public RectangleF Bounds => new RectangleF(center - range, range * 2);

        public bool IsLeaf => isLeaf;

        public int Count => count;

        public IEnumerable<QuadTree<T>> Branches
            => new[] { topLeft, botLeft, topRight, botRight };

        public void Add(T item, Vector pos)
            => add(new Node(item, pos));

        public bool Remove(T item, Vector oldPos)
            => remove(new Node(item, oldPos));

        public bool Update(T item, Vector oldPos, Vector newPos)
        {
            if (oldPos == newPos)
                return true;

            return replace(new Node(item, oldPos), new Node(item, newPos));
        }

        public void Clear()
        {
            if (isLeaf)
                leafNodes.Clear();
            else
                clearBranches();

            count = 0;
            isLeaf = true;
        }

        void clearBranches()
        {
            topLeft.Clear(); topRight.Clear();
            botLeft.Clear(); botRight.Clear();
        }

        void add(Node n)
        {
            count++;

            if (isLeaf)
            {
                leafNodes.Add(n);

                //turn into a branched one if too big
                if (canSplit && leafNodes.Count >= SplitTreshold)
                {
                    isLeaf = false;
                    initBranches();
                    foreach (var nn in leafNodes)
                        addToBranch(nn);
                    leafNodes.Clear();
                }

                Debug.Assert(countNodes() == count);
                return;
            }

            addToBranch(n);
            Debug.Assert(countNodes() == count);
        }


        bool remove(Node item)
        {
            if (isLeaf)
            {
                if (leafNodes.Remove(item))
                {
                    count--;

                    Debug.Assert(countNodes() == count);
                    return true;
                }

                Debug.Assert(countNodes() == count);
                return false;
            }

            var br = getBranch(item.Position);
            if (br.remove(item))
            {
                count--;

                //turn into a leaf if too small
                if (count < SplitTreshold)
                {
                    leafNodes.AddRange(getNodes());
                    clearBranches();
                    isLeaf = true;
                }

                Debug.Assert(countNodes() == count);
                return true;
            }

            Debug.Assert(countNodes() == count);
            return false;
        }

        bool replace(Node oldNode, Node newNode)
        {
            if (isLeaf)
            {
                var id = leafNodes.IndexOf(oldNode);
                if (id >= 0 && id < leafNodes.Count)
                {
                    leafNodes[id] = newNode;
                    return true;
                }
                return false;
            }

            var oldBr = getBranch(oldNode.Position);
            var newBr = getBranch(newNode.Position);

            //if both in the same quadrant, recurse
            if (oldBr == newBr)
                return newBr.replace(oldNode, newNode);

            //otherwise remove from old, add to new
            if (!oldBr.remove(oldNode))
                return false;

            newBr.add(newNode);
            return true;
        }

        public ICollection<T> Query(Vector orign, Vector range)
        {
            var l = new List<T>();
            Query(orign, range, l);
            return l;
        }

        public void Query(Vector qOrigin, Vector qRange, ICollection<T> lst)
        {
            if (isLeaf)
                queryLeafNodes(qOrigin, qRange, lst);
            else
                query(this, qOrigin, qRange, lst);
        }

        void queryLeafNodes(Vector qOrigin, Vector qRange, ICollection<T> lst)
        {
            for (int i = 0; i < leafNodes.Count; i++)
            {
                var n = leafNodes[i];
                if (isInside(qOrigin, qRange, n.Position))
                    lst.Add(n.Item);
            }
        }

        static void query(QuadTree<T> root, Vector qOrigin, Vector qRange, ICollection<T> lst)
        {
            var stack = new Stack<QuadTree<T>>();
            stack.Push(root);

            QuadTree<T> curTree;
            do
            {
                curTree = stack.Pop();

                if (curTree == null)
                    throw new Exception();

                if (curTree.isLeaf)
                {
                    curTree.queryLeafNodes(qOrigin, qRange, lst);
                    continue;
                }

                var c = curTree.center;

                var l = qOrigin.X - qRange.X < c.X;
                var r = qOrigin.X + qRange.X > c.X;
                var b = qOrigin.Y - qRange.Y < c.Y;
                var t = qOrigin.Y + qRange.Y > c.Y;

                if (l && b) stack.Push(curTree.botLeft);
                if (l && t) stack.Push(curTree.topLeft);
                if (r && b) stack.Push(curTree.botRight);
                if (r && t) stack.Push(curTree.topRight);
            }
            while (stack.Count > 0);
        }


        int countNodes() => isLeaf 
            ? leafNodes.Count
            : topLeft.count + topRight.count + botLeft.count + botRight.count;

        void addToBranch(Node n) 
            => getBranch(n.Position).add(n);

        bool isInside(Vector o, Vector r, Vector p)
            => Math.Abs(o.X - p.X) <= r.X
            && Math.Abs(o.Y - p.Y) <= r.Y;

        QuadTree<T> getBranch(Vector pos)
        {
            if (pos.X < center.X)
            {
                if (pos.Y < center.Y)
                    return botLeft;
                else
                    return topLeft;
            }
            else
            {
                if (pos.Y < center.Y)
                    return botRight;
                else
                    return topRight;
            }
        }

        void initBranches()
        {
            var nr = range / 2;
            botLeft = new QuadTree<T>(center + new Vector(-nr.X, -nr.Y), nr, minimumRange);
            topLeft = new QuadTree<T>(center + new Vector(-nr.X, nr.Y), nr, minimumRange);
            botRight = new QuadTree<T>(center + new Vector(nr.X, -nr.Y), nr, minimumRange);
            topRight = new QuadTree<T>(center + new Vector(nr.X, nr.Y), nr, minimumRange);
        }

        IEnumerable<Node> getNodes()
        {
            if (isLeaf)
                foreach (var e in leafNodes)
                    yield return e;
            else
            {
                foreach (var e in botLeft.getNodes()) yield return e;
                foreach (var e in botRight.getNodes()) yield return e;
                foreach (var e in topLeft.getNodes()) yield return e;
                foreach (var e in topRight.getNodes()) yield return e;
            }
        }

        public IEnumerable<Node> Nodes => getNodes();

        public override string ToString()
        {
            var type = isLeaf ? "Leaf" : "Branch";
            return $"{type} @ {center} ({count} elements)";
        }

    }
}

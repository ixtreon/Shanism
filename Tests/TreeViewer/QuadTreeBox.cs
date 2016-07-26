using Shanism.Common;
using Shanism.Engine.GameSystems.Maps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeViewer
{
    class QuadTreeBox : UserControl
    {
        public QuadTree<TestNode> Tree { get; set; }

        double treeWidth => Tree.Bounds.Width;
        double treeHeight => Tree.Bounds.Height;
        double treeRatio => treeWidth / treeHeight;
        double drawWidth => Math.Min(Width, Height * treeRatio);
        double drawHeight => Math.Min(Height, Width / treeRatio);
        Vector drawSize => new Vector(drawWidth, drawHeight);
        Vector scale => drawSize / Tree.Bounds.Size;

        QuadTree<TestNode>.Node? selectedNode;
        Shanism.Common.Point mouseDownPoint;

        string nodeCount = string.Empty;

        public QuadTreeBox()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        Vector treeToScreen(Vector p)
        {
            return (p - Tree.Bounds.Position) * scale;
        }

        Vector screenToTree(Vector p)
        {
            return p / scale + Tree.Bounds.Position;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Tree == null) return;

            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = new Shanism.Common.Point(e.X, e.Y);

                var elems = Tree.Nodes;
                if (elems.Any())
                {
                    var mousePos = screenToTree(new Vector(e.X, e.Y));
                    var minDist = 5 / scale.X;

                    var n = elems
                        .ArgMin(el => mousePos.DistanceToSquared(el.Position));

                    if (n.Position.DistanceTo(mousePos) < minDist)
                        selectedNode = n;
                    else
                        selectedNode = null;

                    Invalidate();
                }
            }
        }

        readonly HashSet<TestNode> queriedNodes = new HashSet<TestNode>();

        void query(Shanism.Common.RectangleF rect)
        {
            queriedNodes.Clear();
            Tree.Query(rect.Center, rect.Size / 2, queriedNodes);

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Tree == null) return;

            if (e.Button == MouseButtons.Left)
            {
                var mouseUpPoint = new Vector(e.X, e.Y);
                if (selectedNode != null)
                {
                    var n = selectedNode.Value;
                    var newPos = screenToTree(new Vector(e.X, e.Y));

                    if (Tree.Update(n.Item, n.Position, newPos))
                        selectedNode = new QuadTree<TestNode>.Node(n.Item, newPos);

                    selectedNode = null;
                    Invalidate();
                }

                else if (mouseUpPoint.DistanceTo(mouseDownPoint) > 10)
                {
                    var st = screenToTree(mouseDownPoint);
                    var end = screenToTree(mouseUpPoint);
                    var r = new Shanism.Common.RectangleF(st, end - st)
                        .MakePositive();

                    query(r);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var pt = screenToTree(new Vector(e.X, e.Y));
                var n = new TestNode();

                Tree.Add(n, pt);
                Invalidate();
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (selectedNode != null)
                {
                    //var n = selectedNode.Value;
                    //var newPos = screenToTree(new Vector(e.X, e.Y));
                    //if (Tree.Update(n.Item, n.Position, newPos))
                    //    selectedNode = new QuadTree<TestNode>.Node(n.Item, newPos);
                    
                    //Invalidate();
                }
            }
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            nodeCount = $"{Tree?.Count ?? -1} nodes";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            if (Tree == null)
                return;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.White, 0, 0, (float)drawWidth, (float)drawHeight);

            g.DrawString(nodeCount, SystemFonts.DefaultFont, Brushes.Black, 10, 10);

            var s = new Stack<QuadTree<TestNode>>();
            s.Push(Tree);
            while (s.Any())
            {
                var curNode = s.Pop();
                if (curNode.IsLeaf)
                {
                    foreach (var n in curNode.Nodes)
                    {
                        const int ptSize = 5;

                        var isInside = curNode.Bounds.Contains(n.Position);
                        var p = treeToScreen(n.Position);
                        var c = (queriedNodes.Contains(n.Item)) ? Color.LimeGreen : Color.DarkRed;

                        if (selectedNode.Equals(n))
                            c = ControlPaint.DarkDark(c);

                        if (!isInside)
                            c = ControlPaint.Light(c);

                        using (var br = new SolidBrush(c))
                            g.FillEllipse(br, (float)(p.X - ptSize), (float)(p.Y - ptSize), ptSize * 2, ptSize * 2);
                    }
                }
                else
                {
                    var cp = treeToScreen(curNode.Bounds.Center);
                    var p = treeToScreen(curNode.Bounds.Position);
                    var pf = treeToScreen(curNode.Bounds.FarPosition);

                    g.DrawLine(Pens.Black, (float)cp.X, (float)p.Y, (float)cp.X, (float)pf.Y);
                    g.DrawLine(Pens.Black, (float)p.X, (float)cp.Y, (float)pf.X, (float)cp.Y);

                    foreach (var nnn in curNode.Branches)
                        s.Push(nnn);
                }
            }

        }


    }
}

using Shanism.Client.UI.Containers;
using System;
using System.Collections.Generic;

namespace Shanism.Client.UI
{
    class DropDown<T> : Label
    {
        struct Node
        {
            public T Item { get; }
            public string Text { get; }

            public Node(T item) : this(item, item.ToString()) 
            { }

            public Node(T item, string text)
            {
                Item = item;
                Text = text;
            }
        }

        readonly ListPanel dropList = new ListPanel(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
        {
            ResizeAnchor = AnchorMode.Top | AnchorMode.Right,
        };

        readonly List<Node> nodes = new List<Node>();
        
        public event Action<T> ItemSelected;


        public DropDown()
        {
            FocusGained += (o, c) => Parent.Add(dropList);
            FocusLost += (o, c) => Parent.Remove(dropList);
        }

        protected override void OnFocusGained(EventArgs e)
        {
            base.OnFocusGained(e);

            dropList.Top = Bottom;
            dropList.Right = Right;
            Parent.Add(dropList);
        }

        protected override void OnFocusLost(EventArgs e)
        {
            base.OnFocusLost(e);

            Parent.Remove(dropList);
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        public void Add(T item, string text)
        {
            var n = new Node(item, text);
            nodes.Add(n);

            var lbl = new Label(text);
            lbl.MouseClick += (o, e) => select(item);
            dropList.Add(lbl);
        }

        void select(T item)
        {
            throw new NotImplementedException();
        }

        public virtual void OnItemSelected(T item)
            => ItemSelected?.Invoke(item);
    }
}

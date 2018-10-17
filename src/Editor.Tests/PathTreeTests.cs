using NUnit.Framework;
using Shanism.Editor.Models.Content;
using System.Linq;

namespace Client.Editor.Tests
{
    [TestFixture]
    public class PathTreeTests
    {

        [Test]
        public void Add_Single()
        {
            // add "hero"
            var tree = new PathTree<object>();
            tree.Add("hero", 42);

            // there's no "/" item, but there's one nested node
            Assert.IsFalse(tree.Root.HasItem);
            Assert.AreEqual(1, tree.Root.Nodes.Count);

            // try finding the node, legal aliases
            Assert.IsTrue(tree.TryFind("hero", out var _));
            Assert.IsTrue(tree.TryFind("/hero", out var _));
            Assert.IsTrue(tree.TryFind("hero/", out var _));
            Assert.IsTrue(tree.TryFind("///hero/", out var _));

            // try some queries that'd fail
            Assert.IsFalse(tree.TryFind("o/hero", out var _));
            Assert.IsFalse(tree.TryFind("hero/o", out var _));
        }

        [Test]
        public void Add_Nested()
        {
            // insert "hero/mage" + nested
            var tree = new PathTree<object>();
            tree.Add("hero/mage", 42);
            tree.Add("hero/mage/walk", 666);

            // there's no "/" item
            Assert.IsFalse(tree.Root.HasItem);

            // there's no "hero" item
            var heroNode = tree.Root.Nodes.Single().Value;
            Assert.IsFalse(heroNode.HasItem);
            Assert.AreEqual("hero", heroNode.Name);

            // there IS a "hero/mage" item
            var mageNode = heroNode.Nodes.Single().Value;
            Assert.IsTrue(mageNode.HasItem);
            Assert.AreEqual("hero/mage", mageNode.Name);

            // there IS a "hero/mage/walk"
            var mageWalkNode = mageNode.Nodes.Single().Value;
            Assert.IsTrue(mageWalkNode.HasItem);


            // try finding the nodes, legal aliases
            Assert.IsFalse(tree.TryFind("hero", out var _));

            Assert.IsTrue(tree.TryFind("hero/mage", out var _));
            Assert.IsTrue(tree.TryFind("hero///mage", out var _));

            Assert.IsTrue(tree.TryFind("hero/mage/walk", out var _));
            Assert.IsTrue(tree.TryFind("hero/mage/walk//", out var _));
            Assert.IsFalse(tree.TryFind("hero/mage/walk/*", out var _));
        }

        [Test]
        public void Rename_Single()
        {
            // add "hero"
            var tree = new PathTree<object>();
            tree.Add("hero", 42);

            // attempt renaming.. twice
            Assert.IsTrue(tree.Rename("hero////", "//tree///"));
            Assert.IsFalse(tree.Rename("hero////", "//tree///"));

            // there's no "/" item
            Assert.IsFalse(tree.Root.HasItem);
            Assert.AreEqual(1, tree.Root.Nodes.Count);

            // there's no "hero" item
            Assert.IsFalse(tree.TryFind("hero", out var _));

            // but there IS a "tree" item
            Assert.IsTrue(tree.TryFind("tree", out var _));
        }

        [Test]
        public void AdvancedRename()
        {
            var tree = new PathTree<object>();

            tree.Add("hero/mage", 42);
            tree.Add("hero/mage/walk", 666);

            Assert.IsTrue(tree.Rename("hero", "tree"));
            Assert.IsFalse(tree.Rename("hero/mage", "tree/birch"));
            Assert.IsFalse(tree.Rename("tree/mage", "tree/birch"));

            Assert.IsFalse(tree.Root.HasItem);
            Assert.AreEqual(2, tree.Root.Nodes.Count);

            Assert.IsFalse(tree.TryFind("hero", out var _));
            Assert.IsFalse(tree.TryFind("hero/mage", out var _));
            Assert.IsTrue(tree.TryFind("tree/birch", out var _));
            Assert.IsTrue(tree.TryFind("hero/mage/walk", out var _));
        }


        [Test]
        public void BasicRemove()
        {
            var tree = new PathTree<object>();
            tree.Add("hero", 42);

            Assert.IsTrue(tree.Remove("hero"));
            Assert.IsFalse(tree.Remove("hero"));
            Assert.IsFalse(tree.Remove("hero"));

            Assert.IsFalse(tree.Root.HasItem);
            Assert.AreEqual(0, tree.Root.Nodes.Count);

            Assert.IsFalse(tree.TryFind("hero", out var _));
            Assert.IsFalse(tree.TryFind("/hero", out var _));
            Assert.IsFalse(tree.TryFind("hero/", out var _));
            Assert.IsFalse(tree.TryFind("hero////", out var _));
        }
    }
}

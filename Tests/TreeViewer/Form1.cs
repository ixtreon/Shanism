using Shanism.Common;
using Shanism.Engine.GameSystems.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeViewer
{
    public partial class Form1 : Form
    {
        QuadTree<TestNode> tree;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            tree = new QuadTree<TestNode>(Vector.Zero, Vector.One);
            quadTreeBox1.Tree = tree;
            quadTreeBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tree != null)
            {
                quadTreeBox1.Invalidate();
            }
        }
    }
}

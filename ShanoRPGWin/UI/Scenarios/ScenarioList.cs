using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoRPGWin.UI.Scenarios
{
    public partial class ScenarioList : TreeView
    {
        ScenarioLibrary _library;
        public ScenarioLibrary Library
        {
            get { return _library; }
            set
            {
                if (value != _library)
                {
                    if (_library != null)
                    {
                        _library.ItemAdded -= OnLibraryItemAdded;
                        _library.ItemRemoved -= OnLibraryItemRemoved;
                    }
                    _library = value;
                    if (_library != null)
                    {
                        _library.ItemAdded += OnLibraryItemAdded;
                        _library.ItemRemoved += OnLibraryItemRemoved;
                    }

                    OnLibraryChanged();
                }
            }
        }

        public virtual void OnLibraryChanged()
        {
        }

        public virtual void OnLibraryItemRemoved(string path)
        {

        }

        public virtual void OnLibraryItemAdded(string path)
        {
            //this.Nodes.Add()
        }

        public ScenarioList()
        {
            InitializeComponent();
        }
    }
}

using ShanoEditor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.ScenarioViews
{
    /// <summary>
    /// Used with TreeViews to provide static access to images. 
    /// </summary>
    static class SingletonImageList
    {
        /// <summary>
        /// A list with icons for directory trees. 
        /// </summary>
        public static ImageList FolderImageList { get; }


        static SingletonImageList()
        {
            FolderImageList = new ImageList();
            FolderImageList.Images.Add(Resources.FileFolder);
            FolderImageList.Images.Add(Resources.FileBitmap);
        }
    }
}

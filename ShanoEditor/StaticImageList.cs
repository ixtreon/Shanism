using ShanoEditor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor
{
    /// <summary>
    /// Used with TreeViews to provide static access to images. 
    /// </summary>
    static class StaticImageList
    {
        /// <summary>
        /// A list with icons for directory trees. 
        /// </summary>
        public static ImageList FolderList { get; }


        static StaticImageList()
        {
            FolderList = new ImageList();
            FolderList.Images.Add(Resources.FileFolder);
            FolderList.Images.Add(Resources.FileBitmap);
        }
    }
}

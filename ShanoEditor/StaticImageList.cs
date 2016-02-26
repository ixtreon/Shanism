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
    static class ImageLists
    {
        /// <summary>
        /// A list with icons for directory trees. 
        /// </summary>
        public static ImageList FolderList { get; }


        public static ImageList AnimationList { get; }

        static ImageLists()
        {
            FolderList = new ImageList();
            FolderList.Images.Add(Resources.FileFolder);
            FolderList.Images.Add(Resources.FileBitmap);

            AnimationList = new ImageList();
            AnimationList.Images.Add(Resources.FileFolder);
            AnimationList.Images.Add(Resources.ActionEvent);
        }
    }
}

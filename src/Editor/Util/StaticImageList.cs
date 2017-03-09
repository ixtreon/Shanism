using Shanism.Editor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor
{
    /// <summary>
    /// Used with TreeViews to provide static access to images. 
    /// </summary>
    static class ImageLists
    {
        /// <summary>
        /// A list with icons for directory trees. 
        /// </summary>
        public static ImageList DefaultList { get; }


        public static ImageList AnimationList { get; }

        static ImageLists()
        {
            DefaultList = new ImageList();
            DefaultList.Images.Add(Resources.FileFolder);
            DefaultList.Images.Add(Resources.FileBitmap);
            DefaultList.Images.Add(Resources.ActionEvent);

            AnimationList = new ImageList();
            AnimationList.Images.Add(Resources.FileFolder);
            AnimationList.Images.Add(Resources.ActionEvent);
        }
    }
}

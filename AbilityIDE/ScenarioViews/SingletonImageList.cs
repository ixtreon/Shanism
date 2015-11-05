using AbilityIDE.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE.ScenarioViews
{
    static class SingletonImageList
    {
        public static ImageList FolderImageList { get; }


        static SingletonImageList()
        {
            FolderImageList = new ImageList();
            FolderImageList.Images.Add(Resources.FileFolder);
            FolderImageList.Images.Add(Resources.FileBitmap);
        }
    }
}

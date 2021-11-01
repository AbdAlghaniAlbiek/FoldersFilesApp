using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FoldersFilesApp.Classes
{
    public class PicruresLibraryItem
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public BitmapImage ImagePath { get; set; }
    }
}

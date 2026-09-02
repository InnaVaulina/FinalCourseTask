using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Command;

namespace WpfBLazorHybridClient.Functions.Progect.AVM
{
    public interface IProgectEdit
    {
        string Title { get; set; }
        string Description { get; set; }
        bool State { get; set; }
        BitmapImage Picture { get; set; }
        FileInfo ImageFilePath { get; set; }
        WCommand SaveProgect { get; }
        void SaveImageContent(FileInfo imageFilePath);
    }
}

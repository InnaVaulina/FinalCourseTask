using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Command;

namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public interface IBlogEdit
    {
        string? Date { get; }
        string Title { get; set; }
        string Article { get; set; }
        bool State { get; set; }
        BitmapImage Picture { get; set; }
        FileInfo ImageFilePath { get; set; }
        WCommand SaveBlog { get; }
        void SaveImageContent(FileInfo imageFilePath);
    }
}

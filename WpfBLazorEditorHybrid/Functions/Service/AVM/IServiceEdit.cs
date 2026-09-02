using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Command;

namespace WpfBLazorHybridClient.Functions.Service.AVM
{
    public interface IServiceEdit
    {
        string Title { get; set; }
        string Description { get; set; }
        WCommand SaveService { get; }

    }
}

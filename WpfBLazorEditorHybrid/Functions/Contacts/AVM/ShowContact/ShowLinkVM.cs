using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.DM;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ShowContact
{
    public class ShowLinkVM: INotifyPropertyChanged
    {

        public ShowLinkVM(ContactLinkDM _linkDM) 
        {
            linkDM = _linkDM;
            iconWorldNetBitmap = new BitmapImage();
            LoadIllustration();
        }

        ContactLinkDM linkDM;
        public ContactLinkDM LinkDM { get { return linkDM; } }

        BitmapImage iconWorldNetBitmap;
        public BitmapImage IconWorldNetBitmap
        {
            get { return iconWorldNetBitmap; }
            set
            {
                iconWorldNetBitmap = value;
                OnPropertyChanged("IconWorldNetBitmap");
            }
        }

        public void LoadIllustration()
        {
            if (linkDM.FileInfo == null)
            {
                IconWorldNetBitmap = new BitmapImage();
                return;
            }
            using (var fileStream = new FileStream(linkDM.FileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = fileStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                IconWorldNetBitmap = bitmapImage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

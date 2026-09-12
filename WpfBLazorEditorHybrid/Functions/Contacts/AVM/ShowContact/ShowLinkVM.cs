using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Configuration;

using TTClassLibrary.Functions.Contacts;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ShowContact
{
    public class ShowLinkVM: INotifyPropertyChanged
    {

        public ShowLinkVM(ContactLinkDM _linkDM) 
        {
            linkDM = _linkDM;
            IconWorldNetBitmap = PictureLoader.LoadIllustration(linkDM.Link.IkonFileName);
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

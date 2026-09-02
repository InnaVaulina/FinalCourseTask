using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.DM;
using static System.Net.Mime.MediaTypeNames;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact
{
    public class ChangeAddressVM: LikeAddressVM
    {
        ContactExampleDM contactDM;
        int contactId;

        public ChangeAddressVM(ContactExampleDM _contactDM) : base()
        {
            contactDM = _contactDM;
            AddressDM = contactDM.AddressDM;
            contactId = contactDM.Content.ID;
            LoadIllustration();

            addAddressPanel = new WCommand(o =>
            {
                var address = new ContactAddress()
                {
                    ContactId = contactId,
                    Address = "",
                    MapFileName = ""
                };
                AddressDM = new ContactAddressDM()
                {
                    Address = address,
                    FileInfo = null
                };
                PictureMapBitmap = new BitmapImage();
            });

            dislikeText = new WCommand(async _ =>
            {
                if (AddressDM.Address.ID != 0)
                    AddressDM.DeletedAddress = AddressDM.Address;
                AddressDM.Address = null;
                Text = "";
                OnPropertyChanged("AddressText");
            });
        }

        public void LoadIllustration()
        {
            if (AddressDM.FileInfo == null)
            {
                PictureMapBitmap = new BitmapImage();
                return;
            }
            using (var fileStream = new FileStream(AddressDM.FileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = fileStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                PictureMapBitmap = bitmapImage;
            }
        }

    }
}

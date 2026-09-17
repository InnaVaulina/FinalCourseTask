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
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using TTClassLibrary.Functions.Contacts;
using System.Configuration;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact
{
    public class ChangeAddressVM: LikeAddressVM
    {
        int contactId;
        public ChangeAddressVM(ContactExampleDM _contactDM) : base(_contactDM.AddressDM)
        {
            if (addressDM.Address != null)
            {
                contactId = _contactDM.Content.ID;
                text = addressDM.Address.Address;
                pictureMapBitmap = PictureLoader.LoadIllustration(addressDM.Address.MapFileName);
            }
            else 
            {
                contactId = _contactDM.Content.ID;
            }

            addAddressPanel = new WCommand(o =>
            {
                AddressDM.Address = new ContactAddress()
                {
                    ContactId = contactId,
                    Address = "",
                    MapFileName = ""
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

    }
}

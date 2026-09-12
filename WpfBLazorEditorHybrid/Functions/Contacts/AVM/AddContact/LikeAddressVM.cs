using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Contacts;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public class LikeAddressVM : INotifyPropertyChanged
    {

        ContactAddressDM addressDM;
        public ContactAddressDM AddressDM
        {
            get { return addressDM; }
            set { addressDM = value; }
        } 
        
        public ContactAddress? Address 
        { 
            get 
            {
                if (addressDM == null) return null;
                return addressDM.Address; 
            } 
        }
        public LikeAddressVM(ContactAddressDM? _addressDM)
        {
            addressDM = _addressDM ?? new ContactAddressDM();
            if (_addressDM != null)
            {
                text = addressDM.Address.Address;
                pictureMapBitmap = PictureLoader.LoadIllustration(addressDM.Address.MapFileName);
            }
            else
            {
                text = "";
                pictureMapBitmap = new BitmapImage(); 
            }
    

            addAddressPanel = new WCommand(o =>
            {
                AddressDM.Address = new ContactAddress()
                {
                    Address = "",
                    MapFileName = ""
                };
                PictureMapBitmap = new BitmapImage();
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                AddressDM.Address.Address = text; 
                OnPropertyChanged("AddressText");
            });

            dislikeText = new WCommand(o =>
            {
                AddressDM.Address = null;
                Text = "";
                OnPropertyChanged("AddressText");
            });
        }

        protected BitmapImage pictureMapBitmap;
        public BitmapImage PictureMapBitmap
        {
            get { return pictureMapBitmap; }
            set { pictureMapBitmap = value; OnPropertyChanged("PictureMapBitmap"); }
        }

        FileInfo pictureMapFilePath;
        public FileInfo PictureMapFilePath
        {
            get { return pictureMapFilePath; }
            set { pictureMapFilePath = value; SaveImageContent(pictureMapFilePath); }
        }

        private void SaveImageContent(FileInfo imageFilePath)
        {
            AddressDM.FileInfo = new ImageFileModel()
            {
                FileName = imageFilePath.Name,
                Content = File.ReadAllBytes(imageFilePath.FullName),
                ContentType = "application/octet-stream"
            };
            AddressDM.Address.MapFileName = "";
        }

        protected string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        public string AddressText
        {
            get 
            { 
                if(AddressDM == null || AddressDM.Address == null) 
                    return "";
                return AddressDM.Address.Address; 
            }
        }

        protected WCommand addAddressPanel;
        public WCommand AddAddressPanel { get { return addAddressPanel; } }

        protected WCommand likeText;
        public WCommand LikeText { get { return likeText; } }

        protected WCommand dislikeText;
        public WCommand DislikeText { get { return dislikeText; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

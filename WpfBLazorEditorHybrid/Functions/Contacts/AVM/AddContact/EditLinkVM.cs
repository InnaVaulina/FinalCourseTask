using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Contacts;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public class EditLinkVM : INotifyPropertyChanged
    {
        public event DeleteLinkHandler Notify_DeleteLink;

        ContactLinkDM linkDM;
        public ContactLinkDM LinkDM { get { return linkDM; } }

        public EditLinkVM(ContactLinkDM _linkDM)
        {
            linkDM = _linkDM;
            iconWorldNetFilePath = null;
            if(linkDM.FileInfo == null)
                IconWorldNetBitmap = PictureLoader.LoadIllustration(linkDM.Link.IkonFileName);
            else 
            {
                using (var fileStream = new FileStream(linkDM.FileInfo.FullFileName, FileMode.Open, FileAccess.Read))
                {
                    IconWorldNetBitmap = new BitmapImage();
                    IconWorldNetBitmap.BeginInit();
                    IconWorldNetBitmap.CacheOption = BitmapCacheOption.OnLoad;
                    IconWorldNetBitmap.StreamSource = fileStream;
                    IconWorldNetBitmap.EndInit();
                    IconWorldNetBitmap.Freeze();
                }
            } 
            text = "";

            editText = new WCommand(o =>
            {
                Text = linkDM.Link.SocialLink;
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                linkDM.Link.SocialLink = text;
                OnPropertyChanged("LinkText");
                Text = "";

            });

            dislikeText = new WCommand(o =>
            {
                Notify_DeleteLink?.Invoke(this);
            });
        }

        FileInfo iconWorldNetFilePath;
        public FileInfo IconWorldNetFilePath
        {
            get { return iconWorldNetFilePath; }
            set { iconWorldNetFilePath = value; SaveImageContent(iconWorldNetFilePath); }
        }

        private void SaveImageContent(FileInfo imageFilePath)
        {
            LinkDM.FileInfo = new ImageFileModel()
            {
                FileName = imageFilePath.Name,
                Content = File.ReadAllBytes(imageFilePath.FullName),
                ContentType = "application/octet-stream"
            };
            LinkDM.Link.IkonFileName = "";
        }

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

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        public string LinkText
        {
            get { return linkDM.Link.SocialLink; }
        }

        WCommand editText;
        public WCommand EditText { get { return editText; } }

        WCommand likeText;
        public WCommand LikeText { get { return likeText; } }

        WCommand dislikeText;
        public WCommand DislikeText { get { return dislikeText; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.DM;

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
            iconWorldNetBitmap = new BitmapImage(new Uri(linkDM.FileInfo.FullName, UriKind.Absolute));
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

        public FileInfo IconWorldNetFilePath
        {
            get { return linkDM.FileInfo; }
            
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

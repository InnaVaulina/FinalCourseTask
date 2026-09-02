using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.DM;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public delegate void DeleteLinkHandler(EditLinkVM item);
    public class LikeLinkVM: INotifyPropertyChanged
    {
        protected ObservableCollection<EditLinkVM> links;
        public ObservableCollection<EditLinkVM> Links
        {
            get { return links; }
            set { links = value; OnPropertyChanged("Links"); }
        }

        protected void DeleteLink(EditLinkVM item) 
        { 
            Links.Remove(item);
        }

        protected ContactLinkDM linkDM;

        public LikeLinkVM()
        {
            links = new ObservableCollection<EditLinkVM>();

            linkDM = null;

            iconWorldNetBitmap = new BitmapImage();

            addLinkPanel = new WCommand(o =>
            {
                var link = new ContactSocialLink()
                {
                    SocialLink = "",
                    IkonFileName = ""
                };
                linkDM = new ContactLinkDM() 
                {
                    Link = link,
                    FileInfo = null
                };
            });

            likeText = new WCommand(o =>
            {
                if(text == null || text.Trim() == "") 
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }
                    
                if(IconWorldNetFilePath == null) 
                {
                    MessageBox.Show("Иконка не выбрана!");
                    return;
                }


                linkDM.Link.SocialLink = text;
                EditLinkVM editLinkVM = new EditLinkVM(linkDM);
                editLinkVM.Notify_DeleteLink += DeleteLink;
                Links.Add(editLinkVM);
                Text = "";
                IconWorldNetBitmap = null;
                IconWorldNetFilePath = null;
            });
        }

        public FileInfo? IconWorldNetFilePath
        {
            get { return linkDM.FileInfo; }
            set 
            {
                linkDM.FileInfo = value;
                linkDM.Link.IkonFileName = "";
            }
        }

        protected BitmapImage iconWorldNetBitmap;
        public BitmapImage IconWorldNetBitmap
        {
            get { return iconWorldNetBitmap; }
            set 
            { 
                iconWorldNetBitmap = value; 
                OnPropertyChanged("IconWorldNetBitmap"); 
            }
        }

        protected string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        protected WCommand addLinkPanel;
        public WCommand AddLinkPanel { get { return addLinkPanel; } }



        protected WCommand likeText;
        public WCommand LikeText { get { return likeText; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

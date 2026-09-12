using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact;
using WpfBLazorHybridClient.Functions.Contacts.AVM.ShowContact;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using TTClassLibrary.Functions.Contacts;
using WpfBLazorHybridClient.Functions.Service.AVM;
using WpfBLazorHybridClient.Main.AVM.Tab;
using System.Configuration;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM
{
    
    public class ContactItemVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new_page;
        public event DeleteContactHandler Notify_delete;
        public event UpdateContactHandler Notify_update;

        ListTabVM tab;
        UC_ContactItem ucContactItem;
        public UC_ContactItem UCContactItem { set { ucContactItem = value; } }

        ContactExampleDM contactDM;

        public ContactExampleDM ContactDM { get { return contactDM; } }

        public ContactContent Contact { get { return contactDM.Content; } }
        public ContactItemVM(ContactExampleDM _contactDM, ListTabVM _tab)
        {
            tab = _tab;
            contactDM = _contactDM;
            if(contactDM.AddressDM.Address != null)
                PictureMapBitmap = PictureLoader.LoadIllustration(contactDM.AddressDM.Address.MapFileName);
            else PictureMapBitmap = new BitmapImage();


            openEditingPage = new WCommand(o => {
                TabVM page = new TabVM()
                {
                    Header = "Изменить сведения"
                };

                var model = new ContactChangeVM(contactDM, page);
                model.Notify_update += NotyfyUpdate;
                model.Notify_close_page += tab.TabClose;

                page.Content = new UC_ContactItemChange(model);
                Notify_new_page?.Invoke(page);
            });

            deleteItem = new WCommand(async _ => {
                try
                {
                    var status = await contactDM.DeleteAsync();
                    if (status)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_delete?.Invoke(ucContactItem);
                    }
                }
                catch (ScopedExeption ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
            });
        }


        public string Title { get { return contactDM.Content.Name; } }

       
        BitmapImage pictureMapBitmap;
        public BitmapImage PictureMapBitmap
        {
            get { return pictureMapBitmap; }
            set { pictureMapBitmap = value; OnPropertyChanged("PictureMapBitmap"); }
        }

        public string AddressText 
        { 
            get 
            { 
                if(contactDM.AddressDM == null || contactDM.AddressDM.Address == null) return "";
                else 
                    return contactDM.AddressDM.Address.Address; 
            } 
        }

        public ObservableCollection<ContactPhone> Phones
        {
            get
            {
                return new ObservableCollection<ContactPhone>(contactDM.Content.Phones);
            }
        }

        public ObservableCollection<ContactMail> Emails
        {
            get
            {
                return new ObservableCollection<ContactMail>(contactDM.Content.Mails);
            }
        }

        public ObservableCollection<UC_ShowSocialLink> Links
        {
            get
            {
                var list = new ObservableCollection<UC_ShowSocialLink>();
                foreach (var item in contactDM.SocialLinksListDM.ContentList)
                {
                    list.Add(new UC_ShowSocialLink(new ShowLinkVM(item)));
                }
                return list;
            }
        }


        WCommand openEditingPage;
        public WCommand OpenEditingPage { get { return openEditingPage; } }

        WCommand deleteItem;
        public WCommand DeleteItem { get { return deleteItem; } }

       

        public void NotyfyUpdate()
        {
            if (contactDM.AddressDM != null)
                PictureMapBitmap = PictureLoader.LoadIllustration(contactDM.AddressDM.Address.MapFileName);
            else PictureMapBitmap = new BitmapImage();
            OnPropertyChanged("PictureMapBitmap"); 
            OnPropertyChanged("Title");
            OnPropertyChanged("AddressText");
            OnPropertyChanged("Phones");
            OnPropertyChanged("Emails");
            OnPropertyChanged("Links");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

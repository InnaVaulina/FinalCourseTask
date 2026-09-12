using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using TTClassLibrary.Functions.Contacts;
using WpfBLazorHybridClient.Functions.Service.AVM;
using WpfBLazorHybridClient.Main.AVM.Tab;


namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public class ContactAddVM : INotifyPropertyChanged
    {
        protected TabVM page;

        public event AddContactHandler Notify_add;
        public event TabCloseHandler Notify_close_page;

        AddNewContactExampleDM contactDM;
        public ContactContent Contact
        {
            get { return contactDM.Content; }
        }

        public ContactAddVM(AddNewContactExampleDM _contactDM, TabVM page)
        {
            contactDM = _contactDM;
            this.page = page;

            uc_address = new UC_AddAddress(new LikeAddressVM(null));
            uc_phone = new UC_Phone(new LikePhoneVM());
            uc_mail = new UC_mailUs(new LikeMailVM());
            uc_link = new UC_SocialLink(new LikeLinkVM());

            saveContact = new WCommand(async _ =>
            {
                if (Title == "")
                {
                    MessageBox.Show("Название не заполнено!");
                    return;
                }

                if(UC_address.Model.AddressDM == null && 
                    UC_phone.Model.Phones.Count == 0 &&
                    UC_mail.Model.Mails.Count == 0 &&
                    UC_link.Model.Links.Count == 0)
                {
                    MessageBox.Show("Контактные данные не заполнены! ");
                    return;
                }

                contactDM.AddressDM = UC_address.Model.AddressDM;
                contactDM.Content.Address = contactDM.AddressDM.Address;
                contactDM.Content.Mails = UC_mail.Model.Mails.Select(m => m.Mail).ToList();
                contactDM.Content.Phones = UC_phone.Model.Phones.Select(p => p.Phone).ToList();
                contactDM.SocialLinksListDM = UC_link.Model.Links.Select(l => l.LinkDM).ToList();
                contactDM.Content.Links = contactDM.SocialLinksListDM.Select(l => l.Link).ToList();

                try
                {
                    var newcontact = await contactDM.CreateContactContentAsync();
                    if (newcontact != null)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_add?.Invoke(newcontact);
                        Notify_close_page?.Invoke(page);
                    }
                }
                catch (ScopedExeption ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
            });
        }


        public string Title
        {
            get { return contactDM.Content.Name; }
            set { contactDM.Content.Name = value; OnPropertyChanged("Title"); }
        }



        UC_SocialLink uc_link;
        public UC_SocialLink UC_link { get { return uc_link; } }

        UC_mailUs uc_mail;
        public UC_mailUs UC_mail { get { return uc_mail; } }


        UC_Phone uc_phone;
        public UC_Phone UC_phone { get { return uc_phone; } }

        UC_AddAddress uc_address;
        public UC_AddAddress UC_address { get { return uc_address; } }

        protected WCommand saveContact;
        public WCommand SaveContact { get { return saveContact; } }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

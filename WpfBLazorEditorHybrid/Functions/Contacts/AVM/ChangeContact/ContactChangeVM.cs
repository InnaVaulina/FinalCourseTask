using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Contacts;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact
{
    public class ContactChangeVM : INotifyPropertyChanged
    {
        public event UpdateContactHandler Notify_update;

        protected TabVM page;

        public event TabCloseHandler Notify_close_page;

        ContactExampleDM contactDM;
        public ContactExampleDM ContactDM
        {
            get { return contactDM; }
        }

        public ContactChangeVM(ContactExampleDM _contactDM, TabVM _page)
        {
            page = _page;
            contactDM = _contactDM;
            page.Notify_close += Page_Notify_close;
            uc_address = new UC_AddAddress(new ChangeAddressVM(ContactDM));
            uc_phone = new UC_Phone(new ChangePhoneVM(ContactDM));
            uc_mail = new UC_mailUs(new ChangeMailVM(ContactDM));
            uc_link = new UC_SocialLink(new ChangeLinkVM(ContactDM));

            finishChanging = new WCommand(async _ =>
            {
                if (Title == "")
                {
                    MessageBox.Show("Название не заполнено!");
                    return;
                }

                if (UC_address.Model.AddressDM == null &&
                    UC_phone.Model.Phones.Count == 0 &&
                    UC_mail.Model.Mails.Count == 0 &&
                    UC_link.Model.Links.Count == 0)
                {
                    MessageBox.Show("Контактные данные не заполнены! ");
                    return;
                }

                

                try
                {
                    var response = await contactDM.ChangeContactContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<ContactContent>();
                    var newcontact = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newcontact != null)
                    {
                        await contactDM.UpdateDM(newcontact);
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_update?.Invoke();
                        Notify_close_page?.Invoke(page);
                    }

                }
                catch (ScopedExeption ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }

            });

        }

        private void Page_Notify_close(TabVM tab)
        {
            Notify_update?.Invoke();
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

        WCommand finishChanging;
        public WCommand FinishChanging { get { return finishChanging; } }
        
            

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

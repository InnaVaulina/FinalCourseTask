using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.DM;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact
{
    public class ChangePhoneVM: LikePhoneVM
    {
        ContactExampleDM contactDM;
        int contactId;

        public ContactPhone Phone { get { return phone; } }

        public ChangePhoneVM(ContactExampleDM _contactDM)
        {
            contactDM = _contactDM;
            contactId = contactDM.Content.ID;
            phones = new ObservableCollection<EditPhoneVM>();
            foreach (var item in contactDM.PhonesListDM.ContentList)
            {
                EditPhoneVM editPhoneVM = new EditPhoneVM(item);
                editPhoneVM.Notify_DeletePhone += DeletePhone;
                phones.Add(editPhoneVM);
            }

            phone = new ContactPhone()
            {
                ContactId = contactId,
                Phone = ""
            };

            addPhonePanel = new WCommand(o =>
            {
                phone = new ContactPhone()
                {
                    ContactId = contactId,
                    Phone = ""
                };
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                phone.Phone = text;
                contactDM.PhonesListDM.ContentList.Add(phone);
                EditPhoneVM editPhoneVM = new EditPhoneVM(phone);
                editPhoneVM.Notify_DeletePhone += DeletePhone;
                phones.Add(editPhoneVM);
                Text = "";
            });

        }

        new void DeletePhone(EditPhoneVM item)
        {
            Phones.Remove(item);
            contactDM.PhonesListDM.ContentList.Remove(item.Phone);
            if (item.Phone.ID != 0) 
            {
                contactDM.PhonesListDM.DeletedContentList.Add(item.Phone);
            }
            
        }
    }
}

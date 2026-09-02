using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    public class ChangeMailVM: LikeMailVM
    {
        ContactExampleDM contactDM;
        int contactId;

        public ContactMail Mail { get { return mail; } }
        public ChangeMailVM(ContactExampleDM _contactDM)
        {
            contactDM = _contactDM;
            contactId = contactDM.Content.ID;
            mails = new ObservableCollection<EditMailVM>();
            foreach (var item in contactDM.MailsListDM.ContentList)
            {
                EditMailVM editMailVM = new EditMailVM(item);
                editMailVM.Notify_DeleteMail += DeleteMail;
                Mails.Add(editMailVM);
            }

            mail = new ContactMail()
            {
                ContactId = contactId,
                Mail = ""
            };

            addMailPanel = new WCommand(o =>
            {
                mail = new ContactMail()
                {
                    ContactId = contactId,
                    Mail = ""
                };
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                mail.Mail = text;
                contactDM.MailsListDM.ContentList.Add(mail);
                EditMailVM editMailVM = new EditMailVM(mail);
                editMailVM.Notify_DeleteMail += DeleteMail;
                mails.Add(new EditMailVM(mail));
                Text = "";
            });
        }

        new void DeleteMail(EditMailVM item)
        {
            Mails.Remove(item);
            contactDM.MailsListDM.ContentList.Remove(item.Mail);
            if (item.Mail.ID != 0)
            {
                contactDM.MailsListDM.DeletedContentList.Add(item.Mail);
            }
        }

    }
}

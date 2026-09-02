using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.DM;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact
{
    public class ChangeLinkVM: LikeLinkVM
    {
        ContactExampleDM contactDM;
        int contactId;

        public ContactLinkDM LinkDM { get { return linkDM; } }
        public ChangeLinkVM(ContactExampleDM _contactDM)
        {
            contactDM = _contactDM;
            contactId = contactDM.Content.ID;
            Links = new ObservableCollection<EditLinkVM>();
            foreach (var item in contactDM.SocialLinksListDM.ContentList)
            {
                EditLinkVM editLinkVM = new EditLinkVM(item);
                editLinkVM.Notify_DeleteLink += DeleteLink;
                Links.Add(editLinkVM);
            }

            iconWorldNetBitmap = new BitmapImage();

            addLinkPanel = new WCommand(o =>
            {
                var link = new ContactSocialLink()
                {
                    ContactId = contactId,
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
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                if (IconWorldNetFilePath == null)
                {
                    MessageBox.Show("Иконка не выбрана!");
                    return;
                }


                linkDM.Link.SocialLink = text;
                contactDM.SocialLinksListDM.ContentList.Add(linkDM);
                EditLinkVM editLinkVM = new EditLinkVM(linkDM);
                editLinkVM.Notify_DeleteLink += DeleteLink;
                Links.Add(editLinkVM);
                Text = "";
                IconWorldNetBitmap = null;
                IconWorldNetFilePath = null;
            });

        }

        new void DeleteLink(EditLinkVM item)
        {
            Links.Remove(item);
            contactDM.SocialLinksListDM.ContentList.Remove(item.LinkDM);
            if (item.LinkDM.Link.ID != 0)
            {
                contactDM.SocialLinksListDM.DeletedContentList.Add(item.LinkDM);
            }
        }

    }
}

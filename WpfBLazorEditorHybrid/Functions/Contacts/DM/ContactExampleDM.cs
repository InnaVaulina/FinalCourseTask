using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.AVM;

namespace WpfBLazorHybridClient.Functions.Contacts.DM
{
   
    public class ContactExampleDM
    {
        ContactClient requestMaker;
        HttpResponseMessageDeserialize<ContactContent> jsonSerializer;

        ContactContent content;
        public ContactContent Content
        {
            get { return content; }
        }

        ContactContent deletedcontent;
        public ContactContent DeletedContent
        {
            get { return deletedcontent; }
        }

        ContactAddressDM addressDM;
        public ContactAddressDM AddressDM
        {
            get { return addressDM; }
            set { addressDM = value; }
        }

        ContentListDM<ContactLinkDM> socialLinksListDM;
        public ContentListDM<ContactLinkDM> SocialLinksListDM
        {
            get { return socialLinksListDM; }
            set { socialLinksListDM = value; }
        }

        ContentListDM<ContactPhone> phonesListDM;
        public ContentListDM<ContactPhone> PhonesListDM
        {
            get { return phonesListDM; }
            set { phonesListDM = value; }
        }

        ContentListDM<ContactMail> mailsListDM;
        public ContentListDM<ContactMail> MailsListDM
        {
            get { return mailsListDM; }
            set { mailsListDM = value; }
        }

        public ContactExampleDM(ContactClient _requestMaker, ContactContent _content)
        {
            requestMaker = _requestMaker;
            content = _content;
            jsonSerializer = new HttpResponseMessageDeserialize<ContactContent>(requestMaker);
            addressDM = new ContactAddressDM() 
            {
                    Address = content.Address,
                    DeletedAddress = null
            };
            phonesListDM = new ContentListDM<ContactPhone>() 
            {
                    ContentList = content.Phones,
                    DeletedContentList = new List<ContactPhone>()
            };
            mailsListDM = new ContentListDM<ContactMail>() 
            {
                    ContentList = content.Mails,
                    DeletedContentList = new List<ContactMail>()
            };
            socialLinksListDM = new ContentListDM<ContactLinkDM>() 
            {
                    ContentList = content.Links.Select(link => new ContactLinkDM() { Link = link }).ToList(),
                    DeletedContentList = new List<ContactLinkDM>()
            };

        }

        public static async Task<ContactExampleDM> CreateAsync(ContactClient _requestMaker, ContactContent _content)
        {
            var dm = new ContactExampleDM(_requestMaker, _content);
            await dm.InitializeAsync();
            return dm;
        }

        private async Task InitializeAsync()
        {
            if(addressDM.Address != null && addressDM.Address.MapFileName != "")
                addressDM.FileInfo = await requestMaker.GetImage(addressDM.Address.MapFileName);
            foreach (var linkDM in socialLinksListDM.ContentList)
            {
                if (linkDM.Link.IkonFileName != "")
                    linkDM.FileInfo = await requestMaker.GetImage(linkDM.Link.IkonFileName);
            }
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await requestMaker.DeleteContact(Content);
            return response.IsSuccessStatusCode;
        }

        public async Task<ContactContent> ChangeContactContentAsync()
        {
            Content.Address = AddressDM.Address;
            if (PhonesListDM.ContentList.Count > 0)
            {
                Content.Phones = new List<ContactPhone>();
                foreach (var phone in PhonesListDM.ContentList)
                    Content.Phones.Add(phone);
            }
            if (MailsListDM.ContentList.Count > 0)
            {
                Content.Mails = new List<ContactMail>();
                foreach (var mail in MailsListDM.ContentList)
                    Content.Mails.Add(mail);
            }
            if (SocialLinksListDM.ContentList.Count > 0)
            {
                Content.Links = new List<ContactSocialLink>();
                foreach (var linkDM in SocialLinksListDM.ContentList)
                    Content.Links.Add(linkDM.Link);
            }

            deletedcontent = new ContactContent() 
            {
                ID = content.ID,
                Name = content.Name,
                Address = AddressDM.DeletedAddress,
                Phones = PhonesListDM.DeletedContentList,
                Mails = MailsListDM.DeletedContentList,
                Links = SocialLinksListDM.DeletedContentList.Select(linkDM => linkDM.Link).ToList()
            };

            var response = await requestMaker.UpdateContact(this);
            content = await jsonSerializer.DeserealizeResultToContentAsync(response);
            await InitializeAsync();
            return content;
        }
    }
}

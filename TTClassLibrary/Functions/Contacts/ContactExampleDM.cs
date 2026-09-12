using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Contacts
{
    public class ContactExampleDM
    {
        IContactRequestSender requestMaker;

        MultipartFormDataContent formData;

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

        public ContactExampleDM(IContactRequestSender _requestMaker, ContactContent _content)
        {
            requestMaker = _requestMaker;
            content = _content;
            
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

        public static async Task<ContactExampleDM> CreateAsync(IContactRequestSender _requestMaker, ContactContent _content)
        {
            var dm = new ContactExampleDM(_requestMaker, _content);
            await dm.InitializeAsync();
            return dm;
        }

        private async Task InitializeAsync()
        {
            if (addressDM.Address != null && addressDM.Address.MapFileName != "")
                await requestMaker.GetImage(addressDM.Address.MapFileName);
            foreach (var linkDM in socialLinksListDM.ContentList)
            {
                if (linkDM.Link.IkonFileName != "")
                    await requestMaker.GetImage(linkDM.Link.IkonFileName);
            }
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await requestMaker.DeleteContact(Content.ID);
            return response.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage> ChangeContactContentAsync()
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

            using (var formData = new MultipartFormDataContent())
            {
                var json1 = JsonSerializer.Serialize(Content);
                formData.Add(new StringContent(json1, Encoding.UTF8, "application/json"), "contactContent");

                var json2 = JsonSerializer.Serialize(deletedcontent);
                formData.Add(new StringContent(json2, Encoding.UTF8, "application/json"), "deletedContactContent");

                if (AddressDM.FileInfo != null)
                {
                    var imageContent = new ByteArrayContent(AddressDM.FileInfo.Content, 0, AddressDM.FileInfo.Content.Length);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    formData.Add(imageContent, "addressPicture", AddressDM.FileInfo.FileName);
                }


                foreach (var linkDM in SocialLinksListDM.ContentList)
                {
                    if (linkDM.FileInfo != null) 
                    {
                        var imageContent = new ByteArrayContent(linkDM.FileInfo.Content, 0, linkDM.FileInfo.Content.Length);
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        formData.Add(imageContent, "socialIcons", linkDM.FileInfo.FileName);
                    }
                    else 
                    {
                        var imageContent = new ByteArrayContent(Array.Empty<byte>());
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        formData.Add(imageContent, "socialIcons", linkDM.Link.IkonFileName);
                    }
                }

                var response = await requestMaker.UpdateContact(formData, Content.ID);
                return response;
            }
 
        }

        public async Task UpdateDM(ContactContent newcontent)
        {
            content = newcontent;

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

            await InitializeAsync();
        }
    }
}

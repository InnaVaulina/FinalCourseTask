using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Contacts
{
    public class AddNewContactExampleDM
    {
        IContactRequestSender requestMaker;

        ContactContent content;
        public ContactContent Content
        {
            get { return content; }
        }

        ContactAddressDM addressDM;
        public ContactAddressDM AddressDM
        {
            get { return addressDM; }
            set { addressDM = value; }
        }

        List<ContactLinkDM> socialLinksListDM;
        public List<ContactLinkDM> SocialLinksListDM
        {
            get { return socialLinksListDM; }
            set { socialLinksListDM = value; }
        }


        public AddNewContactExampleDM(IContactRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            
            content = new ContactContent()
            {
                ID = 0,
                Name = "",
                Address = null,
                Phones = new List<ContactPhone>(),
                Mails = new List<ContactMail>(),
                Links = new List<ContactSocialLink>()
            };
            addressDM = new ContactAddressDM();
            socialLinksListDM = new List<ContactLinkDM>();
           
        }


        public async Task<ContactContent?> CreateContactContentAsync()
        {
            using (var formData = new MultipartFormDataContent())
            {
                var json = JsonSerializer.Serialize(Content);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "contactContent");

                if (AddressDM.FileInfo != null)
                {
                    var imageContent = new ByteArrayContent(AddressDM.FileInfo.Content, 0, AddressDM.FileInfo.Content.Length);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    formData.Add(imageContent, "addressPicture", AddressDM.FileInfo.FileName);
                }


                foreach (var linkDM in SocialLinksListDM)
                {
                    var imageContent = new ByteArrayContent(linkDM.FileInfo.Content, 0, linkDM.FileInfo.Content.Length);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    formData.Add(imageContent, "socialIcons", linkDM.FileInfo.FileName);
                }

                var response = await requestMaker.SaveContact(formData);
                var jsonSerializer = new HttpResponseMessageDeserialize<ContactContent>();
                var newcontact = await jsonSerializer.DeserealizeResultToContentAsync(response);
                return newcontact;
            }
        }
    }
}

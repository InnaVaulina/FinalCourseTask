using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Functions.Contacts.DM
{
    public class AddNewContactExampleDM
    {
        ContactClient requestMaker;
        HttpResponseMessageDeserialize<ContactContent> jsonSerializer;

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


        public AddNewContactExampleDM(ContactClient _requestMaker)
        {
            requestMaker = _requestMaker;
            jsonSerializer = new HttpResponseMessageDeserialize<ContactContent>(requestMaker);
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
            var response = await requestMaker.SaveContact(this);
            var newcontact = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return newcontact;
        }
    }
}

using System.Collections.ObjectModel;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Contacts;

namespace TTB.VModel.ContactVM
{
    public interface INotAuthContactItemVM 
    {
        int Id { get; }
        string Title { get; }
        ContactAddress Address { get; }
        List <ContactPhone> Phones { get; }
        List<ContactMail> Emails { get; }
        List<ContactSocialLink> Links { get; }

    }
    public class ContactItemVM : INotAuthContactItemVM
    {
        ContactExampleDM contactExampleDM;

        public ContactItemVM(ContactExampleDM _contactExampleDM)
        {
            contactExampleDM = _contactExampleDM;
            
        }

        public int Id { get { return contactExampleDM.Content.ID; } }
        public string Title { get { return contactExampleDM.Content.Name; } }


       
        public ContactAddress Address
        {
            get
            {
                if (contactExampleDM.AddressDM == null) return null;
                else
                    return contactExampleDM.AddressDM.Address;
            }
        }



        public List<ContactPhone> Phones { get { return contactExampleDM.Content.Phones;} }

        public List<ContactMail> Emails { get { return contactExampleDM.Content.Mails; } }

        public List<ContactSocialLink> Links { get { return contactExampleDM.Content.Links; } }
    }
}

using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Contacts
{
    public class ContactAddressDM
    {
        public ImageFileModel? FileInfo { get; set; }
        public ContactAddress? Address {get; set; }

        public ContactAddress? DeletedAddress { get; set; }
    }

    

}

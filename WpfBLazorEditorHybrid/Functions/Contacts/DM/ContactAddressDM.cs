using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Functions.Contacts.DM
{
    public class ContactAddressDM
    {
        public FileInfo? FileInfo { get; set; }
        public ContactAddress? Address {get; set; }

        public ContactAddress? DeletedAddress { get; set; }
    }

    

}

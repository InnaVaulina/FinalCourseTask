using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.DataModel
{
    public class ContactContent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ContactAddress? Address { get; set; }
        public List<ContactPhone> Phones { get; set; }
        public List<ContactMail> Mails { get; set; }
        public List<ContactSocialLink> Links { get; set; }
    }


    public class ContactAddress
    {
        public int ID { get; set; }
        public int ContactId { get; set; }
        public string Address { get; set; }
        public string? MapFileName { get; set; }
    }

    public class ContactPhone
    {
        public int ID { get; set; }
        public int ContactId { get; set; }
        public string Phone { get; set; }
    }

    public class ContactMail
    {
        public int ID { get; set; }
        public int ContactId { get; set; }
        public string Mail { get; set; }
    }

    public class ContactSocialLink
    {
        public int ID { get; set; }
        public int ContactId { get; set; }
        public string SocialLink { get; set; }
        public string IkonFileName { get; set; }
    }

   
}

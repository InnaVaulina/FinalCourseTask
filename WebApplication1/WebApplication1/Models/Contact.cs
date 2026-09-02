using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{

    public class Contact
    {
        [Display(Name = "Номер подразделения")]
        public int ID { get; set; }

        [Display(Name = "Название подразделения")]
        public string Name { get; set; }
        public ContactAddress? Address { get; set; }
        public List<ContactPhone> Phones { get; set; }
        public List<ContactMail> Mails { get; set; }
        public List<ContactSocialLink> Links { get; set; }
    }

    public class ContactAddress
    {
        public int ID { get; set; }

        [Display(Name = "Номер подразделения")]
        public int ContactId { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Карта")]
        public string MapFileName { get; set; }


        //[ForeignKey("ContactId")]
        //public Contact Contact { get; set; }
    }

    public class ContactPhone 
    {
        public int ID { get; set; }

        [Display(Name = "Номер подразделения")]
        public int ContactId { get; set; }
        public string Phone { get; set; }


        //[ForeignKey("ContactId")]
        //public Contact Contact { get; set; }
    }

    public class ContactMail 
    {
        public int ID { get; set; }

        [Display(Name = "Номер подразделения")]
        public int ContactId { get; set; }
        public string Mail { get; set; }


        //[ForeignKey("ContactId")]
        //public Contact Contact { get; set; }
    }

    public class ContactSocialLink 
    {
        public int ID { get; set; }

        [Display(Name = "Номер подразделения")]
        public int ContactId { get; set; }
        public string SocialLink { get; set; }

        [Display(Name = "Значок")]
        public string IkonFileName { get; set; }



        //[ForeignKey("ContactId")]
        //public Contact Contact { get; set; }
    }
}

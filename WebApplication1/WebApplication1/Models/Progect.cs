using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Progect
    {
        [Display(Name = "Номер")]
        public int ID { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Иллюстрация")]
        public string IllustrationId { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }
    }
}

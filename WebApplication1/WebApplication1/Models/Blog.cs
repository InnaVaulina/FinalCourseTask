using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Blog
    {
        [Display(Name = "Номер")]
        public int ID { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTime PostDate { get; set; }

        [Display(Name = "Автор")]
        public string WriterId { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Статья")]
        public string Article { get; set; }

        [Display(Name = "Иллюстрация")]
        public string IllustrationId { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }
    }
}

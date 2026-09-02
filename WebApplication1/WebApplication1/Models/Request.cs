using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Request
    {
        [Display(Name = "Номер")]
        public int ID { get; set; }

        [Display(Name = "Дата поступления")]
        public DateTime RequestIn { get; set; }

        [Display(Name = "Имя заказчика")]
        public string FullName { get; set; }

        [Display(Name = "Контакты")]
        public string Contact {  get; set; }

        [Display(Name = "Текст запроса")]
        public string RequestText {  get; set; }

        [Display(Name = "Обработка")]
        public string? PerformingInfo {  get; set; }

        [Display(Name = "Статус")]
        public string Status {  get; set; }
    }
}

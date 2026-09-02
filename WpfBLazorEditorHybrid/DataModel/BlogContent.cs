using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.DataModel
{
    public class BlogContent
    {
        public int ID { get; set; }
        public string PostDate { get; set; }
        public string WriterId { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public string IllustrationId { get; set; }
        public string Status { get; set; }
    }
}

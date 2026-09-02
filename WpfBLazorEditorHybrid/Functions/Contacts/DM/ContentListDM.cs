using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.Functions.Contacts.DM
{
    public class ContentListDM<T>
    {
        public List<T> ContentList { get; set; }
        public List<T> DeletedContentList { get; set; }
    }
}

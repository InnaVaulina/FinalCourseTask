using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.Functions.Contacts
{
    public class ContentListDM<T>
    {
        public List<T> ContentList { get; set; }
        public List<T> DeletedContentList { get; set; }
    }
}

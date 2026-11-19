using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.RequestSender;
using static System.Net.WebRequestMethods;

namespace TTClassLibrary.Support.Filters
{
    public class BlogFilter : IFilter
    {
        public int Page { get { return parameters.Page; } set { parameters.Page = value; } }

        int totalPage;
        public int TotalPages { get { return totalPage; } set { totalPage = value; } }

        DateTime? beginDate;
        public DateTime? BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; parameters.BeginDate = value?.ToString("yyyy-MM-ddTHH:mm:ss"); }
        }

        DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; parameters.EndDate = value?.ToString("yyyy-MM-ddTHH:mm:ss"); }
        }

        public JsonContent? FilterContent 
        { 
            get 
            {
                if (EndDate.Value.Date == DateTime.Today.Date)
                {
                    EndDate = DateTime.Now;
                }
                else EndDate = EndDate.Value.Date.AddDays(1).AddSeconds(-1);
                return JsonContent.Create(parameters); 
            } 
        }

        BlogFilterParameters parameters;
        public BlogFilterParameters Parameters
        {
            get { return parameters; }
        }


        public BlogFilter()
        {
            parameters = new BlogFilterParameters()
            {
                Search = "ShowAll",
                BeginDate = "",
                EndDate = "",
                Page = 1
            };
            BeginDate = DateTime.MinValue;
            EndDate = DateTime.Now;
            totalPage = 1;
        }

        public BlogFilter(BlogFilterParameters _parameters)
        {
            parameters = _parameters;
        }


    }
}

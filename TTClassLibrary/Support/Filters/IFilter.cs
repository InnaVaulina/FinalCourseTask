using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.Support.Filters
{
    public interface IFilter
    {

        int Page { get; set; }
        int TotalPages { get; set; }
        JsonContent? FilterContent { get; }
    }
}

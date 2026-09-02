using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Controllers.RequestModel
{
    public class RequestReceived
    {      
        public string FullName { get; set; }
      
        public string Contact { get; set; }       
        public string RequestText { get; set; }
    }


}

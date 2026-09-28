using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using System.Text.Json;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Controllers.RequestModel;
using WebApplication1.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "admin, work")]
    public class RequestController: ControllerBase
    {
        private readonly IRequestQuery _collection;
        private readonly ServiceContext _context;


        public RequestController(ServiceContext context,
                                 IRequestQuery collection)
        {
            _context = context;
            _collection = collection;
        }

       

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SaveRequest([FromForm] string workContent)
        {
            Request request = new Request();
            try
            {
                request = JsonSerializer.Deserialize<Request>(workContent);
                request.Status = "received";
                await _collection.AddRequest(_context, request);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

       

        [HttpPut]
        public async Task<ActionResult> UpdateRequest([FromForm] string workContent, int id)
        {
            Request request = new Request();
            try
            {
                request = JsonSerializer.Deserialize<Request>(workContent);
                await _collection.SaveRequest(_context, request);
                request = await _collection.SelectExample(_context, id);
                return Ok(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }       
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests(string Search, string BeginDate, string EndDate, int Page)
        {
            List<Request> requestList;
            try 
            {
                DateTime beginDate = DateTime.ParseExact(BeginDate, "yyyy-MM-ddTHH:mm:ss", null);
                DateTime endDate = DateTime.ParseExact(EndDate, "yyyy-MM-ddTHH:mm:ss", null);
                switch (Search)
                {
                    case "ShowAll":
                        requestList = await _collection.SelectAllRequests(_context, beginDate, endDate);
                        break;
                    case "ShowReceived":
                        requestList = await _collection.SelectReceived(_context, beginDate, endDate);
                        break;
                    case "ShowTaken":
                        requestList = await _collection.SelectTakenOnWork(_context, beginDate, endDate);
                        break;
                    case "ShowRejected":
                        requestList = await _collection.SelectRejected(_context, beginDate, endDate);
                        break;
                    case "ShowFinished":
                        requestList = await _collection.SelectFinished(_context, beginDate, endDate);
                        break;
                    case "ShowCancelled":
                        requestList = await _collection.SelectCancelled(_context, beginDate, endDate);
                        break;
                    default:
                        throw new Exception("Неверный параметр поиска");
                }

                if (requestList.Count > 0)
                {
                    requestList.Reverse();
                    int pageCount = 6;
                    int totalPages = requestList.Count / 6 + (requestList.Count % 6 > 0 ? 1 : 0);
                    if (Page < 1 || Page > totalPages)
                    {
                        Page = 1;
                    }
                    if (Page == totalPages) pageCount = requestList.Count % 6 > 0 ? requestList.Count % 6 : 6;

                    GetAllWorkResponseParamertes paramertes = new GetAllWorkResponseParamertes()
                    {
                        Requests = requestList.GetRange(Page * 6 - 6, pageCount),
                        CurrentPage = Page,
                        TotalPages = totalPages
                    };
                    return Ok(paramertes);
                }
                else
                {
                    GetAllWorkResponseParamertes paramertes = new GetAllWorkResponseParamertes()
                    {
                        Requests = requestList,
                        CurrentPage = 1,
                        TotalPages = 1
                    };
                    return Ok(paramertes);
                }

            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }           
        }


    }

    public class GetAllWorkResponseParamertes
    {
        public List<Request> Requests { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

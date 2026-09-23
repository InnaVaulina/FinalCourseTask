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

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "admin, work")]
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
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetAll(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try 
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectAllRequests(_context,range);
                return Ok(requestList);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }           
        }


        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReceived(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectReceived(_context, range);
                return Ok(requestList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetTakenOnWork(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectTakenOnWork(_context, range);
                return Ok(requestList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRejected(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectRejected(_context, range);
                return Ok(requestList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetFinished(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectFinished(_context, range);
                return Ok(requestList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<IEnumerable<Request>>> GetCancelled(DateTime start, DateTime end)
        {
            List<Request> requestList;
            try
            {
                var range = new RequestRange() { Start = start, End = end };
                requestList = await _collection.SelectCancelled(_context, range);
                return Ok(requestList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }


       

    }
}

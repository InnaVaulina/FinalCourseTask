using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context.DBQuery;
using WebApplication1.Context;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using System.Diagnostics.Eventing.Reader;
using System.Data;
using WebApplication1.Controllers.RequestModel;

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
        public async Task<ActionResult> AddRequest(RequestReceived model)
        {
            Request request = new Request()
            {
                RequestIn = DateTime.Now,
                FullName = model.FullName,
                Contact = model.Contact,
                RequestText = model.RequestText,
                Status = "received"
            };

            try
            {
                await _collection.AddRequest(_context, request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(this.AddRequest), model);
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


        [HttpGet]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult<Request>> UpdateRequest(int id)
        {           
            try
            {
                var curRequest = await _collection.UpdateRequest(_context, id);
                return Ok(curRequest);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin, work")]
        public async Task<ActionResult> SaveRequest(int id, Request request)
        {
            try
            {
                if (id == request.ID)
                    await _collection.SaveRequest(_context, request);
                else throw new Exception($"Невозможно изменить запись с id ={id} на запись с id ={request.ID}."); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return Ok();
        }

    }
}

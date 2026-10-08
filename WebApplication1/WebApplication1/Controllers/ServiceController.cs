using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceContext _context;

        public ServiceController(ServiceContext context)
        {
            _context = context;
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteService(int id)
        {
            try
            {
                BusinesService service = await _context.SelectService(id);
                if (service != null)
                {
                    await _context.DeleteService(id);
                }
                else
                {
                    throw new Exception("объект не найден");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> SaveServiceRId([FromForm] string serviceContent)
        {
            BusinesService service;
            try
            {
                service = await SaveService(serviceContent);
                if (service == null) return BadRequest(ModelState);
                return Ok(service.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPost]
        public async Task<ActionResult> SaveServiceRContent([FromForm] string serviceContent)
        {
            BusinesService? service;
            try
            {
                service = await SaveService(serviceContent);
                if (service == null) return BadRequest(ModelState);
                return Ok(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

   
        private async Task<BusinesService?> SaveService([FromForm] string serviceContent)
        {
            BusinesService service = new BusinesService();
            try
            {
                service = JsonSerializer.Deserialize<BusinesService>(serviceContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле serviceContent: " + ex.Message);
                return null;
            }

            try
            {
                await _context.AddService(service);
                service = await _context.SelectLastService();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return null;
            }

            return service;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateServiceROk(int id, [FromForm] string serviceContent)
        {
            BusinesService service;
            try
            {
                service = await UpdateService(id, serviceContent);
                if (service == null) return BadRequest(ModelState);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateServiceRContent(int id, [FromForm] string serviceContent)
        {
            BusinesService? service;
            try
            {
                service = await UpdateService(id, serviceContent);
                if (service == null) return BadRequest(ModelState);
                return Ok(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        private async Task<BusinesService> UpdateService(int id, [FromForm] string serviceContent)
        {
            BusinesService service = new BusinesService();
            try
            {
                service = JsonSerializer.Deserialize<BusinesService>(serviceContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле serviceContent: " + ex.Message);
                return null;
            }

            try
            {
                await _context.UpdateService(service);
                service = await _context.SelectService(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return null;
            }

            return service;
        }



        [HttpGet]
        public async Task<ActionResult<BusinesService>> GetService(int id)
        {
            BusinesService service;
            try
            {
                service = await _context.SelectService(id);
                return Ok(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinesService>>> GetAllServices()
        {
            List<BusinesService> progects;
            try
            {
                progects = await _context.SelectAllServices();
                return Ok(progects);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}

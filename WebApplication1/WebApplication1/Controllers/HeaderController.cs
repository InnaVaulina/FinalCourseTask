using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Text.Json;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "admin, mainpage")]
    public class HeaderController : ControllerBase
    {
        private readonly IHeaderQuery _collection;
        private readonly ServiceContext _context;

        public HeaderController(IHeaderQuery collection, ServiceContext context)
        {
            _collection = collection;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetHeader()
        {
            try
            {
                var header = await _collection.SelectHeader(_context);
                if (header != null)
                {
                    return Ok(header);
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
        }

        [HttpPost]
        public async Task<ActionResult> CreateHeader([FromForm] string headerContent, IFormFile? headerPicture)
        {
            Header header;
            try
            {
                header = JsonSerializer.Deserialize<Header>(headerContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле headerContent: " + ex.Message);
                return BadRequest(ModelState);
            }

            if (header == null)
                return BadRequest("Неверный формат содержимого.");
            if (headerPicture != null && headerPicture.Length != 0)
                if (!FileValidator.IsValidImage(headerPicture))
                    return BadRequest("Неверный формат файла изображения.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);


            try
            {
                await _collection.CreateHeader(_context, header);

                header = await _collection.SelectHeader(_context);

                if (headerPicture != null && headerPicture.Length != 0)
                {
                    var pictureFileName = $"heroImage{Path.GetExtension(headerPicture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, pictureFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await headerPicture.CopyToAsync(stream);
                    }
                    header.ImagePath = pictureFileName;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return Ok(header);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHeader([FromForm] string headerContent, IFormFile? headerPicture)
        {

            Header header;
            try
            {
                header = JsonSerializer.Deserialize<Header>(headerContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле headerContent: " + ex.Message);
                return BadRequest(ModelState);
            }

            if (header == null)
                return BadRequest("Неверный формат содержимого.");
            if (headerPicture != null && headerPicture.Length != 0)
                if (!FileValidator.IsValidImage(headerPicture))
                    return BadRequest("Неверный формат файла изображения.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);


            try
            {
                await _collection.UpdateHeader(_context, header);

                header = await _collection.SelectHeader(_context);

                if (headerPicture != null && headerPicture.Length != 0)
                {
                    var pictureFileName = $"heroImage{Path.GetExtension(headerPicture.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, pictureFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await headerPicture.CopyToAsync(stream);
                    }
                    header.ImagePath = pictureFileName;
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return Ok(header);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.Json;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProgectController : ControllerBase
    {
        private readonly IProgectQuery _collection;
        private readonly ServiceContext _context;

        public ProgectController(IProgectQuery collection, ServiceContext context)
        {
            _collection = collection;
            _context = context;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProgect(int id)
        {
            try
            {
                Progect progect = await _collection.SelectProgect(_context, id);
                if (progect != null)
                {
                    if (progect.IllustrationId != "")
                    {
                        var deleteFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
                        var filePath = Path.Combine(deleteFolder, progect.IllustrationId);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    await _collection.DeleteProgect(_context, id);
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
        public async Task<ActionResult> SaveProgectRId([FromForm] string progectContent, IFormFile? progectPicture)
        {
            Progect progect;
            try
            {
                progect = await SaveProgect(progectContent, progectPicture);
                if (progect == null) return BadRequest(ModelState);
                return Ok(progect.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPost]
        public async Task<ActionResult> SaveProgectRContent([FromForm] string progectContent, IFormFile? progectPicture)
        {
            Progect? progect;
            try
            {
                progect = await SaveProgect(progectContent, progectPicture);
                if (progect == null) return BadRequest(ModelState);
                return Ok(progect);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        private async Task<Progect?> SaveProgect([FromForm] string progectContent, IFormFile? progectPicture)
        {

            Progect progect = new Progect();
            try
            {
                progect = JsonSerializer.Deserialize<Progect>(progectContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле progectContent: " + ex.Message);
                return null;
            }

            if (progectPicture != null && progectPicture.Length != 0)
                if (!FileValidator.IsValidImage(progectPicture))
                {
                    ModelState.AddModelError(string.Empty, "Неверный формат файла изображения.");
                    return null;
                }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);


            await _collection.AddProgect(_context, progect);
            progect = await _collection.SelectLastProgect(_context);

            if (progectPicture != null && progectPicture.Length != 0)
            {
                var pictureFileName = $"progectImage_{progect.ID}{Path.GetExtension(progectPicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, pictureFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await progectPicture.CopyToAsync(stream);
                }
                progect.IllustrationId = pictureFileName;
                await _context.SaveChangesAsync();

            }

            return progect;
        }


        [HttpPut]
        public async Task<ActionResult> UpdateProgectROk(int id, [FromForm] string progectContent, IFormFile? progectPicture)
        {
            Progect progect;
            try
            {
                progect = await UpdateProgect(id, progectContent, progectPicture);
                if (progect == null) return BadRequest(ModelState);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateProgectRContent(int id, [FromForm] string progectContent, IFormFile? progectPicture)
        {
            Progect? progect;
            try
            {
                progect = await UpdateProgect(id, progectContent, progectPicture);
                if (progect == null) return BadRequest(ModelState);
                return Ok(progect);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        private async Task<Progect?> UpdateProgect(int id, [FromForm] string progectContent, IFormFile? progectPicture)
        {

            Progect progect = new Progect();
            try
            {
                progect = JsonSerializer.Deserialize<Progect>(progectContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле progectContent: " + ex.Message);
                return null;
            }

            if (progectPicture != null && progectPicture.Length != 0)
                if (!FileValidator.IsValidImage(progectPicture))
                {
                    ModelState.AddModelError(string.Empty, "Неверный формат файла изображения.");
                    return null;
                }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);


            progect.ID = id;
            await _collection.UpdateProgect(_context, progect);
            progect = await _collection.SelectProgect(_context, id);

            if (progectPicture != null && progectPicture.Length != 0)
            {
                var pictureFileName = $"progectImage_{progect.ID}{Path.GetExtension(progectPicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, pictureFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await progectPicture.CopyToAsync(stream);
                }
                progect.IllustrationId = pictureFileName;
                await _context.SaveChangesAsync();

            }

            return progect;
        }


        [HttpGet]
        public async Task<ActionResult<Progect>> GetProgect(int id)
        {
            Progect progect;
            try
            {
                progect = await _collection.SelectProgect(_context, id);
                return Ok(progect);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Progect>>> GetAllProgects(string Search, int Page)
        {
            List<Progect> progects;
            try
            {
                switch (Search)
                {
                    case "ShowAll":
                        progects = await _collection.SelectAllProgects(_context);
                        break;
                    case "ShowPublished":
                        progects = await _collection.SelectAllPublishedProgects(_context);
                        break;
                    case "ShowInWork":
                        progects = await _collection.SelectAllProgectsInWork(_context);
                        break;
                    default:
                        throw new Exception("Неверный параметр поиска");
                }

                if (progects.Count > 0)
                {
                    progects.Reverse();
                    int pageCount = 6;
                    int totalPages = progects.Count / 6 + (progects.Count % 6 > 0 ? 1 : 0);
                    if (Page < 1 || Page > totalPages)
                    {
                        Page = 1;
                    }
                    if (Page == totalPages) pageCount = progects.Count % 6 > 0 ? progects.Count % 6 : 6;

                    GetAllProgectsResponseParamertes paramertes = new GetAllProgectsResponseParamertes()
                    {
                        Progects = progects.GetRange(Page * 6 - 6, pageCount),
                        CurrentPage = Page,
                        TotalPages = totalPages
                    };
                    return Ok(paramertes);
                }
                else
                {
                    GetAllProgectsResponseParamertes paramertes = new GetAllProgectsResponseParamertes()
                    {
                        Progects = progects,
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

    public class GetAllProgectsResponseParamertes
    {
        public List<Progect> Progects { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

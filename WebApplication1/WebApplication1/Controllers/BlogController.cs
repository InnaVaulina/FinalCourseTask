using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Controllers.RequestModel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "admin, blog")]
    public class BlogController: ControllerBase
    {
        private readonly IBlogQuery _collection;
        private readonly ServiceContext _context;

        public BlogController(IBlogQuery collection, ServiceContext context)
        {
            _collection = collection;
            _context = context;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBlog(int id) 
        {
            try 
            {
                Blog blog = await _collection.SelectBlog(_context, id);
                if (blog != null)
                {
                    if(blog.IllustrationId != "") 
                    {
                        var deleteFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
                        var filePath = Path.Combine(deleteFolder, blog.IllustrationId);
                        if (System.IO.File.Exists(filePath)) 
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }                    
                    await _collection.DeleteBlog(_context, id);
                }
                else 
                {
                    throw new Exception("объект не найден");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }



        [HttpPost]
        public async Task<ActionResult> SaveBlogRId([FromForm] string blogContent, IFormFile? blogPicture) 
        {
            Blog blog;
            try
            {
                blog = await SaveBlog(blogContent, blogPicture);
                if (blog == null) return BadRequest(ModelState);
                return Ok(blog.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> SaveBlogRContent([FromForm] string blogContent, IFormFile? blogPicture)
        {
            Blog? blog;
            try
            {
                blog = await SaveBlog(blogContent, blogPicture);
                if(blog == null) return BadRequest(ModelState);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }



        private async Task<Blog?> SaveBlog([FromForm] string blogContent, IFormFile? blogPicture)
        {

            Blog blog;
            try
            {
                blog = JsonSerializer.Deserialize<Blog>(blogContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле blogContent: " + ex.Message);
                return null;
            }

            if (blogPicture != null && blogPicture.Length != 0)
                if (!FileValidator.IsValidImage(blogPicture))
                {
                    ModelState.AddModelError(string.Empty, "Неверный формат файла изображения.");
                    return null;
                }


            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
        
            await _collection.AddBlog(_context,blog);
            blog = await _collection.SelectLastBlog(_context);

            if (blogPicture != null && blogPicture.Length != 0)
            {
                var pictureFileName = $"blogImage_{blog.ID}{Path.GetExtension(blogPicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, pictureFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await blogPicture.CopyToAsync(stream);
                }
                blog.IllustrationId = pictureFileName;
                await _context.SaveChangesAsync();
            }

            return blog;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBlogROk(int id, [FromForm] string blogContent, IFormFile? blogPicture)
        {
            Blog blog;
            try
            {
                blog = await UpdateBlog(id, blogContent, blogPicture);
                if (blog == null) return BadRequest(ModelState);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateBlogRContent(int id, [FromForm] string blogContent, IFormFile? blogPicture)
        {
            Blog? blog;
            try
            {
                blog = await UpdateBlog(id, blogContent, blogPicture);
                if (blog == null) return BadRequest(ModelState);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }

        private async Task<Blog?> UpdateBlog(int id, [FromForm] string blogContent, IFormFile? blogPicture)
        {
            Blog blog;
            try
            {
                blog = JsonSerializer.Deserialize<Blog>(blogContent);
            }
            catch (JsonException ex)
            {
                ModelState.AddModelError(string.Empty, "Неверный формат JSON в поле blogContent: " + ex.Message);
                return null;
            }

            if (blogPicture != null && blogPicture.Length != 0)
                if (!FileValidator.IsValidImage(blogPicture))
                {
                    ModelState.AddModelError(string.Empty, "Неверный формат файла изображения.");
                    return null;
                }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            
            blog.ID = id;
            await _collection.UpdateBlog(_context,blog);
            blog = await _collection.SelectBlog(_context,id);

            if (blogPicture != null && blogPicture.Length != 0)
            {
                var pictureFileName = $"blogImage_{blog.ID}{Path.GetExtension(blogPicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, pictureFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await blogPicture.CopyToAsync(stream);
                }
                blog.IllustrationId = pictureFileName;
                await _context.SaveChangesAsync();
            }

            return blog;
        }

        


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            Blog blog;
            try
            {
                blog = await _collection.SelectBlog(_context, id);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }


       

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs(string Search, string BeginDate, string EndDate, int Page)
        {
            List<Blog> blogs;
            try
            {
                DateTime beginDate = DateTime.ParseExact(BeginDate, "yyyy-MM-ddTHH:mm:ss", null);
                DateTime endDate = DateTime.ParseExact(EndDate, "yyyy-MM-ddTHH:mm:ss", null);
                switch (Search)
                {
                    case "ShowAll":
                        blogs = await _collection.SelectAllBlogs(_context, beginDate, endDate);
                        break;
                    case "ShowPublished":
                        blogs = await _collection.SelectAllPublishedBlogs(_context, beginDate, endDate);
                        break;
                    case "ShowInWork":
                        blogs = await _collection.SelectAllBlogsInWork(_context, beginDate, endDate);
                        break;
                    default:
                        throw new Exception("Неверный параметр поиска");
                }

                if (blogs.Count > 0) 
                {
                    blogs.Reverse();
                    int pageCount = 6;
                    int totalPages = blogs.Count / 6 + (blogs.Count % 6 > 0 ? 1 : 0);
                    if (Page < 1 || Page > totalPages)
                    {
                        Page = 1;
                    }
                    if (Page == totalPages) pageCount = blogs.Count % 6 > 0 ? blogs.Count % 6 : 6;

                    GetAllBlogsResponseParamertes paramertes = new GetAllBlogsResponseParamertes()
                    {
                        Blogs = blogs.GetRange(Page * 6 - 6, pageCount),
                        CurrentPage = Page,
                        TotalPages = totalPages
                    };
                    return Ok(paramertes);
                }
                else 
                {
                    GetAllBlogsResponseParamertes paramertes = new GetAllBlogsResponseParamertes()
                    {
                        Blogs = blogs,
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


    public class GetAllBlogsResponseParamertes
    {
        public List<Blog> Blogs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.RequestModel;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public interface IBlogQuery 
    {
        Task<List<Blog>> SelectAllBlogs(ServiceContext context, DateTime beginDate, DateTime endDate);
        Task<List<Blog>> SelectAllPublishedBlogs(ServiceContext context, DateTime beginDate, DateTime endDate);
        Task<List<Blog>> SelectAllBlogsInWork(ServiceContext context, DateTime beginDate, DateTime endDate);
        Task<Blog> SelectLastBlog(ServiceContext context);
        Task<Blog> SelectBlog(ServiceContext context, int id);

        Task AddBlog(ServiceContext context, Blog note);
        Task UpdateBlog(ServiceContext context, Blog note);

        Task DeleteBlog(ServiceContext context, int id);
    }
    public class BlogQuery: IBlogQuery
    {
        public async Task<List<Blog>> SelectAllBlogs(ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate).ToListAsync();
        }

        public async Task<List<Blog>> SelectAllPublishedBlogs(ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate && b.Status == "Published").ToListAsync();
        }

        public async Task<List<Blog>> SelectAllBlogsInWork(ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate && b.Status == "InWork").ToListAsync();
        }

        public async Task<Blog> SelectLastBlog(ServiceContext context)
        {
            return await context.Blogs.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public async Task<Blog> SelectBlog(ServiceContext context, int id)
        {
            return await context.Blogs.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddBlog(ServiceContext context, Blog note)
        {
            try
            {
                context.Blogs.Add(note);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

        }

        public async Task UpdateBlog(ServiceContext context, Blog note)
        {
            try
            {
                var desired = await context.Blogs.Where(r => r.ID == note.ID).SingleAsync();
                if (desired != null)
                {
                    desired.Title = note.Title;
                    desired.PostDate = note.PostDate;
                    desired.Article = note.Article;
                    desired.IllustrationId = note.IllustrationId;
                    desired.Status = note.Status;
                }
                else
                    throw new Exception("не найден объект для изменения");

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task DeleteBlog(ServiceContext context, int id)
        {
            try 
            {
                var desired = await context.Blogs.Where(r => r.ID == id).SingleAsync();
                if (desired != null)
                    context.Blogs.Remove(desired);
                else
                    throw new Exception("не найден объект для удаления");
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
    }
}

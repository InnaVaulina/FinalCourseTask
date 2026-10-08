using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.RequestModel;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class BlogQuery
    {
        public static async Task<List<Blog>> SelectAllBlogs(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate).ToListAsync();
        }

        public static async Task<List<Blog>> SelectAllPublishedBlogs(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate && b.Status == "Published").ToListAsync();
        }

        public static async Task<List<Blog>> SelectAllBlogsInWork(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Blogs.Where(b => b.PostDate >= beginDate && b.PostDate <= endDate && b.Status == "InWork").ToListAsync();
        }

        public static async Task<Blog> SelectLastBlog(this ServiceContext context)
        {
            return await context.Blogs.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public static async Task<Blog> SelectBlog(this ServiceContext context, int id)
        {
            return await context.Blogs.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public static async Task AddBlog(this ServiceContext context, Blog note)
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

        public static async Task UpdateBlog(this ServiceContext context, Blog note)
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

        public static async Task DeleteBlog(this ServiceContext context, int id)
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

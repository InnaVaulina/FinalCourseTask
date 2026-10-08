using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class ProgectQuery
    {
        public static async Task<List<Progect>> SelectAllProgects(this ServiceContext context)
        {
            return await context.Progects.ToListAsync();
        }

        public static async Task<List<Progect>> SelectAllPublishedProgects(this ServiceContext context) 
        {
            return await context.Progects.Where(b => b.Status == "Published").ToListAsync();
        }
        public static async Task<List<Progect>> SelectAllProgectsInWork(this ServiceContext context) 
        {
            return await context.Progects.Where(b => b.Status == "InWork").ToListAsync();
        }
        public static async Task<Progect> SelectLastProgect(this ServiceContext context)
        {
            return await context.Progects.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public static async Task<Progect> SelectProgect(this ServiceContext context, int id)
        {
            return await context.Progects.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public static async Task AddProgect(this ServiceContext context, Progect note)
        {
            try
            {
                context.Progects.Add(note);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

        }

        public static async Task UpdateProgect(this ServiceContext context, Progect note)
        {
            try
            {
                var desired = await context.Progects.Where(r => r.ID == note.ID).SingleAsync();
                if (desired != null)
                {
                    desired.Title = note.Title;
                    desired.Description = note.Description;
                    desired.Status = note.Status;
                    desired.IllustrationId = note.IllustrationId;
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

        public static async Task DeleteProgect(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Progects.Where(r => r.ID == id).SingleAsync();
                if (desired != null)
                    context.Progects.Remove(desired);
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

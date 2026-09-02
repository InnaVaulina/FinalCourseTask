using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public interface IProgectQuery 
    {
        Task<List<Progect>> SelectAllProgects(ServiceContext context);
        Task<List<Progect>> SelectAllPublishedProgects(ServiceContext context);
        Task<List<Progect>> SelectAllProgectsInWork(ServiceContext context);
        Task<Progect> SelectLastProgect(ServiceContext context);
        Task<Progect> SelectProgect(ServiceContext context, int id);

        Task AddProgect(ServiceContext context, Progect note);
        Task UpdateProgect(ServiceContext context, Progect note);

        Task DeleteProgect(ServiceContext context, int id);
    }
    public class ProgectQuery: IProgectQuery
    {
        public async Task<List<Progect>> SelectAllProgects(ServiceContext context)
        {
            return await context.Progects.ToListAsync();
        }

        public async Task<List<Progect>> SelectAllPublishedProgects(ServiceContext context) 
        {
            return await context.Progects.Where(b => b.Status == "Published").ToListAsync();
        }
        public async Task<List<Progect>> SelectAllProgectsInWork(ServiceContext context) 
        {
            return await context.Progects.Where(b => b.Status == "InWork").ToListAsync();
        }
        public async Task<Progect> SelectLastProgect(ServiceContext context)
        {
            return await context.Progects.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public async Task<Progect> SelectProgect(ServiceContext context, int id)
        {
            return await context.Progects.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddProgect(ServiceContext context, Progect note)
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

        public async Task UpdateProgect(ServiceContext context, Progect note)
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

        public async Task DeleteProgect(ServiceContext context, int id)
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

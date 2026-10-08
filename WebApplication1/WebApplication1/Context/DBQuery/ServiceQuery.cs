using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class ServiceQuery 
    {
        public static async Task<List<BusinesService>> SelectAllServices(this ServiceContext context)
        { 
            return await context.Services.ToListAsync();
        }

        public static async Task<BusinesService> SelectLastService(this ServiceContext context)
        {
            return await context.Services.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public static async Task<BusinesService> SelectService(this ServiceContext context, int id)
        {
            return await context.Services.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public static async Task AddService(this ServiceContext context, BusinesService note)
        {
            try
            {
                context.Services.Add(note);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

        }

        public static async Task UpdateService(this ServiceContext context, BusinesService note)
        {
            try
            {
                var desired = await context.Services.Where(r => r.ID == note.ID).SingleAsync();
                if (desired != null)
                {
                    desired.Title = note.Title;
                    desired.Description = note.Description;
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

        public static async Task DeleteService(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Services.Where(r => r.ID == id).SingleAsync();
                if (desired != null)
                    context.Services.Remove(desired);
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

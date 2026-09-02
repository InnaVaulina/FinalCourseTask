using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{

    public interface IServiceQuery
    {
        Task<List<BusinesService>> SelectAllServices(ServiceContext context);

        Task<BusinesService> SelectLastService(ServiceContext context);
        Task<BusinesService> SelectService(ServiceContext context, int id);

        Task AddService(ServiceContext context, BusinesService note);
        Task UpdateService(ServiceContext context, BusinesService note);

        Task DeleteService(ServiceContext context, int id);
    }
    public class ServiceQuery : IServiceQuery
    {
        public async Task<List<BusinesService>> SelectAllServices(ServiceContext context)
        { 
            return await context.Services.ToListAsync();
        }

        public async Task<BusinesService> SelectLastService(ServiceContext context)
        {
            return await context.Services.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public async Task<BusinesService> SelectService(ServiceContext context, int id)
        {
            return await context.Services.Where(b => b.ID == id).FirstOrDefaultAsync();
        }

        public async Task AddService(ServiceContext context, BusinesService note)
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

        public async Task UpdateService(ServiceContext context, BusinesService note)
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

        public async Task DeleteService(ServiceContext context, int id)
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

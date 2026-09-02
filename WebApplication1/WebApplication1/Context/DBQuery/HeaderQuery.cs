using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public interface IHeaderQuery
    {
        Task<Header> SelectHeader(ServiceContext context);
        Task UpdateHeader(ServiceContext context, Header header);
        Task CreateHeader(ServiceContext context, Header header);
    }
    public class HeaderQuery: IHeaderQuery
    {
        public async Task<Header> SelectHeader(ServiceContext context)
        {
            return await context.Header.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public async Task UpdateHeader(ServiceContext context, Header header)
        {
            try
            {
                var desired = await context.Header.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
                if (desired != null)
                {
                    desired.HtmlPattern = header.HtmlPattern;
                    desired.Title = header.Title;
                    desired.Aims = header.Aims;
                    desired.Motto = header.Motto;
                    desired.ButtonText = header.ButtonText;
                    desired.ImagePath = header.ImagePath;
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("объект не найден");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task CreateHeader(ServiceContext context, Header header)
        {
            try
            {
                var any = context.Header.Any();
                if (any)
                {
                    throw new Exception("объект уже существует");
                }
                context.Header.Add(header);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
    }
}

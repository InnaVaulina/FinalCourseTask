using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class HeaderQuery
    {
        public static async Task<Header> SelectHeader(this ServiceContext context)
        {
            return await context.Header.OrderByDescending(b => b.ID).FirstOrDefaultAsync();
        }

        public static async Task UpdateHeader(this ServiceContext context, Header header)
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

        public static async Task CreateHeader(this ServiceContext context, Header header)
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

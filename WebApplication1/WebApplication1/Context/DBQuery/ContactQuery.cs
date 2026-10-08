using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class ContactQuery
    {
        public static async Task<List<Contact>> SelectAllContacts(this ServiceContext context) 
        {
            return await context.Contacts
                .AsNoTracking()
                .Include(c => c.Address)
                .Include(c => c.Phones)
                .Include(c => c.Mails)
                .Include(c => c.Links)
                .ToListAsync();
        }

        public static async Task<Contact?> SelectLastContact(this ServiceContext context) 
        {
            return await context.Contacts.OrderByDescending(b => b.ID)
                .Include(c => c.Address)
                .Include(c => c.Phones)
                .Include(c => c.Mails)
                .Include(c => c.Links)
                .FirstOrDefaultAsync();
        }

        public static async Task<Contact?> SelectContact(this ServiceContext context, int id) 
        {
            return await context.Contacts.Where(b => b.ID == id)
                .Include(c => c.Address)
                .Include(c => c.Phones)
                .Include(c => c.Mails)
                .Include(c => c.Links)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public static async Task AddContact(this ServiceContext context, Contact note) 
        {
            try
            {
                context.Contacts.Add(note);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public static async Task<ContactAddress?> AddAddress(this ServiceContext context, ContactAddress note)
        {
            try
            {
                var addresses = await context.Addresses.Where(a => a.ContactId == note.ContactId).ToListAsync();
                if (!addresses.Any())
                {
                    var exists = await context.Contacts.AnyAsync(c => c.ID == note.ContactId);
                    if (exists)
                    {
                        context.Addresses.Add(note);
                        await context.SaveChangesAsync();
                    }
                    else throw new Exception("нет такого контакта");
                }
                else throw new Exception("может быть только один адрес");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            var newNote = context.Addresses.Where(c => c.ContactId == note.ContactId)
                .FirstOrDefaultAsync().Result;
            return newNote;
        }

        public static async Task<ContactPhone?> AddPhone(this ServiceContext context, ContactPhone note)
        {
            try
            {
                var exists = await context.Contacts.AnyAsync(c => c.ID == note.ContactId);
                if (exists)
                {
                    context.Phones.Add(note);
                    await context.SaveChangesAsync();
                }
                else throw new Exception("нет такого контакта");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            var lastNote = context.Phones.Where(c => c.ContactId == note.ContactId)
                .OrderByDescending(p => p.ID).FirstOrDefaultAsync().Result;
            return lastNote;
        }

        public static async Task<ContactMail?> AddMail(this ServiceContext context, ContactMail note)
        {
            try
            {
                var exists = await context.Contacts.AnyAsync(c => c.ID == note.ContactId);
                if (exists)
                {
                    context.Mails.Add(note);
                    await context.SaveChangesAsync();
                }
                else throw new Exception("нет такого контакта");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            var lastNote = context.Mails.Where(c => c.ContactId == note.ContactId)
                .OrderByDescending(p => p.ID).FirstOrDefaultAsync().Result;
            return lastNote;
        }

        public static async Task<ContactSocialLink?> AddSocialLink(this ServiceContext context, ContactSocialLink note)
        {
            try
            {
                var exists = await context.Contacts.AnyAsync(c => c.ID == note.ContactId);
                if (exists)
                {
                    context.SocialLinks.Add(note);
                    await context.SaveChangesAsync();
                }
                else throw new Exception("нет такого контакта");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            var lastNote = context.SocialLinks.Where(c => c.ContactId == note.ContactId)
                .OrderByDescending(p => p.ID).FirstOrDefaultAsync().Result;
            return lastNote;
        }

        public static async Task DeleteContact(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Contacts.Where(b => b.ID == id)
                .Include(c => c.Address)
                .Include(c => c.Phones)
                .Include(c => c.Mails)
                .Include(c => c.Links)
                .FirstOrDefaultAsync();

                if (desired != null) 
                {
                    context.Contacts.Remove(desired);
                    await context.SaveChangesAsync();
                }                   
                else
                    throw new Exception("не найден объект для удаления");
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public static async Task DeleteAddress(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Addresses.FirstOrDefaultAsync(r => r.ID == id);
                if (desired != null)
                {
                    context.Addresses.Remove(desired);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("не найден объект для удаления");
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public static async Task DeletePhone(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Phones.FirstOrDefaultAsync(r => r.ID == id);
                if (desired != null)
                {
                    context.Phones.Remove(desired);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("не найден объект для удаления");
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public static async Task DeleteMail(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.Mails.FirstOrDefaultAsync(r => r.ID == id);
                if (desired != null)
                {
                    context.Mails.Remove(desired);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("не найден объект для удаления");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public static async Task DeleteSocialLink(this ServiceContext context, int id)
        {
            try
            {
                var desired = await context.SocialLinks.FirstOrDefaultAsync(r => r.ID == id);
                if (desired != null)
                {
                    context.SocialLinks.Remove(desired);
                    await context.SaveChangesAsync();
                }
                else
                    throw new Exception("не найден объект для удаления"); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
    }
}

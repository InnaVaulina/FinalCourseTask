using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Numerics;
using System.Text.Json;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactQuery _collection;
        private readonly ServiceContext _context;

        public ContactController(IContactQuery collection, ServiceContext context)
        {
            _collection = collection;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            List<Contact> contacts;
            try
            {
                contacts = await _collection.SelectAllContacts(_context);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            Contact contact;
            try
            {
                contact = await _collection.SelectContact(_context, id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public async Task<ActionResult> SaveContact([FromForm] string contactContent, IFormFile? addressPicture, List<IFormFile>? socialIcons)
        {
            Contact newContent = JsonSerializer.Deserialize<Contact>(contactContent);

            if (newContent == null)
                return BadRequest("Неверный формат содержимого.");

            if (socialIcons == null && newContent.Links.Count > 0)
                return BadRequest("Файл изображения отсутсвует.");
            if (socialIcons != null && newContent.Links.Count != socialIcons.Count)
                return BadRequest("Файл изображения отсутсвует.");

            if (addressPicture != null && addressPicture.Length != 0)
                if (!FileValidator.IsValidImage(addressPicture))
                    return BadRequest("Неверный формат файла адреса.");
            foreach (var f in socialIcons ?? Enumerable.Empty<IFormFile>())
            {
                if (!FileValidator.IsValidImage(f))
                    return BadRequest("Неверный формат одного из изображений социальных иконок.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            try
            {
                await _collection.AddContact(_context, newContent);
                newContent = await _collection.SelectLastContact(_context);

                if (addressPicture != null && addressPicture.Length != 0)
                {
                    var newMapFileName = $"mapImage_{newContent.ID}_{newContent.Address.ID}{Path.GetExtension(addressPicture.FileName)}";
                    var addressFilePath = Path.Combine(uploadsFolder, newMapFileName);
                    using (var stream = new FileStream(addressFilePath, FileMode.Create))
                    {
                        await addressPicture.CopyToAsync(stream);
                    }
                    newContent.Address.MapFileName = newMapFileName;

                    await _context.SaveChangesAsync();
                }

                if (newContent.Links.Count > 0)
                {
                    for (int i = 0; i < newContent.Links.Count; i++)
                    {
                        var link = newContent.Links[i];
                        var socialIconFile = socialIcons[i];
                        var newIconFileName = $"socialIcon_{newContent.ID}_{link.ID}{Path.GetExtension(socialIconFile.FileName)}";
                        var iconFilePath = Path.Combine(uploadsFolder, newIconFileName);
                        using (var stream = new FileStream(iconFilePath, FileMode.Create))
                        {
                            await socialIconFile.CopyToAsync(stream);
                        }
                        link.IkonFileName = newIconFileName;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return Ok(newContent);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateContact(int id, [FromForm] string contactContent, [FromForm] string deletedContactContent, IFormFile? addressPicture, List<IFormFile>? socialIcons)
        {
            Contact newContent;
            Contact deletedContent;
            try 
            {
                newContent = JsonSerializer.Deserialize<Contact>(contactContent);
                deletedContent = JsonSerializer.Deserialize<Contact>(deletedContactContent);
            }
            catch (Exception ex)
            {
                return BadRequest("десериализация");
            }

            if (newContent == null)
                return BadRequest("Неверный формат содержимого.");
            if(deletedContent == null)
                return BadRequest("Неверный формат содержимого.");
            if (socialIcons == null && newContent.Links.Count > 0)
                return BadRequest("Файл изображения отсутсвует.");
            if (socialIcons != null && newContent.Links.Count != socialIcons.Count)
                return BadRequest("Файл изображения отсутсвует.");

            if (addressPicture != null && addressPicture.Length != 0)
                if (!FileValidator.IsValidImage(addressPicture))
                    return BadRequest("Неверный формат файла адреса.");
            foreach (var f in socialIcons ?? Enumerable.Empty<IFormFile>())
            {
                if (!FileValidator.IsValidImage(f))
                    return BadRequest("Неверный формат одного из изображений социальных иконок.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            try
            {
                string? addressFilePath = null;
                List<string> socialIconFilePaths = new List<string>();

                if (deletedContent.Address != null)
                {
                    if (deletedContent.Address.MapFileName != "")
                        addressFilePath = Path.Combine(uploadsFolder, deletedContent.Address.MapFileName);
                    await _collection.DeleteAddress(_context, deletedContent.Address.ID);
                }

                foreach (var phone in deletedContent.Phones)
                    await _collection.DeletePhone(_context, phone.ID);
                foreach (var mail in deletedContent.Mails)
                    await _collection.DeleteMail(_context, mail.ID);


                foreach (var link in deletedContent.Links)
                {
                    socialIconFilePaths.Add(Path.Combine(uploadsFolder, link.IkonFileName));
                    await _collection.DeleteSocialLink(_context, link.ID);
                }
                if (addressFilePath != null && System.IO.File.Exists(addressFilePath))
                    System.IO.File.Delete(addressFilePath);
                foreach (var path in socialIconFilePaths)
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                return BadRequest("десериализация");
            }

            try 
            {
                string? addressFilePath = null;

                var content = await _collection.SelectContact(_context, id);
                if(content == null)
                    return BadRequest("Объект для изменения не найден.");

                content.Name = newContent.Name;

                if (newContent.Address != null)
                {
                    if (newContent.Address.ID == 0)
                    {
                        if(!_context.Addresses.Any(a => a.ContactId == id))
                            await _collection.AddAddress(_context, newContent.Address);
                    }
                    else
                    {
                        var desired = _context.Addresses.FirstOrDefault(a => a.ID == newContent.Address.ID);
                        if (desired != null)
                        {
                            desired.Address = newContent.Address.Address;
                            desired.MapFileName = newContent.Address.MapFileName;
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                    var addressContent = _context.Addresses.FirstOrDefault(a => a.ContactId == id);

                    if (addressContent.MapFileName == "")
                        if (addressPicture != null && addressPicture.Length != 0)
                        {
                            var newMapFileName = $"mapImage_{addressContent.ID}_{addressContent.ID}{Path.GetExtension(addressPicture.FileName)}";
                            addressFilePath = Path.Combine(uploadsFolder, newMapFileName);
                            using (var stream = new FileStream(addressFilePath, FileMode.Create))
                            {
                                await addressPicture.CopyToAsync(stream);
                            }
                            addressContent.MapFileName = newMapFileName;
                            await _context.SaveChangesAsync();
                        }
                }

            }
            catch (Exception ex)
            {
                return BadRequest("блок адреса");
            }

            try
            {

                foreach (var phone in newContent.Phones)
                {
                    if (phone.ID == 0)
                    {
                        await _collection.AddPhone(_context, phone);
                    }
                    else
                    {
                        var existingPhone = _context.Phones.FirstOrDefault(p => p.ID == phone.ID);
                        if (existingPhone != null)
                        {
                            existingPhone.Phone = phone.Phone;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest("блок телефоны");
            }

            try
            {

                foreach (var mail in newContent.Mails)
                {
                    if (mail.ID == 0)
                    {
                        await _collection.AddMail(_context, mail);
                    }
                    else
                    {
                        var existingMail = _context.Mails.FirstOrDefault(m => m.ID == mail.ID);
                        if (existingMail != null)
                        {
                            existingMail.Mail = mail.Mail;
                            await _context.SaveChangesAsync();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return BadRequest("блок почта");
            }

            try
            {
                for (int i = 0; i < newContent.Links.Count; i++)
                {
                    var newlink = newContent.Links[i];
                    ContactSocialLink? link = null;
                    if (newlink.ID == 0)
                    {
                        link = await _collection.AddSocialLink(_context, newlink);
                    }
                    else
                    {
                        link = _context.SocialLinks.FirstOrDefault(l => l.ID == newlink.ID);
                        if (link != null)
                        {
                            link.SocialLink = newlink.SocialLink;
                            link.IkonFileName = newlink.IkonFileName;
                            await _context.SaveChangesAsync();
                        }
                    }

                    if(link.IkonFileName == "")
                    {
                        var socialIconFile = socialIcons[i];
                        var newIconFileName = $"socialIcon_{newContent.ID}_{newlink.ID}{Path.GetExtension(socialIconFile.FileName)}";
                        var iconFilePath = Path.Combine(uploadsFolder, newIconFileName);
                        using (var stream = new FileStream(iconFilePath, FileMode.Create))
                        {
                            await socialIconFile.CopyToAsync(stream);
                        }
                        link.IkonFileName = newIconFileName;
                        await _context.SaveChangesAsync();
                    }
                }


            }
            catch (Exception ex)
            {
                

                return BadRequest("блок  сслылки");
            }
            return Ok(newContent);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteContact(int id)
        {
            Contact contact;
            string? addressFilePath = null;
            List<string> socialIconFilePaths = new List<string>();
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");

            try
            {
                contact = await _collection.SelectContact(_context, id);
                if (contact.Address != null && contact.Address.MapFileName != "")
                    addressFilePath = Path.Combine(uploadsFolder, contact.Address.MapFileName);

                foreach (var link in contact.Links)
                    socialIconFilePaths.Add(Path.Combine(uploadsFolder, link.IkonFileName));

                await _collection.DeleteContact(_context, id);
                contact = await _collection.SelectContact(_context, id);
                if (contact == null)
                {
                    if (addressFilePath != null && System.IO.File.Exists(addressFilePath))
                        System.IO.File.Delete(addressFilePath);
                    foreach (var path in socialIconFilePaths)
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    return NoContent();
                }
                else
                    throw new Exception("Не удалось удалить объект");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            ContactAddress? address;
            string? addressFilePath = null;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            try
            {
                address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.ID == id);
                if (address == null)
                    return BadRequest("Не найден объект для удаления");
                
                if (address.MapFileName != "")
                    addressFilePath = Path.Combine(uploadsFolder, address.MapFileName);

                await _collection.DeleteAddress(_context, id);
                address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.ID == id);
                if (address == null)
                {
                    if (addressFilePath != null && System.IO.File.Exists(addressFilePath))
                        System.IO.File.Delete(addressFilePath);
                    
                    return NoContent();
                }
                else
                    throw new Exception("Не удалось удалить объект");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePhone(int id)
        {
            ContactPhone? phone = null;
            try
            {
                phone = await _context.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);
                if(phone == null)
                    return BadRequest("Не найден объект для удаления");

                await _collection.DeletePhone(_context, id);
                phone = await _context.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);
                if (phone == null)
                    return NoContent();
                else
                    throw new Exception("Не удалось удалить объект");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMail(int id)
        {
            ContactMail? mail = null;
            try
            {
                mail = await _context.Mails.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                if(mail == null)
                    return BadRequest("Не найден объект для удаления");
                
                await _collection.DeleteMail(_context, id);
                mail = await _context.Mails.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                if (mail == null)
                    return NoContent();
                else
                    throw new Exception("Не удалось удалить объект");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSocialLink(int id)
        {
            string? iconFilePath = null;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            try
            {
                var link = await _context.SocialLinks.AsNoTracking().FirstOrDefaultAsync(l => l.ID == id);
                if(link == null)
                    return BadRequest("Не найден объект для удаления");
                
                if (link.IkonFileName != "")
                    iconFilePath = Path.Combine(uploadsFolder, link.IkonFileName);
                await _collection.DeleteSocialLink(_context, id);
                link = await _context.SocialLinks.AsNoTracking().FirstOrDefaultAsync(l => l.ID == id);
                if (link == null)
                {
                    if (iconFilePath != null && System.IO.File.Exists(iconFilePath))
                        System.IO.File.Delete(iconFilePath);
                    return NoContent();
                }
                else
                    throw new Exception("Не удалось удалить объект");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }

        }
    }
}

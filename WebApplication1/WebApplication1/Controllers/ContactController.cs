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

            try
            {
                await _collection.AddContact(_context, newContent);
                newContent = await _collection.SelectLastContact(_context);

                if (addressPicture != null && addressPicture.Length != 0)
                    newContent.Address.MapFileName = await SavePicture(newContent.ID, newContent.Address.ID, addressPicture, "mapImage");
                

                if (newContent.Links.Count > 0)
                {
                    for (int i = 0; i < newContent.Links.Count; i++)
                    {
                        var link = newContent.Links[i];    
                        link.IkonFileName = await SavePicture(newContent.ID, link.ID, socialIcons[i], "socialIcon");
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            return Ok(newContent);
        }

        private async Task<string> SavePicture(int contactId,int elementId, IFormFile file, string fileName) 
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var newFileName = $"{fileName}_{contactId}_{elementId}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, newFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return newFileName;
        }

        private async Task DeletePicture(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "IMG");
            var filePath = Path.Combine(uploadsFolder, fileName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateContact(int id, [FromForm] string contactContent, [FromForm] string? deletedContactContent, IFormFile? addressPicture, List<IFormFile>? socialIcons)
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
                return BadRequest("Ошибка десериализации");
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
                if (f.Length == 0) continue;
                if (!FileValidator.IsValidImage(f))
                    return BadRequest("Неверный формат одного из изображений социальных иконок.");
            }

            Contact? content;

            try
            {
                content = await _collection.SelectContact(_context, id);
                if (content == null)
                    return BadRequest("Объект для изменения не найден.");
                content.Name = newContent.Name;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Объект для изменения не найден.");
            }

            try
            {
                if (deletedContent.Address != null)
                {
                    string? addressFileName = null;
                    if (deletedContent.Address.MapFileName != "")
                        addressFileName = deletedContent.Address.MapFileName;
                    await _collection.DeleteAddress(_context, deletedContent.Address.ID);
                    if(addressFileName != null)
                        await DeletePicture(addressFileName);
                }

                foreach (var phone in deletedContent.Phones)
                    await _collection.DeletePhone(_context, phone.ID);
                foreach (var mail in deletedContent.Mails)
                    await _collection.DeleteMail(_context, mail.ID);

                foreach (var link in deletedContent.Links)
                {
                    List<string> socialIconFileNames = new List<string>();
                    socialIconFileNames.Add(link.IkonFileName);
                    await _collection.DeleteSocialLink(_context, link.ID);
                    foreach(var fileName in socialIconFileNames)
                        await DeletePicture(fileName);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка при удалении данных");
            }

            try 
            {
                if (newContent.Address != null)
                {
                    ContactAddress? addressContent = null;
                    if (newContent.Address.ID == 0)
                    {
                        if(!_context.Addresses.Any(a => a.ContactId == id))
                            addressContent = await _collection.AddAddress(_context, newContent.Address);
                    }
                    else
                    {
                        addressContent = _context.Addresses.FirstOrDefault(a => a.ID == newContent.Address.ID);
                        if (addressContent != null)
                        {
                            addressContent.Address = newContent.Address.Address;
                            addressContent.MapFileName = newContent.Address.MapFileName;
                        }
                    }

                    if (addressContent.MapFileName == "")
                        if (addressPicture != null && addressPicture.Length != 0)
                            addressContent.MapFileName = await SavePicture(newContent.ID, addressContent.ID, addressPicture, "mapImage");
                            
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка в блоке адреса");
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
                            existingPhone.Phone = phone.Phone;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка в блоке телефоны");
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
                            existingMail.Mail = mail.Mail;                          
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка в блоке почта");
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
                        }
                    }

                    if (link.IkonFileName == "")
                        link.IkonFileName = await SavePicture(newContent.ID, link.ID, socialIcons[i], "socialIcon");
                    
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка в блоке ссылки");
            }

            content = await _collection.SelectContact(_context, id);
            return Ok(content);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteContact(int id)
        {
            string? addressFileName = null;
            List<string> socialIconFileNames = new List<string>();

            try
            {
                Contact contact = await _collection.SelectContact(_context, id);
                if(contact == null)
                    throw new Exception("Не найден объект для удаления");

                if (contact.Address != null && contact.Address.MapFileName != "")
                    addressFileName = contact.Address.MapFileName;

                foreach (var link in contact.Links)
                    socialIconFileNames.Add(link.IkonFileName);

                await _collection.DeleteContact(_context, id);

                contact = await _collection.SelectContact(_context, id);
                if (contact == null)
                {
                    if(addressFileName!=null) await DeletePicture(addressFileName);
                    foreach (var fileName in socialIconFileNames)
                        if (fileName != null) await DeletePicture(fileName);
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

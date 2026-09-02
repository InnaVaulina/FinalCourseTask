using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class ServiceContext : IdentityDbContext<AppUser>
    {
        public ServiceContext(DbContextOptions<ServiceContext> options)
            : base(options)
        {
        }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Blog> Blogs { get; set; } 
        
        public DbSet<Progect> Progects { get; set; } 

        public DbSet<BusinesService> Services { get; set; }

        public DbSet<Header> Header { get; set; }


        public DbSet<Contact> Contacts { get; set; } 
        public DbSet<ContactAddress> Addresses { get; set; }
        public DbSet<ContactPhone> Phones { get; set; }
        public DbSet<ContactMail> Mails { get; set; }
        public DbSet<ContactSocialLink> SocialLinks { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>()
                .HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<ContactAddress>(a => a.ContactId)
                .IsRequired();

            builder.Entity<Contact>()
                .HasMany(c => c.Phones)
                .WithOne()
                .HasForeignKey(p => p.ContactId)
                .IsRequired();

            builder.Entity<Contact>()
                .HasMany(c => c.Mails)
                .WithOne()
                .HasForeignKey(m => m.ContactId)
                .IsRequired();

            builder.Entity<Contact>()
                .HasMany(c => c.Links)
                .WithOne()
                .HasForeignKey(l => l.ContactId)
                .IsRequired();
        }
    }

}


using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Controllers.RequestModel;

namespace WebApplication1.Context.DBQuery
{
    public static class RequestQuery
    {

        public static async Task<Request> SelectExample(this ServiceContext context, int id) 
        {
            return await context.Requests.Where(r => r.ID == id).SingleAsync();
        }

        public static async Task<List<Request>> SelectAllRequests(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.
                Where(r => (r.RequestIn <= endDate)&& (r.RequestIn >= beginDate)).
                ToListAsync();
        }

        public static async Task<List<Request>> SelectReceived(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "received")).
                Where(r => (r.RequestIn <= endDate) && (r.RequestIn >= beginDate)).
                ToListAsync();
        }

        public static async Task<List<Request>> SelectTakenOnWork(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "taken")).
                Where(r => (r.RequestIn <= endDate) && (r.RequestIn >= beginDate)).
                ToListAsync();
        }


        public static async Task<List<Request>> SelectRejected(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "rejected")).
                Where(r => (r.RequestIn <= endDate) && (r.RequestIn >= beginDate)).
                ToListAsync();
        }


        public static async Task<List<Request>> SelectFinished(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "finished")).
                Where(r => (r.RequestIn <= endDate) && (r.RequestIn >= beginDate)).
                ToListAsync();
        }

        public static async Task<List<Request>> SelectCancelled(this ServiceContext context, DateTime beginDate, DateTime endDate)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "cancelled")).
                Where(r => (r.RequestIn <= endDate) && (r.RequestIn >= beginDate)).
                ToListAsync();
        }


        public static async Task AddRequest(this ServiceContext context, Request note)
        {
            try
            {
                context.Requests.Add(note);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

        }

        public static async Task<Request> UpdateRequest(this ServiceContext context, int id) 
        {
            return await context.Requests.Where(r => r.ID == id).SingleAsync();
        }


        public static async Task SaveRequest(this ServiceContext context, Request note) 
        {
            try
            {

                var desired = await context.Requests.Where(r => r.ID == note.ID).SingleAsync();
                if (desired != null) 
                {
                    desired.RequestText = note.RequestText;
                    desired.Status = note.Status;
                    desired.Contact = note.Contact;
                    desired.PerformingInfo = note.PerformingInfo;
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
    }

    
}
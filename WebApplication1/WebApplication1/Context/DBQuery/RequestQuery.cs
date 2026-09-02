using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Controllers.RequestModel;

namespace WebApplication1.Context.DBQuery
{


    public interface IRequestQuery
    {
        Task<List<Request>> SelectAllRequests(ServiceContext context, RequestRange range);
        Task<List<Request>> SelectReceived(ServiceContext context, RequestRange range);
        Task<List<Request>> SelectTakenOnWork(ServiceContext context, RequestRange range);
        Task<List<Request>> SelectRejected(ServiceContext context, RequestRange range);
        Task<List<Request>> SelectFinished(ServiceContext context, RequestRange range);
        Task<List<Request>> SelectCancelled(ServiceContext context, RequestRange range);


        Task AddRequest(ServiceContext context, Request note);

        Task<Request> UpdateRequest(ServiceContext context, int id);

        Task SaveRequest(ServiceContext context, Request note);
    }

    public class RequestQuery : IRequestQuery
    {

        public RequestQuery() { }

        public async Task<List<Request>> SelectAllRequests(ServiceContext context, RequestRange range)
        {
            return await context.Requests.
                Where(r => (r.RequestIn <= range.End)&& (r.RequestIn >= range.Start)).
                ToListAsync();
        }

        public async Task<List<Request>> SelectReceived(ServiceContext context, RequestRange range)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "received")).
                Where(r => (r.RequestIn <= range.End) && (r.RequestIn >= range.Start)).
                ToListAsync();
        }

        public async Task<List<Request>> SelectTakenOnWork(ServiceContext context, RequestRange range)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "taken")).
                Where(r => (r.RequestIn <= range.End) && (r.RequestIn >= range.Start)).
                ToListAsync();
        }


        public async Task<List<Request>> SelectRejected(ServiceContext context, RequestRange range)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "rejected")).
                Where(r => (r.RequestIn <= range.End) && (r.RequestIn >= range.Start)).
                ToListAsync();
        }


        public async Task<List<Request>> SelectFinished(ServiceContext context, RequestRange range)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "finished")).
                Where(r => (r.RequestIn <= range.End) && (r.RequestIn >= range.Start)).
                ToListAsync();
        }

        public async Task<List<Request>> SelectCancelled(ServiceContext context, RequestRange range)
        {
            return await context.Requests.Where(r => EF.Functions.Like(r.Status, "cancelled")).
                Where(r => (r.RequestIn <= range.End) && (r.RequestIn >= range.Start)).
                ToListAsync();
        }


        public async Task AddRequest(ServiceContext context, Request note)
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

        public async Task<Request> UpdateRequest(ServiceContext context, int id) 
        {
            return await context.Requests.Where(r => r.ID == id).SingleAsync();
        }


        public async Task SaveRequest(ServiceContext context, Request note) 
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
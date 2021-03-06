using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vtc_Freelancer.Models;

namespace Vtc_Freelancer.Services
{
    public class RequestService
    {
        private MyDbContext dbContext;
        public RequestService(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool CreateRequest(int? UserId, string inputRequest, string category, string SubCategory, string inputDeliveredTime, double inputBudget, string urlFile)
        {
            try
            {
                Request req = new Request();
                req.Description = inputRequest;
                req.DeliveredTime = inputDeliveredTime;
                req.Budget = inputBudget;
                req.Category = category;
                req.SubCategory = SubCategory;
                req.LinkFile = urlFile;
                req.TimeCreate = DateTime.Now;
                req.QuantityOffers = 0;
                req.Status = 0;
                req.UserId = UserId;
                dbContext.Add(req);
                dbContext.SaveChanges();
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return false;
            }
        }
        public Request getRequestByRequestId(int? requestId)
        {
            return dbContext.Request.FirstOrDefault(x => x.RequestId == requestId);
        }
        public List<Request> getListRequestByUserId(int userId)
        {
            return dbContext.Request.Where(x => x.UserId == userId).ToList();
        }
        public List<Request> getListRequestByCategory(List<Category> listCategory)
        {
            if (listCategory.Count > 0)
            {
                List<Request> listRequest = new List<Request>();
                foreach (var item in listCategory)
                {
                    List<Request> SubListRequest = dbContext.Request.Where(x => x.Category == item.CategoryName).ToList();
                    listRequest.AddRange(SubListRequest);
                }
                return listRequest;
            }
            return null;
        }
        public List<Offer> GetOffersByRequestId(int? requestId)
        {
            List<Offer> offers = new List<Offer>();
            offers = dbContext.Offer.Where(x => x.RequestId == requestId).ToList();
            return offers;
        }
        // public  List<Service> GetServiceByRequestId(int? requestId)
        // {

        //     List<Service> services = new List<Service>();
        //     services = dbContext.
        // }
        public List<Request> GetRequestByUserId(int? userId)
        {
            List<Request> requests = new List<Request>();
            requests = dbContext.Request.Where(x => x.UserId == userId).ToList();
            return requests;
        }
    }
}
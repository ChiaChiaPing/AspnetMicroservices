using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public interface IOrderingService
    {

        public Task<IEnumerable<OrderResponseModel>> GetOrdersWithUserName(string userName);
    }
}

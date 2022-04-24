using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IOrderingService
    {
        public Task<IEnumerable<OrderResponseModel>> GetOrdersWithUserName(string userName);
    }
}

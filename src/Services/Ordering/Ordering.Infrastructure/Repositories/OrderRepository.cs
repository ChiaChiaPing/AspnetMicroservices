using System;
using Ordering.Domain.Entities;
using Ordering.Application.Contracts.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ordering.Infrastructure.Persistence;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Infrastructure.Repositories
{
    // real implementation
    public class OrderRepository: RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext):
            base(orderContext)
        {
          
        }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
           return await _dbContext.Orders
                                    .Where(c => c.UserName == userName)
                                    .ToListAsync();
        }
    }
}

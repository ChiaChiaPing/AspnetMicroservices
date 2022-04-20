using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence
{
    // like specified interface
    // the interface can inherit other interface without defining the method that withint the parent interface
    // the child interface can add on its own method.
    // but if some class implement the child interface, need to implement the method defined in the parent and child interface
    public interface IOrderRepository: IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
    }
}

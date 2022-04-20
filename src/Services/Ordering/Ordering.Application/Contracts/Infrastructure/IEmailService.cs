using System;
using System.Threading.Tasks;
using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastructure
{

    // the contract to the communication with external Infrastructure.
    public interface IEmailService
    {

        Task<bool> SentEmail(Email email);

       
    }
}

using System;
namespace Ordering.Application.Exceptions
{
    public class NotFoundException: ApplicationException
    {
        public NotFoundException(string name, object key)
            :base($"THe Entity \"{name}\" {key} was not found")
        {

        }
    }
}

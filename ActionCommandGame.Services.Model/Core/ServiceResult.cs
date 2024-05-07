using System.Collections.Generic;
using System.Linq;

namespace ActionCommandGame.Services.Model.Core
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            Messages = new List<ServiceMessage>();
        }

        public IList<ServiceMessage> Messages { get; set; }

        public bool IsSuccessful
        {
            get
            {
                //No error messages means success!
                return Messages.All(m => m.MessagePriority != MessagePriority.Error);
            }
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult()
        {

        }
        public ServiceResult(T data)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }
}
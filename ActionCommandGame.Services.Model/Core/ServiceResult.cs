using System.Collections.Generic;
using System.Linq;
namespace ActionCommandGame.Services.Model.Core
{
    public class ServiceResult
    {
        public bool IsSuccessful => Messages.All(m => m.MessagePriority != MessagePriority.Error);

        public IList<ServiceMessage> Messages { get; set; } = new List<ServiceMessage>();
    }
}

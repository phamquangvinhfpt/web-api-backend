using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.Serilog
{
    public class DummyService(ILogger<DummyService> logger) : IDummyService
    {
        public void DoSomething()
        {
            logger.LogInformation("something is done");
            logger.LogCritical("oops");
            logger.LogDebug("nothing much");
            logger.LogInformation("Invoking {@Event} with ID as {@Id}", "SomeEvent", Guid.NewGuid());
        }
    }
}
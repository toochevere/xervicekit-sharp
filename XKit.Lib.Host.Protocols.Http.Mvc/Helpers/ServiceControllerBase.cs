using System;
using System.Threading.Tasks;
using XKit.Lib.Common.Fabric;
using Microsoft.AspNetCore.Mvc;
using XKit.Lib.Common.Host;
using System.IO;
using XKit.Lib.Common.Utility.Extensions;

namespace XKit.Lib.Host.Protocols.Http.Mvc.Helpers {

    [Produces("application/json")]
    [ApiController]
    public abstract class ServiceControllerBase : ControllerBase {

        protected IHostEnvironment LocalEnvironment { get; private set; }
        protected ServiceOperationContext Context { get; private set; }
        protected IServiceBase ServiceCore { get; private set; } 
        protected IServiceOperation Operation { get; private set; }

        protected ServiceControllerBase(
            IHostEnvironment localEnvironment,
            IServiceBase service
        ) {
            LocalEnvironment = localEnvironment ?? throw new ArgumentNullException(nameof(localEnvironment));
            ServiceCore = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected ServiceControllerBase(
            IHostEnvironment localEnvironment
        ) {
            LocalEnvironment = localEnvironment ?? throw new ArgumentNullException(nameof(localEnvironment));
        }

        protected void SetService(
            IServiceBase service
        ) {
            ServiceCore = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected async Task<ActionResult> RunServiceOperationAndHttpResponse(
            IServiceBase service
        ) {
            if (service == null) {
                return BadRequest("Service not found");
            }
            SetService(service);

            using var reader = new StreamReader(Request.Body);
            string content = await reader.ReadToEndAsync();
            ServiceCallRequest request = Json.From<ServiceCallRequest>(content);                        

            ServiceCallResult result = null;
            Exception operationException = null;
            try { 
                result = await service.ExecuteCall(request);
            } catch(Exception ex) { 
                operationException = ex; 
            }
            
            var serviceStatus = ServiceCore.GetServiceStatus();

            return Ok(result);
        }
    }
}
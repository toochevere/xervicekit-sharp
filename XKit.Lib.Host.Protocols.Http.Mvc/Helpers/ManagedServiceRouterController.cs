using System;
using System.Linq;
using System.Threading.Tasks;
using XKit.Lib.Common.Host;
using Microsoft.AspNetCore.Mvc;

namespace XKit.Lib.Host.Protocols.Http.Mvc.Helpers {

    [Route("managed")]
    public class ManagedServiceRouterController : ServiceControllerBase {
        private static IHostEnvironment hostEnvironment;

        public static void SetHostEnvironment(IHostEnvironment hostEnvironment) {
            ManagedServiceRouterController.hostEnvironment = hostEnvironment;
        }

        public ManagedServiceRouterController() : base(
            hostEnvironment
        ) { }

        [HttpPost("{collectionName}/{serviceName}/{serviceVersion}")]
        public async Task<IActionResult> RouteServiceRequest(
            [FromRoute] string collectionName,
            [FromRoute] string serviceName,
            [FromRoute] int serviceVersion
        ) {
            var svc = HostEnvironment.GetManagedServices(
                collectionName,
                serviceName,
                serviceVersion
            ).SingleOrDefault();

            return await RunServiceOperationAndHttpResponse(
                svc
            );
        }
    }
}

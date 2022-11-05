using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> logger;
        /// <summary>
        /// Inject the ILogger service through the ASP.Net Core dependency injection service
        /// Takes a controller of the specified type as a generic parameter
        /// Another:hunglm@clamsoft.vn
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;

        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {

                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you visited does not exist";
                    logger.LogWarning($"and the query string = " +
                $"{statusCodeResult.OriginalPath} and the query string = " +
                $"{statusCodeResult.OriginalQueryString}");
                    break;

            }

            return View("NotFound");

        }


        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            logger.LogError($"path:{exceptionHandlerPathFeature.Path},generated an error{exceptionHandlerPathFeature.Error}");
            return View("Error");

        }


    }
}

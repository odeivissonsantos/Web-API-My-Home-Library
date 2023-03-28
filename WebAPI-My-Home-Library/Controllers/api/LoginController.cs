using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebAPI_My_Home_Library.Controllers.api
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Logar()
        {
            bool is_action = false;
            string error = "";
            try
            {

                is_action = true;
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }

            return Json(new { error, is_action });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Demo.Interface;
using NetCore.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Demo.Interface.BussinessModel;
using System.Runtime.InteropServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebAPI
{
    [ApiExplorerSettings(GroupName = "Demo")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : APIController
    {

        #region Initialization
        private readonly IUserService userService;

        public DemoController(IUserService userService)
        {
            this.userService = userService;
        }

        #endregion Initialization


        [HttpPost]
        [Route("[action]")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] LoginReq req)
        {
            return Ok(this.userService.Login(req.Account, req.Password));
        }
    }
}


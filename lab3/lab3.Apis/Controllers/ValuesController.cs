using lab3.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace lab3.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<Employee> userManager;

        public ValuesController(UserManager<Employee> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("userinfo")]
        public async Task< ActionResult> GetUserInfo()
        {
            var user =await userManager.GetUserAsync(User);
            return Ok(new
            {

                Username = user?.UserName,
                Email= user?.Email,
                address= user?.address

            }) ;
 }

        [HttpGet]
        [Authorize(Policy ="adminsonly") ]
        [Route("Admin")]
        public ActionResult GetInfoForManager()
        {
            return Ok(new string[] { "value1 for admin only", "value2 for admin only" });
        }





        [HttpGet]
        [Authorize (Policy ="usersandadmins")]
        [Route("User")]
        public ActionResult getdataforUser()
        {

            return Ok (new string[] {"value1 for admin and user", "value2 for admin and user"});

        }



    }
}

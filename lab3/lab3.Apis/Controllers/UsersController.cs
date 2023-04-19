using lab3.BL.Dtos;
using lab3.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lab3.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<Employee> userManager;
        private readonly IConfiguration configuration;

        public UsersController(UserManager<Employee> userManager , IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }


        #region adminregister
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterDto registerDto)

        {
            var employee = new Employee
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                address = registerDto.Address,
            };

            var result = await userManager.CreateAsync(employee, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,employee.Id),
                 new Claim(ClaimTypes.Name, employee.UserName),
                 new Claim(ClaimTypes.Role,"Admin") 

        };
            await userManager.AddClaimsAsync(employee, claims);

            return Ok();


        }

        #endregion


        #region userregister

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)

        {
            var employee = new Employee
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                address = registerDto.Address,
            };

            var result = await userManager.CreateAsync(employee, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,employee.Id),
                 new Claim(ClaimTypes.Name, employee.UserName),
                 new Claim(ClaimTypes.Role,"User")

            };
            await userManager.AddClaimsAsync(employee, claims);

            return Ok();


        }

        #endregion

        #region login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDto>> Login (LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized();

            }
            var isauthenticated = await userManager.CheckPasswordAsync(user,loginDto.Password);
            
            if(!isauthenticated)
            {
                return Unauthorized();
            }

            var claims = await userManager.GetClaimsAsync(user);

            #region secretkey 
            var secretKeyString =configuration.GetValue<string>("SecretKey");
            var secretyKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString ?? string.Empty);
            var secretKey = new SymmetricSecurityKey(secretyKeyInBytes);
           #endregion

            #region create a combination of secretkey and algorithm
            var signingCredentials = new SigningCredentials(secretKey,
            SecurityAlgorithms.HmacSha256Signature);
            #endregion

            #region putting all together
            var expiryDate =DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(
                claims:claims,
                expires:expiryDate,
                signingCredentials:signingCredentials);
            #endregion

            #region convert token from object to string 
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenstring= tokenhandler.WriteToken(token);

            #endregion

            return new TokenDto(tokenstring, expiryDate);

        }


        #endregion





    }
}

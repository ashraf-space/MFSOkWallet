using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using OneMFS.SecurityApiServer.Filters;
using System.Reflection;
using MFS.SecurityService.Models.Utility;

namespace OneMFS.SecurityApiServer.Controllers
{
	//[ApiGuardAuth]
	[Produces("application/json")]
	[Route("api/[controller]")]

	public class SecurityController : Controller
	{
		public IApplicationUserService usersService;
		private IErrorLogService errorLogService;
		private JwtModel jwtModel = null;
		IMerchantUserService merchantUserService;
		private IDisbursementUserService _DisbursementUserService;
        public SecurityController(IMerchantUserService _merchantUserService,IErrorLogService _errorLogService, IApplicationUserService _usersService, JwtModel _jwtModel,
            IDisbursementUserService objDisbursementUserService)
		{
			usersService = _usersService;
			jwtModel = _jwtModel;
			errorLogService = _errorLogService;
			merchantUserService = _merchantUserService;
            _DisbursementUserService = objDisbursementUserService;
		}

		[HttpPost]
		[Route("Login")]
		public object Login([FromBody]LoginModel model)
		{
			try
			{
				AuthUserModel obj = usersService.login(model);
				if (obj.IsAuthenticated && obj.User.LogInStatus == "N")
				{
					obj.IsAuthenticated = false;
				}
				if (obj.IsAuthenticated && obj.User.Pstatus == "L")
				{
					obj.IsAuthenticated = false;
				}

				if (obj.IsAuthenticated)
				{
					obj.BearerToken = CreateJwtToken(obj);
					return StatusCode(StatusCodes.Status200OK, obj);
				}
				else
				{
					return StatusCode(StatusCodes.Status200OK, obj);
				}
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return StatusCode(StatusCodes.Status401Unauthorized); ;
			}

		}
		[HttpPost]
		[Route("ClientLogIn")]
		public object ClientLogIn([FromBody]LoginModel model)
		{
			try
			{
				AuthClientUser obj = merchantUserService.ClientLogIn(model);
				if (obj.IsAuthenticated && obj.User.LogInStatus == "N")
				{
					obj.IsAuthenticated = false;
				}
				if (obj.IsAuthenticated && obj.User.Pstatus == "L")
				{
					obj.IsAuthenticated = false;
				}

				if (obj.IsAuthenticated)
				{
					//obj.BearerToken = CreateJwtTokenForClient(obj);
					//return StatusCode(StatusCodes.Status200OK, obj);		
					return obj;
				}
				else
				{
					return StatusCode(StatusCodes.Status200OK, obj);
				}
			}
			catch (Exception ex)
			{
				errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
				return StatusCode(StatusCodes.Status401Unauthorized); ;
			}
		}

        [HttpPost]
        [Route("DisbursementUserLogIn")]
        public object DisbursementUserLogIn([FromBody]LoginModel model)
        {
            try
            {
                AuthDisbursementUser obj = _DisbursementUserService.DisbursementUserLogIn(model);
                if (obj.IsAuthenticated && obj.User.LogInStatus == "N")
                {
                    obj.IsAuthenticated = false;
                }
                if (obj.IsAuthenticated && obj.User.Pstatus == "L")
                {
                    obj.IsAuthenticated = false;
                }

                if (obj.IsAuthenticated)
                {
                    //obj.BearerToken = CreateJwtTokenForClient(obj);
                    //return StatusCode(StatusCodes.Status200OK, obj);		
                    return obj;
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, obj);
                }
            }
            catch (Exception ex)
            {
                errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
                return StatusCode(StatusCodes.Status401Unauthorized); ;
            }
        }

        [HttpPost]
		[Route("LockAccount")]
		public object LockAccount([FromBody]ApplicationUser model)
		{
			try
			{
				model.Pstatus = "L";
				this.usersService.Update(model);
				return model;
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}

		}

		private string CreateJwtToken(AuthUserModel authModel)
		{
			try
			{
				JwtSettings settings = new JwtSettings();
				jwtModel = settings.Initiate();

				SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Key));

				List<Claim> jwtClaims = new List<Claim>();

				jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, authModel.User.Username.ToString()));
				jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

				jwtClaims.Add(new Claim("IsAuthenticated", authModel.IsAuthenticated.ToString().ToLower()));

				var token = new JwtSecurityToken(jwtModel.Issuer, jwtModel.Audience, jwtClaims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(jwtModel.MinutesToExpiration), new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

				return new JwtSecurityTokenHandler().WriteToken(token);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
			}

		}
		private string CreateJwtTokenForClient(AuthClientUser authModel)
		{
			try
			{
				JwtSettings settings = new JwtSettings();
				jwtModel = settings.Initiate();

				SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Key));

				List<Claim> jwtClaims = new List<Claim>();

				jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, authModel.User.MobileNo.ToString()));
				jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

				jwtClaims.Add(new Claim("IsAuthenticated", authModel.IsAuthenticated.ToString().ToLower()));

				var token = new JwtSecurityToken(jwtModel.Issuer, jwtModel.Audience, jwtClaims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(jwtModel.MinutesToExpiration), new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

				return new JwtSecurityTokenHandler().WriteToken(token);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
			}

		}
	}
}
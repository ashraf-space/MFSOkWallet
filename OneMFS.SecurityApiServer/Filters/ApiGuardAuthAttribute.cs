using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.SecurityApiServer.Filters
{
	
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ApiGuardAuthAttribute : Attribute, IAsyncActionFilter
	{
		private const string ApiKeyHeaderName = "ApiKey";
		private ICommonSecurityService applicationUserService;

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.HttpContext.RequestServices.GetRequiredService<ICommonSecurityService>() != null)
			{
				applicationUserService = context.HttpContext.RequestServices.GetRequiredService<ICommonSecurityService>();
			}


			if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
			{
				context.Result = new UnauthorizedResult();
				return;
			}
			if (context.HttpContext.Request.Headers.TryGetValue("UserInfo", out var potentialUserInfo))
			{
				var infos = potentialUserInfo.ToString();
				var userInfos = infos.Split(",").ToList();

				if (applicationUserService != null && !applicationUserService.IsProceedToController(userInfos))
				{
					context.Result = new UnauthorizedResult();
					applicationUserService = null;
					return;
				}
				applicationUserService = null;
			}


			var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
			var apiKey = configuration.GetValue<string>("ApiKey");

			if (!apiKey.Equals(potentialApiKey))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			await next();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
    [Route("api/enterprise")]
    public class EnterpriseController : Controller
    {
	    private IEnterpriseService enterpriseService;

	    public EnterpriseController(IEnterpriseService enterpriseService)
	    {
			this.enterpriseService = enterpriseService;
	    }
		[ApiGuardAuth]
		[HttpPost]
		[Route("Save")]
	    public object Save(bool isEdit, string evnt,[FromBody] Reginfo aReginfo)
		{
			return enterpriseService.Save(aReginfo,isEdit, evnt);

		}
		[HttpGet]
		[Route("getCustomerByMphone")]
	    public object GetCustomerByMphone(string mPhone)
		{
			return enterpriseService.GetCustomerByMphone(mPhone);

		}
	}
}
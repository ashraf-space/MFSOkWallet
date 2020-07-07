using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
	public interface ICommonSecurityService : IBaseService<ApplicationUser>
	{
		bool IsProceedToController(List<string> userInfos);
	}
	public class CommonSecurityService : BaseService<ApplicationUser>, ICommonSecurityService
	{
		public ICommonSecurityRepository securityRepo;
		
		public CommonSecurityService(ICommonSecurityRepository _securityRepo)
		{
			securityRepo = _securityRepo;
			
		}


		public bool IsProceedToController(List<string> userInfos)
		{
			var userId = userInfos[0];
			var roleId = userInfos[1];
			if(securityRepo.IsProceedToController(userInfos) != null)
			{
				var userInfo = (Tuple<string, string>)securityRepo.IsProceedToController(userInfos);
				if (userInfo.Item1 != roleId || userInfo.Item2 == "N")
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
			
		}
	}
}

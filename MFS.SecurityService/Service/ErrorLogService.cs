using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
	public interface IErrorLogService : IBaseService<Errorlog>
	{
		object InsertToErrorLog(Exception exception, string functionName, string userInfo);
		object GetErrorByFiltering(DateRangeModel date, string user);
	}
	public class ErrorLogService : BaseService<Errorlog>, IErrorLogService
	{
		private IErrorLogRepository errorLogRepository;
		public ErrorLogService(IErrorLogRepository _errorLogRepository)
		{
			this.errorLogRepository = _errorLogRepository;
		}

		public object GetErrorByFiltering(DateRangeModel date, string user)
		{
			return errorLogRepository.GetErrorByFiltering(date, user);
		}

		public object InsertToErrorLog(Exception exception, string functionName, string userInfo)
		{
			try
			{
				if (userInfo != null)
				{
					string[] userInfos = userInfo.Split(',');
					Errorlog errorLog = new Errorlog
					{
						ErrorCode = "100",
						Message = exception.Message.ToString(),
						ErrorDate = DateTime.Now,
						FunctionName = functionName,
						UserId = userInfos[0],
						RoleId = userInfos[1]
					};
					errorLogRepository.Add(errorLog);
					return null;
				}
				else
				{
					Errorlog errorLog = new Errorlog
					{
						ErrorCode = "100",
						Message = exception.Message.ToString(),
						ErrorDate = DateTime.Now,
						FunctionName = functionName
					};
					errorLogRepository.Add(errorLog);
					return null;
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}

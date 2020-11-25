using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Service
{
	public interface IEmsService:IBaseService<EmsReport>
	{
		List<EmsReport> GetEmsReport(string fromDate, string toDate, string transNo, string studentId, string schoolId);
	}
	public class EmsService : BaseService<EmsReport>, IEmsService
	{
		private readonly IEmsRepository emsRepository;
		public EmsService(IEmsRepository _emsRepository)
		{
			this.emsRepository = _emsRepository;
		}
		public List<EmsReport> GetEmsReport(string fromDate, string toDate, string transNo, string studentId, string schoolId)
		{
			return emsRepository.GetEmsReport(fromDate, toDate, transNo, studentId, schoolId);
		}
	}
}

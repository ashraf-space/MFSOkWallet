using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFS.ReportingService.Models;
using MFS.ReportingService.Repository;

namespace MFS.ReportingService.Service
{
	public interface ILankaBanglaService
	{
		List<LankaBangla> GetDpsDetailsInfo(string fromDate, string toDate);
	}
	public class LankaBanglaService : ILankaBanglaService
	{
		private readonly ILankaBanglaRepository repository;
		public LankaBanglaService(ILankaBanglaRepository lankaBanglaRepository)
		{
			this.repository = lankaBanglaRepository;
		}
		public List<LankaBangla> GetDpsDetailsInfo(string fromDate, string toDate)
		{
			return repository.GetDpsDetailsInfo(fromDate, toDate);
		}
	}
}

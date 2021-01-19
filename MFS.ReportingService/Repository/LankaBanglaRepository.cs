using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Repository
{
	public interface ILankaBanglaRepository
	{
		List<LankaBangla> GetDpsDetailsInfo(string fromDate, string toDate);
	}
	public class LankaBanglaRepository : BaseRepository<LankaBangla>, ILankaBanglaRepository
	{
		private readonly string dbUser;
		public LankaBanglaRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public List<LankaBangla> GetDpsDetailsInfo(string fromDate, string toDate)
		{
			using (var connection = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();

				dyParam.Add("f_date", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
				dyParam.Add("t_date", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));				
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

				List<LankaBangla> result = SqlMapper.Query<LankaBangla>(connection, dbUser + "RPT_LB_DPS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				return result;
			}
		}
	}
}

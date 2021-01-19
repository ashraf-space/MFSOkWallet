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
	public interface IEmsRepository : IBaseRepository<EmsReport>
	{
		List<EmsReport> GetEmsReport(string fromDate, string toDate, string transNo, string studentId, string schoolId, string branchCode);
	}
	public class EmsRepository : BaseRepository<EmsReport>, IEmsRepository
	{
		private readonly string dbUser;
		public EmsRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public List<EmsReport> GetEmsReport(string fromDate, string toDate, string transNo, string studentId, string schoolId, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo=="null"?null:transNo);
					dyParam.Add("V_STUDENTID", OracleDbType.Varchar2, ParameterDirection.Input, studentId=="null"?null:studentId);
					dyParam.Add("V_SCHOOLID", OracleDbType.Varchar2, ParameterDirection.Input, schoolId=="null"?null:schoolId);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<EmsReport> result = SqlMapper.Query<EmsReport>(connection, dbUser + "RPT_EMS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}

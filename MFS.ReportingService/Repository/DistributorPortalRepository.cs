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
	public interface IDistributorPortalRepository : IBaseRepository<DistributorPortal>
	{
		List<AgentDsrList> GetAgentDsrListByPmphone(string mphone);
	}
	public class DistributorPortalRepository : BaseRepository<DistributorPortal>, IDistributorPortalRepository
	{
		private readonly string dbUser;
		public DistributorPortalRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public List<AgentDsrList> GetAgentDsrListByPmphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input,mphone);					
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<AgentDsrList> result = SqlMapper.Query<AgentDsrList>(connection, dbUser + "RPT_DISTPORT_AGDSRLIST", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

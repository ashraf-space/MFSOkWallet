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
		object GetBalanceInformation(string mphone, string filterId);
		object GetDistPortalInfo(string mphone);
		List<CustomerRegDistPort> GetCustomerListByAgent(string mphone, string fromDate, string toDate, string agentNo);
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
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
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

		public object GetBalanceInformation(string mphone, string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = string.Empty;
					if (filterId != "All")
					{
						query = @"select t.mphone,nvl(t.company_name,t.name) as name,t.pmphone,one.func_get_balance(t.mphone,'M') as ""BALANCE"" from one.reginfo t where t.pmphone = '" + mphone + "' and t.cat_id='" + filterId + "'";

					}
					else
					{
						query = @"select t.mphone,nvl(t.company_name,t.name) as name,t.pmphone,one.func_get_balance(t.mphone,'M') as ""BALANCE"" from one.reginfo t where t.pmphone = '" + mphone + "'";
					}

					var result = connection.Query<dynamic>(query).ToList();
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<CustomerRegDistPort> GetCustomerListByAgent(string mphone, string fromDate, string toDate, string agentNo)
		{
			using (var connection = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();

				dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
				dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
				dyParam.Add("V_AGENT_NO", OracleDbType.Varchar2, ParameterDirection.Input, agentNo == "" ? null : agentNo);
				dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

				List<CustomerRegDistPort> result = SqlMapper.Query<CustomerRegDistPort>(connection, dbUser + "RPT_DPORT_CUSTBYAGENT", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				return result;
			}
		}

		public object GetDistPortalInfo(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					DistPortalInfo result = SqlMapper.Query<DistPortalInfo>(connection, dbUser + "SP_GET_DST_PORT_INFO", param: dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

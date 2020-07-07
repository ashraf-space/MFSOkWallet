using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.DistributionService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;

namespace MFS.DistributionService.Repository
{
	public interface IAgentRepository : IBaseRepository<Reginfo>
	{
		IEnumerable<Reginfo> GetAllAgents();
		object GetclusterByTerritoryCode(string code);
		string GetClusterCodeByTerritoryCode(string code);
		object GenerateAgentCode(string code);

		object GetAgentByMobilePhone(string mPhone);
		object GetAgentListByClusterCode(string cluster);

		object GetAgentListByParent(string code, string catId);
		object GetDistCodeByAgentInfo(string territoryCode, string companyName, string offAddr);
		string GenerateAgentCodeAsString(string code);
	}
	public class AgentRepository : BaseRepository<Reginfo>, IAgentRepository
	{
		
		public IEnumerable<Reginfo> GetAllAgents()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, "PR_GET_AGENTS_GRID_LIST", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);
					return result;
				}
					
			}
			catch (Exception ex)
			{				
				throw ex;
			}

		}

		public object GetclusterByTerritoryCode(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("TERRITORY_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GET_CLUSTERS_BY_TERRITORY", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);
					return result;
				}
					
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public string GetClusterCodeByTerritoryCode(string code)
		{

			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @" SELECT code FROM location WHERE PARENT = " + "'" + code + "'" + "";

					var result = connection.Query<string>(query).FirstOrDefault();

					this.CloseConnection(connection);
					return result;
				}
					
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GenerateAgentCode(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CLUSTER_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("AGENT_CODE_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_GENERATE_AGENT_CODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}
					
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object GetAgentByMobilePhone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, "SP_GET_AGENT_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}				

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object GetAgentListByClusterCode(string cluster)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select * from REGINFOVIEW where catid = 'A' and distCode like '%" + cluster + "%'";

					var result = connection.Query<Reginfo>(query);

					this.CloseConnection(connection);
					return result;
				}					
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object GetAgentListByParent(string code, string catId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select * from REGINFOVIEW where catid = '" + catId + "' and pmphone='" + code + "' ";

					var result = connection.Query<Reginfo>(query);

					this.CloseConnection(connection);
					return result;
				}					
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object GetDistCodeByAgentInfo(string territoryCode, string companyName, string offAddr)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("TERRITORY_CODE", OracleDbType.Varchar2, ParameterDirection.Input, territoryCode);
					parameter.Add("V_COMPANY_NAME", OracleDbType.Varchar2, ParameterDirection.Input, companyName);
					parameter.Add("V_OFF_ADDR", OracleDbType.Varchar2, ParameterDirection.Input, offAddr);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_GET_DIST_CODE_BY_AGENTINFO", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}
					
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public string GenerateAgentCodeAsString(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CLUSTER_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("AGENT_CODE_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<string>(connection, "SP_GENERATE_AGENT_CODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

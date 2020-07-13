using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.DistributionService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;

namespace MFS.DistributionService.Repository
{
	public interface IEnterpriseRepository : IBaseRepository<Reginfo>
	{
		object GetCustomerGridList();
		object GetCustomerByMphone(string mPhone);
	}
	public class EnterpriseRepository : BaseRepository<Reginfo>, IEnterpriseRepository
	{
		private readonly string dbUser;
		public EnterpriseRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object GetCustomerGridList()
		{			
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser+"PR_GET_CUSTOMER_GRID_LIST", param: parameter, commandType: CommandType.StoredProcedure);
					connection.Close();
					return result;
				}
					
			}
			catch (Exception e)
			{
				return e.ToString();
			}
			
		}

		public object GetCustomerByMphone(string mPhone)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
				parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_GET_CUSTOMER_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
				this.CloseConnection(connection);
				return result;
			}
				
		}
	}
}

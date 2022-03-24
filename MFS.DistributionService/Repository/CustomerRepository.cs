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
	public interface ICustomerRepository : IBaseRepository<Reginfo>
	{
		object GetCustomerGridList();
		object GetCustomerByMphone(string mPhone);
		bool IsPhotoIdExist(string catId, string photoId, int code);
	}
	public class CustomerRepository:BaseRepository<Reginfo>,ICustomerRepository
	{
		private readonly string dbUser;
		public CustomerRepository(MainDbUser objMainDbUser)
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
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}
					
			}
			catch (Exception e)
			{				
				throw;
			}
			
		}

		public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_GET_CUSTOMER_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}
					
			}
			catch (Exception e)
			{				
				throw;
			}

		}

		public bool IsPhotoIdExist(string catId, string photoId, int code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select count(*) as ""total"" from " + dbUser + "reginfo t where t.photo_id = '" + photoId + "' and t.cat_id = '" + catId + "' and t.status <> 'C'";

					var result = connection.Query<int>(query).FirstOrDefault();
					this.CloseConnection(connection);
					connection.Dispose();
					if (Convert.ToUInt32(result) == 0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}

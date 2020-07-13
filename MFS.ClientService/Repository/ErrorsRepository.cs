using Dapper;
using MFS.ClientService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.ClientService.Repository
{
	public interface IErrorsRepository : IBaseRepository<Errors>
	{
		object GetErrorLog();
	}

	public class ErrorsRepository : BaseRepository<Errors>, IErrorsRepository
	{
		private readonly string dbUser;
		public ErrorsRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object GetErrorLog()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();					
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, dbUser+"SP_GET_ERRORLOG", param: parameter, commandType: CommandType.StoredProcedure);
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
	}
}

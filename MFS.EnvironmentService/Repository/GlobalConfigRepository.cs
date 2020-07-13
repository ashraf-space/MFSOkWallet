using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.EnvironmentService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;

namespace MFS.EnvironmentService.Repository
{
	public interface IGlobalConfigRepository : IBaseRepository<GlobalConfig>
	{			
		object GetGlobalConfigs();
	} 
	public class GlobalConfigRepository : BaseRepository<GlobalConfig>, IGlobalConfigRepository
	{
		private readonly string dbUser;
		public GlobalConfigRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}

		public object GetGlobalConfigs()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_GLOBAL_CONFIG", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, dbUser+"SP_GET_GLOBAL_CONFIG", param: parameter, commandType: CommandType.StoredProcedure);
					return result;
				}
					
			}
			catch (Exception e)
			{				
				throw;
			}
		}
	}
}

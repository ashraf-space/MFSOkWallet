using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.EnvironmentService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;

namespace MFS.EnvironmentService.Repository
{
	public interface IDistrictThanaRepository : IBaseRepository<Disthana>
	{
		object GetRegionDropdownList();
	}
	public class DistrictThanaRepository : BaseRepository<Disthana>, IDistrictThanaRepository
	{
		public object GetRegionDropdownList()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("CUR_REGION", OracleDbType.RefCursor, ParameterDirection.Output);
				parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
				var result = SqlMapper.Query<Location>(connection, "PKG_ENVIORONMENT.PR_GET_REGION", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}							
		}
	}
}

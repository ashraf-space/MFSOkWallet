using Dapper;
using MFS.SecurityService.Models;
using MFS.SecurityService.Models.Utility;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        IEnumerable<PermissionViewModel> GetPermissionWorklist(int roleId);
    }

    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public IEnumerable<PermissionViewModel> GetPermissionWorklist(int roleId)
        {
			using (var conn = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();
				dyParam.Add("FEATURE_LIST", OracleDbType.RefCursor, ParameterDirection.Output);
				dyParam.Add("ROLEID", OracleDbType.Int32, ParameterDirection.Input, roleId);

				var result = SqlMapper.Query<PermissionViewModel>(conn, "PR_MFS_GETPERMISSIONWORKLIST", param: dyParam, commandType: CommandType.StoredProcedure);
				this.CloseConnection(conn);
				return result;
			}
				
        }
    }
}

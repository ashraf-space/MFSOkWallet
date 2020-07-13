using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace MFS.SecurityService.Repository
{
    public interface ICommonSecurityRepository : IBaseRepository<ApplicationUser>
    {
        object IsProceedToController(List<string> userInfos);
    }
    public class CommonSecurityRepository : BaseRepository<ApplicationUser>, ICommonSecurityRepository
    {
        //private static string dbuser;
        //public CommonSecurityRepository(MainDbUser objMainDbUser)
        //{
        //    dbuser = objMainDbUser.DbUser;
        //}
        MainDbUser mainDbUser = new MainDbUser();
        public object IsProceedToController(List<string> userInfos)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("UID", OracleDbType.Varchar2, ParameterDirection.Input, userInfos[0]);
                    dyParam.Add("ROLE_ID", OracleDbType.Int32, ParameterDirection.Output, null, 32767);
                    dyParam.Add("LG_STATUS", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    SqlMapper.Query(conn, mainDbUser.DbUser + "PR_PROCEED_LOGIN", param: dyParam, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    var roleId = dyParam.oracleParameters[1].Value.ToString();
                    var fg = dyParam.oracleParameters[2].Value.ToString();
                    if (roleId != null && fg != null)
                    {
                        return Tuple.Create(roleId, fg);
                    }
                    else
                    {
                        return null;
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

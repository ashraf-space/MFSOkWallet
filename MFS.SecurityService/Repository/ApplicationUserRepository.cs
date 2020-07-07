using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
    {
        ApplicationUser validateLogin(string userName, string password);
        string GetTransAmtLimit(string createUser);
		object IsProceedToController(List<string> userInfos);
		object GetAppUserListDdl();
	}

    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
             
        public ApplicationUser validateLogin(string userName, string password)
        {
            try
            {
				using (var conn = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("UNAME", OracleDbType.Varchar2, ParameterDirection.Input, userName);
					dyParam.Add("PWD", OracleDbType.Varchar2, ParameterDirection.Input, password);
					dyParam.Add("LOGIN_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

					IList<ApplicationUser> result = SqlMapper.Query<ApplicationUser>(conn, "PR_MFS_VALIDATELOGIN", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(conn);

					if (result.Count == 0)
					{
						ApplicationUser obj = conn.QueryFirstOrDefault<ApplicationUser>("Select " + this.GetCamelCaseColumnList(new ApplicationUser()) + " from Application_User where username='" + userName + "'");
						obj.Is_validated = false;
						return obj;
					}
					else
					{
						result[0].Is_validated = true;
						return result[0];
					}
				}					
            }
            catch (Exception e)
            {
                return new ApplicationUser() { Is_validated = false };
            }
            
        }

        public string GetTransAmtLimit(string createUser)
        {
            try
            {
				using (var conn = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("createUser", OracleDbType.Varchar2, ParameterDirection.Input, createUser);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<string>(conn, "SP_Get_TransAmtLimitByUser", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					conn.Close();
					return result;
				}
					
            }
            catch (Exception)
            {
                throw;
            }

        }

		public object IsProceedToController(List<string> userInfos)
		{
			try
			{
				using (var conn = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_WHO", OracleDbType.Varchar2, ParameterDirection.Input, userInfos[0]);
					dyParam.Add("ROLE_ID", OracleDbType.Int32, ParameterDirection.Output, null, 32767);
					dyParam.Add("FORCE_LG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
					SqlMapper.Query(conn, "PR_PROCEED_LOGIN", param: dyParam, commandType: CommandType.StoredProcedure);
					conn.Close();
					var roleId = dyParam.oracleParameters[1].Value.ToString();
					var fg = dyParam.oracleParameters[2].Value.ToString();
					return Tuple.Create(roleId, fg);
				}
					
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		public object GetAppUserListDdl()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.username as ""label"", t.id ""value"" from application_user t";
					var result = connection.Query<CustomDropDownModel>(query).ToList();
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}

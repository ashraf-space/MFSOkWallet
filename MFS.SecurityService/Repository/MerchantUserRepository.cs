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
    public interface IMerchantUserRepository : IBaseRepository<MerchantUser>
    {
        MerchantUser validateLogin(string userName, string password);
		object GetRegInfoByMphone(string mobileNo);
		object GetMerChantUserById(string id);
		object CheckMerchantUserAlreadyExist(string username);
		
	}

    public class MerchantUserRepository : BaseRepository<MerchantUser>, IMerchantUserRepository
    {
        private readonly string dbUser;
        public MerchantUserRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }

		public object CheckMerchantUserAlreadyExist(string username)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.* from " + dbUser + "merchant_user t where LOWER(t.username) = '" + username.ToLower()+"'";
					var result = connection.Query<MerchantUser>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetMerChantUserById(string id)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.* from " + dbUser + "merchant_user t where t.id = " + Convert.ToInt32(id) + "";
					var result = connection.Query<MerchantUser>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public dynamic GetRegInfoByMphone(string mobileNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select * from " + dbUser + "RegInfoView where mphone= '" + mobileNo + "' ";

					var result = connection.Query<dynamic>(query).FirstOrDefault();

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

		public MerchantUser validateLogin(string userName, string password)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("UACC", OracleDbType.Varchar2, ParameterDirection.Input, userName.Trim());
					//dyParam.Add("UNAME", OracleDbType.Varchar2, ParameterDirection.Input, userName);
					dyParam.Add("PWD", OracleDbType.Varchar2, ParameterDirection.Input, password);
                    dyParam.Add("LOGIN_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

                    IList<MerchantUser> result = SqlMapper.Query<MerchantUser>(conn, dbUser + "PR_VALIDATELOGIN_CLIENT", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(conn);

                    if (result.Count == 0)
                    {
                        MerchantUser obj = conn.QueryFirstOrDefault<MerchantUser>("Select " + this.GetCamelCaseColumnList(new MerchantUser()) + " from " + dbUser + "MERCHANT_USER where mobile_no='" + userName + "'");
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
                return new MerchantUser() { Is_validated = false };
            }

        }

        //public string GetTransAmtLimit(string createUser)
        //{
        //    try
        //    {
        //        using (var conn = this.GetConnection())
        //        {
        //            var parameter = new OracleDynamicParameters();
        //            parameter.Add("createUser", OracleDbType.Varchar2, ParameterDirection.Input, createUser);
        //            parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
        //            var result = SqlMapper.Query<string>(conn, dbUser + "SP_Get_TransAmtLimitByUser", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //            conn.Close();
        //            return result;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //public object IsProceedToController(List<string> userInfos)
        //{
        //    try
        //    {
        //        using (var conn = this.GetConnection())
        //        {
        //            var dyParam = new OracleDynamicParameters();
        //            dyParam.Add("V_WHO", OracleDbType.Varchar2, ParameterDirection.Input, userInfos[0]);
        //            dyParam.Add("ROLE_ID", OracleDbType.Int32, ParameterDirection.Output, null, 32767);
        //            dyParam.Add("FORCE_LG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
        //            SqlMapper.Query(conn, dbUser + "PR_PROCEED_LOGIN", param: dyParam, commandType: CommandType.StoredProcedure);
        //            conn.Close();
        //            var roleId = dyParam.oracleParameters[1].Value.ToString();
        //            var fg = dyParam.oracleParameters[2].Value.ToString();
        //            return Tuple.Create(roleId, fg);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public object GetAppUserListDdl()
        //{
        //    try
        //    {
        //        using (var connection = this.GetConnection())
        //        {
        //            string query = @"select t.username as ""label"", t.id ""value"" from " + dbUser + "application_user t";
        //            var result = connection.Query<CustomDropDownModel>(query).ToList();
        //            this.CloseConnection(connection);
        //            connection.Dispose();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}

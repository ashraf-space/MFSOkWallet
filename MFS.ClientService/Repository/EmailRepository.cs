using Dapper;
using MFS.ClientService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.ClientService.Repository
{
    public interface IEmailRepository : IBaseRepository<Email>
    {

        object SendVeriCodeToEmail(Email objEmail);
        bool IsCheckExist(ForgotPassReset forgotPassResetModel);
    }

    public class EmailRepository : BaseRepository<Email>, IEmailRepository
    {
        private readonly string dbUser;
        public EmailRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }
        public object SendVeriCodeToEmail(Email objEmail)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input, objEmail.EMAIL);
                    parameter.Add("V_TEMPLETE", OracleDbType.Varchar2, ParameterDirection.Input, objEmail.TEMPLETE);
                    parameter.Add("V_DATA1", OracleDbType.Varchar2, ParameterDirection.Input, objEmail.DATA1);
                    parameter.Add("V_DATA2", OracleDbType.Varchar2, ParameterDirection.Input, objEmail.DATA2);
                    var result = SqlMapper.Query<dynamic>(connection, dbUser + "PROC_MAKE_EMAIL", param: parameter, commandType: CommandType.StoredProcedure);
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

        public bool IsCheckExist(ForgotPassReset forgotPassResetModel)
        {
            try
            {
                int result = 0;
                using (var connection = this.GetConnection())
                {
                    string query;

                    query = @"Select count(*) from " + dbUser + "application_user  where username = '" + forgotPassResetModel.UserName.Trim() + "' " +
                        "and Employee_Id = '" + forgotPassResetModel.EmployeeId.Trim() + "' " +
                        "and mobile_no = '" + forgotPassResetModel.MobileNo.Trim() + "' " +
                        "and email_id= '" + forgotPassResetModel.OfficialEmail.Trim() + "'";


                    //var result = connection.QueryFirstOrDefault<dynamic>(query);
                     result = connection.Query<int>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

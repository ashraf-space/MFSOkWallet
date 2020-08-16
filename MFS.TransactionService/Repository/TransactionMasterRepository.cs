using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;

namespace MFS.TransactionService.Repository
{
    public interface ITransactionMasterRepository : IBaseRepository<GlTransMst>
    {
        dynamic GetTransactionList(string mphone, DateTime fromDate, DateTime toDate);
        dynamic GetTransactionMasterByTransNo(string transactionNumber);
        dynamic GetBankDepositStatus(DateTime fromDate, DateTime toDate, string balanceType, string roleName);
        object approveOrRejectBankDepositStatus(string roleName, string userName, string evnt, List<TblBdStatus> objTblBdStatusList);
        object ExecuteEOD(DateTime todayDate, string userName);
        TblBdStatus GetBankDepositStatusByTransNo(string tranno);
    }
    public class TransactionMasterRepository : BaseRepository<GlTransMst>, ITransactionMasterRepository
    {
        MainDbUser mainDbUser = new MainDbUser();
        public dynamic GetTransactionList(string mphone, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("FROM_DATE", OracleDbType.Date, ParameterDirection.Input, fromDate);
                    dyParam.Add("UPTO_DATE", OracleDbType.Date, ParameterDirection.Input, toDate);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    var result = SqlMapper.Query<GlTransMst>(connection, mainDbUser.DbUser + "SP_GET_GLTRANSMST_BY_MPHONE", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

        public dynamic GetTransactionMasterByTransNo(string transactionNumber)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var result = connection.QueryFirstOrDefault<GlTransMst>("select * from "+ mainDbUser.DbUser + "gltransmstview where transNo = '" + transactionNumber + "'");
                    //this.CloseConnection(connection);
                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            //var result = conn.QueryFirstOrDefault<GlTransMst>("select * from gltransmstview where transNo = '" + transactionNumber + "'");
            //this.CloseConnection(conn);

            //return result;
        }

        public dynamic GetBankDepositStatus(DateTime fromDate, DateTime toDate, string balanceType, string roleName)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    List<TblBdStatus> result = new List<TblBdStatus>();

                    string fStatus = null;// filtering Status
                    if (balanceType == "Main Balance")
                    {
                        if (roleName == "SOM")
                        {
                            fStatus = "N";
                        }
                        else if (roleName == "Financial Maker")
                        {
                            fStatus = "M";
                        }
                        else
                        {
                            fStatus = "C";
                        }

                        var dyParam = new OracleDynamicParameters();

                        dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, fromDate);
                        dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, toDate);
                        dyParam.Add("balanceType", OracleDbType.Varchar2, ParameterDirection.Input, balanceType);
                        dyParam.Add("fStatus", OracleDbType.Varchar2, ParameterDirection.Input, fStatus);
                        dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                        result = SqlMapper.Query<TblBdStatus>(connection, mainDbUser.DbUser + "SP_GET_Bank_Deposit_Status", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                        this.CloseConnection(connection);
                    }




                    return result;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
           
        }

        public object approveOrRejectBankDepositStatus(string roleName, string userName, string evnt, List<TblBdStatus> objTblBdStatusList)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string successOrErrorMsg = null;

                    foreach (var item in objTblBdStatusList)
                    {
                        //cheking is already action performed?
                        string fStatus = null;
                        if (roleName == "SOM")
                        {
                            fStatus = "N";
                        }
                        else if (roleName == "Financial Maker")
                        {
                            fStatus = "M";
                        }
                        else
                        {
                            fStatus = "C";
                        }
                        string status = connection.QueryFirstOrDefault<string>("Select status from "+ mainDbUser.DbUser + "TBL_BD_STATUS where Tranno ='" + item.Tranno + "'");
                        if (fStatus != status)
                        {
                            return successOrErrorMsg = "MissMatch";
                        }
                    }

                    Boolean isFinCheckerApproval = false;
                    foreach (var item in objTblBdStatusList)
                    {

                        if (item.MakeStatus)
                        {
                            if (evnt == "reject")
                            {
                                item.Status = "R";
                            }
                            else if (evnt == "register")
                            {
                                if (roleName == "SOM")
                                {
                                    item.Status = "M";
                                }
                                else if (roleName == "Financial Maker")
                                {
                                    item.Status = "C";
                                }
                                else
                                {
                                    isFinCheckerApproval = true;
                                    item.Status = "Y";
                                }


                            }

                            string tranno = item.Tranno;
                            string phone = item.phone;
                            double tranAmt = item.TranAmt;
                            string status = item.Status;

                            var parameter = new OracleDynamicParameters();
                            parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(tranno));
                            parameter.Add("V_FR_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, phone);

                            if (isFinCheckerApproval)
                            {
                                parameter.Add("V_BALANCE_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "M");
                                parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, tranAmt);
                                parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                                parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                                parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                                parameter.Add("V_TO_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                                parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input);
                                parameter.Add("V_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, "C");
                                parameter.Add("CheckBy", OracleDbType.Varchar2, ParameterDirection.Input, userName);

                                //parameter.Add("V_RoleName", OracleDbType.Varchar2, ParameterDirection.Input, roleName);
                                parameter.Add("V_Status", OracleDbType.Varchar2, ParameterDirection.Input, item.Status);

                                SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Bank_Status_APPROVE", param: parameter, commandType: CommandType.StoredProcedure);

                                this.CloseConnection(connection);

                                string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                                if (flag == "0")
                                {
                                    successOrErrorMsg = parameter.oracleParameters[6].Value != null ? parameter.oracleParameters[6].Value.ToString() : null;
                                }
                                else
                                {
                                    successOrErrorMsg = flag;
                                }
                            }
                            else
                            {
                                parameter.Add("CheckBy", OracleDbType.Varchar2, ParameterDirection.Input, userName);
                                parameter.Add("V_RoleName", OracleDbType.Varchar2, ParameterDirection.Input, roleName);
                                parameter.Add("V_Status", OracleDbType.Varchar2, ParameterDirection.Input, status);
                                parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, tranAmt);

                                SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Update_Bank_Deposit_Status", param: parameter, commandType: CommandType.StoredProcedure);
                                successOrErrorMsg = "1";
                            }


                        }


                    }
                    return successOrErrorMsg;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }

        public object ExecuteEOD(DateTime todayDate, string userName)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("todayDate", OracleDbType.Date, ParameterDirection.Input, todayDate);
                    parameter.Add("CheckBy", OracleDbType.Varchar2, ParameterDirection.Input, userName);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_EOD_Process", param: parameter, commandType: CommandType.StoredProcedure);

                    //this.CloseConnection(conn);
                    connection.Close();

                    string successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;


                    return successOrErrorMsg;
                }

               
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }

        public TblBdStatus GetBankDepositStatusByTransNo(string tranno)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    return connection.QueryFirstOrDefault<TblBdStatus>("Select * from " + mainDbUser.DbUser + "TBL_BD_STATUS where Tranno ='" + tranno + "'");
                }
                    
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

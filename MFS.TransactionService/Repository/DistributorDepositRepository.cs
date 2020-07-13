using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface IDistributorDepositRepository : IBaseRepository<TblCashEntry>
    {
        object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        string GetTransactionNo();
        TblCashEntry GetDestributorDepositByTransNo(string transNo);
        object DataInsertToTransMSTandDTL(TblCashEntry cashEntry);
    }
    public class DistributorDepositRepository : BaseRepository<TblCashEntry>, IDistributorDepositRepository
    {

        MainDbUser mainDbUser = new MainDbUser();

        public object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    int intValue = isRegistrationPermitted == true ? 1 : 0;
                    parameter.Add("isRegistrationPermitted", OracleDbType.Int32, ParameterDirection.Input, intValue);
                    parameter.Add("transAmtLimit", OracleDbType.Double, ParameterDirection.Input, transAmtLimit);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<TblCashEntry>(connection, mainDbUser.DbUser + "SP_Get_CashEntry_ByBranchCode", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                //int intValue = isRegistrationPermitted == true ? 1 : 0;
                //parameter.Add("isRegistrationPermitted", OracleDbType.Int32, ParameterDirection.Input, intValue);
                //parameter.Add("transAmtLimit", OracleDbType.Double, ParameterDirection.Input, transAmtLimit);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<TblCashEntry>(connection, "SP_Get_CashEntry_ByBranchCode", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetTransactionNo()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<string>(connection, mainDbUser.DbUser + "SP_Get_TransactionNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<string>(connection, "SP_Get_TransactionNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //connection.Close();

                //return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TblCashEntry GetDestributorDepositByTransNo(string transNo)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("transNo", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<TblCashEntry>(connection, mainDbUser.DbUser+ "SP_Get_DepositByTransNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("transNo", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<TblCashEntry>(connection, "SP_Get_DepositByTransNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                //connection.Close();

                //return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object DataInsertToTransMSTandDTL(TblCashEntry cashEntry)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(cashEntry.TransNo));
                    parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.AcNo);
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, cashEntry.Amount);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.EntryBranchCode);
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.CheckedUser);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_INSERT_TBL_CASH_ENTRY", param: parameter, commandType: CommandType.StoredProcedure);


                    connection.Close();
                    string flag = parameter.oracleParameters[4].Value != null ? parameter.oracleParameters[4].Value.ToString() : null;
                    string successOrErrorMsg = null;
                    if (flag == "0")
                    {
                        successOrErrorMsg = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                    }
                    else
                    {
                        successOrErrorMsg = flag;
                    }
                    return successOrErrorMsg;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(cashEntry.TransNo));
                //parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.AcNo);
                //parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, cashEntry.Amount);
                //parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                //parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                //parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.EntryBranchCode);
                //parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, cashEntry.CheckedUser);

                //SqlMapper.Query<dynamic>(connection, "SP_INSERT_TBL_CASH_ENTRY", param: parameter, commandType: CommandType.StoredProcedure);


                //connection.Close();
                //string flag = parameter.oracleParameters[4].Value != null ? parameter.oracleParameters[4].Value.ToString() : null;
                //string successOrErrorMsg = null;
                //if (flag == "0")
                //{
                //    successOrErrorMsg = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                //}
                //else
                //{
                //    successOrErrorMsg = flag;
                //}
                //return successOrErrorMsg;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

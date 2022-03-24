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
    public interface ICommissionConversionRepository : IBaseRepository<TblCommissionConversion>
    {
        object GetCashEntryListByBranchCode(string branchCode, bool isRegistrationPermitted, double transAmtLimit);
        object GetCommissionConversionList(bool isRegistrationPermitted, double transAmtLimit);
        string GetTransactionNo();
        TblCommissionConversion GetCommissionConversionByTransNo(string transNo);
        object DataInsertToTransMSTandDTL(TblCommissionConversion cashEntry);
        void AddBySP(TblCommissionConversion tblCommissionConversion);
    }
    public class CommissionConversionRepository : BaseRepository<TblCommissionConversion>, ICommissionConversionRepository
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
                    var result = SqlMapper.Query<TblCommissionConversion>(connection, mainDbUser.DbUser + "SP_Get_CashEntry_ByBranchCode", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetCommissionConversionList(bool isRegistrationPermitted, double transAmtLimit)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = null;
                    if (isRegistrationPermitted)
                    {
                        query = @"Select Trans_No as TransNo,Mphone,Amount, Create_Date as CreateDate,Create_User as CreateUser, Status
                              from " + mainDbUser.DbUser + "TBL_COMMISSION_CONVERSION where (status is null or status='P') and amount <= " + transAmtLimit + " order by Create_Date desc";
                    }
                    else
                    {
                        query = @"Select Trans_No as TransNo,Mphone,Amount, Create_Date as CreateDate,Create_User as CreateUser  ,Status 
                            from " + mainDbUser.DbUser + "TBL_COMMISSION_CONVERSION order by Create_Date desc";
                    }

                    var result = connection.Query<TblCommissionConversion>(query).ToList();
                    connection.Close();

                    return result;
                }

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

            }
            catch (Exception)
            {

                throw;
            }
        }

        public TblCommissionConversion GetCommissionConversionByTransNo(string transNo)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select c.Trans_No as TransNo,c.Mphone,
                               c.Amount,c.Create_Date AS TransDate,c.Status,c.Create_User AS CreateUser,c.Checked_User AS CheckedUser
                               ,c.Checked_Date as CheckedDate,c.REMARKS, r.name as n_ame,  
                               CASE Cat_ID
                                                          WHEN 'D' THEN
                                                                'Distributor'
                                                          WHEN 'R' THEN
                                                                'DSR'
                                                          WHEN 'A' THEN
                                                                'Agent' 
                                                         WHEN 'M' THEN
                                                               'Merchant' 
                                                           WHEN 'C' THEN
                                                           'Customer'     
                                                      END as C_ategory 
                            from " + mainDbUser.DbUser + "tbl_commission_conversion c inner join " + mainDbUser.DbUser + "reginfo r on c.mphone = r.mphone where Trans_no='" + transNo + "'";


                    var result = connection.QueryFirst<TblCommissionConversion>(query);
                    connection.Close();

                    return result;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public object DataInsertToTransMSTandDTL(TblCommissionConversion _TblCommissionConversion)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, "BC");
                    parameter.Add("V_FR_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, _TblCommissionConversion.Mphone);
                    parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, "BC");
                    parameter.Add("V_BALANCE_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "C");
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, _TblCommissionConversion.Amount);
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, null);
                    parameter.Add("V_BILLNO", OracleDbType.Varchar2, ParameterDirection.Input, null);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, _TblCommissionConversion.TransNo);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, "C");
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, _TblCommissionConversion.CheckedUser);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_APPROVE_TBL_COM_CONVERSION", param: parameter, commandType: CommandType.StoredProcedure);

                    
                    connection.Close();
                    string flag = parameter.oracleParameters[9].Value != null ? parameter.oracleParameters[9].Value.ToString() : null;
                    string successOrErrorMsg = null;
                    if (flag == "0")
                    {
                        //successOrErrorMsg = "Sorry! This receiver account has reached the momentary maximum transaction amount limit.";
                        successOrErrorMsg = "Sorry! Transaction failed.";
                    }
                    else
                    {
                        successOrErrorMsg = flag;
                    }
                    return successOrErrorMsg;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddBySP(TblCommissionConversion tblCommissionConversion)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, tblCommissionConversion.TransNo);
                    parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, tblCommissionConversion.Mphone);
                    parameter.Add("V_AMOUNT", OracleDbType.Double, ParameterDirection.Input, tblCommissionConversion.Amount);
                    parameter.Add("V_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, tblCommissionConversion.Status);
                    parameter.Add("V_CREATE_USER", OracleDbType.Varchar2, ParameterDirection.Input, tblCommissionConversion.CreateUser);
                    
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_INSERT_TBL_COM_CONVERSION", param: parameter, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

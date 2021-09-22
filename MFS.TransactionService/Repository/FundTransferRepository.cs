using Dapper;
using MFS.TransactionService.Models;
using MFS.TransactionService.Models.ViewModels;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface IFundTransferRepository : IBaseRepository<FundTransfer>
    {
        object GetGlList();
        object getGlDetailsForRobi();
        object getGlDetailsForAirtel();
        object getGlDetailsForBlink();
        object getGlDetailsForTtalk();
        object GetAmountByGL(string sysCode);
        object GetACList();
        object GetAmountByAC(string mPhone);
        object GetTransactionList(string hotkey, string branchCode, double transAmtLimt);
        string DataInsertToTransMSTandDTL(FundTransfer fundTransferModel, string transType);
        VMACandGLDetails GetACandGLDetailsByMphone(string transFrom);
        object saveBranchCashIn(BranchCashIn branchCashIn);
        object AproveOrRejectBranchCashout(TblPortalCashout tblPortalCashout, string evnt);
        object saveRobiTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry);
        object saveAirtelTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry);
        object saveBlinkTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry);
        object saveTtalkTopupStockEntry(RobiTopupStockEntry ttalkTopupStockEntry);
        object getAmountByTransNo(string transNo, string mobile);
        object GetGLBalanceByGLSysCoaCode(string sysCoaCode);
        string GetCoaCodeBySysCoaCode(string fromSysCoaCode);
        object GetCommissionGlListForDDL();
        object GetCommssionMobileList(string sysCoaCode, string entryOrApproval);
        void SaveCommissionEntry(CommissionMobile item, string entryBy, string toCatId, string entrybrCode, string transNo);
        string CheckPendingApproval();
        void UpdateCommissionEntry(CommissionMobile item, string entryBy);
        string AproveOrRejectCommissionEntry(CommissionMobile item, string entryBy);
        string CheckData(string transNo, string mphone, double amount);
    }
    public class FundTransferRepository : BaseRepository<FundTransfer>, IFundTransferRepository
    {
        MainDbUser mainDbUser = new MainDbUser();
        public object GetGlList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetGlListForDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object getGlDetailsForRobi()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Get_GlDetailsForRobi", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object getGlDetailsForAirtel()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var result = connection.QueryFirstOrDefault<dynamic>("Select g.Coa_code as GLCODE,g.sys_coa_code as SysCoaCode,g.coa_desc GLName,round((Select SUM(DR_AMT) - SUM(CR_AMT) from" + mainDbUser.DbUser + "GL_Trans_DTL where SYS_COA_CODE = g.sys_coa_code), 2) as Amount from " + mainDbUser.DbUser + "GL_COA g where g.coa_code ='1010302'");
                    connection.Close();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object getGlDetailsForBlink()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var result = connection.QueryFirstOrDefault<dynamic>("Select g.Coa_code as GLCODE,g.sys_coa_code as SysCoaCode,g.coa_desc GLName,round((Select SUM(DR_AMT) - SUM(CR_AMT) from" + mainDbUser.DbUser + "GL_Trans_DTL where SYS_COA_CODE = g.sys_coa_code), 2) as Amount from " + mainDbUser.DbUser + "GL_COA g where g.coa_code ='1010303'");
                    connection.Close();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object getGlDetailsForTtalk()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var result = connection.QueryFirstOrDefault<dynamic>("Select g.Coa_code as GLCODE,g.sys_coa_code as SysCoaCode,g.coa_desc GLName,NVL(round((Select SUM(DR_AMT) - SUM(CR_AMT) from" + mainDbUser.DbUser + "GL_Trans_DTL where SYS_COA_CODE = g.sys_coa_code), 2),0) as Amount from " + mainDbUser.DbUser + "GL_COA g where g.coa_code ='1010304'");
                    connection.Close();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetACList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetACListForDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetAmountByGL(string sysCode)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("SysCoaCode", OracleDbType.Varchar2, ParameterDirection.Input, sysCode);
                    parameter.Add("GLBalance", OracleDbType.Double, ParameterDirection.Output);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_GET_GLBalanceBySysCoaCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();
                    var result = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetAmountByAC(string mPhone)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("mPhoneAC", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_GetAmountByAC", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetTransactionList(string hotkey, string branchCode, double transAmtLimt)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("Hot_Key", OracleDbType.Varchar2, ParameterDirection.Input, hotkey);
                    parameter.Add("branchCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    parameter.Add("transAmtLimt", OracleDbType.Double, ParameterDirection.Input, transAmtLimt);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_Get_TransactionListForDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string DataInsertToTransMSTandDTL(FundTransfer fundTransferModel, string transType)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, transType);
                    if (fundTransferModel.Hotkey.Substring(0, fundTransferModel.Hotkey.IndexOf(" ")) == "AC")
                    {
                        parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.TransFrom);
                    }
                    else
                    {
                        parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.FromSysCoaCode);
                    }
                    if (fundTransferModel.Hotkey.Substring(fundTransferModel.Hotkey.LastIndexOf(' ') + 1) == "AC")
                    {
                        parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.TransTo);
                    }
                    else
                    {
                        parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.ToSysCoaCode);
                    }


                    parameter.Add("V_AMT", OracleDbType.Double, ParameterDirection.Input, fundTransferModel.PayAmt);
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.Hotkey);
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("V_ENTRY_USER", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.CheckUser);
                    parameter.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input);
                    parameter.Add("P_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, fundTransferModel.TransNo);

                    //SqlMapper.Query<dynamic>(connection, "PROC_BASIC_TRANSACTION", param: parameter, commandType: CommandType.StoredProcedure);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Approve_FundTransfer", param: parameter, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;


                    return flag;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VMACandGLDetails GetACandGLDetailsByMphone(string transFrom)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("transFrom", OracleDbType.Varchar2, ParameterDirection.Input, transFrom);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<VMACandGLDetails>(connection, mainDbUser.DbUser + "SP_GET_ACandGLDetailsByMphone", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object saveBranchCashIn(BranchCashIn branchCashIn)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(branchCashIn.TransNo));
                    parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, branchCashIn.Mphone);
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, branchCashIn.CashInAmount);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "BP");
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, branchCashIn.BranchCode);
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, branchCashIn.CheckedUser);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Insert_Portal_CashIn", param: parameter, commandType: CommandType.StoredProcedure);
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


            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object AproveOrRejectBranchCashout(TblPortalCashout tblPortalCashout, string evnt)
        {
            string successOrErrorMsg = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = "Select Status from " + mainDbUser.DbUser + "Tbl_Portal_Cashout where Trans_no =" + "'" + tblPortalCashout.TransNo + "'";
                    string staus = connection.QueryFirstOrDefault<string>(query);

                    if (staus == null)
                    {
                        var parameter = new OracleDynamicParameters();
                        parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(tblPortalCashout.TransNo));
                        parameter.Add("V_FR_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, tblPortalCashout.Mphone);
                        parameter.Add("V_BALANCE_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, tblPortalCashout.BalanceType);
                        parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, tblPortalCashout.Amount);
                        parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                        parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                        parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                        parameter.Add("V_TO_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "BP");
                        parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, tblPortalCashout.BranchCode);
                        parameter.Add("V_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, tblPortalCashout.Gateway);
                        parameter.Add("CheckBy", OracleDbType.Varchar2, ParameterDirection.Input, tblPortalCashout.CheckBy);

                        if (evnt == "register")
                        {
                            //var result = SqlMapper.Query<dynamic>(connection, "SP_Branch_Cashout_Approve", param: parameter, commandType: CommandType.StoredProcedure);
                            SqlMapper.Query(connection, mainDbUser.DbUser + "SP_Branch_Cashout_Approve", param: parameter, commandType: CommandType.StoredProcedure);
                        }
                        else
                        {
                            var result = SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Branch_Cashout_Reject", param: parameter, commandType: CommandType.StoredProcedure);
                        }



                        connection.Close();

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
                        successOrErrorMsg = "Failed";
                    }



                    return successOrErrorMsg;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object saveRobiTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            string successOrErrorMsg = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    // string newID = "";
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "A2G");
                    //parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.FromSysCoaCode);
                    parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "01848471748");
                    parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "A40000000131");
                    parameter.Add("V_AMT", OracleDbType.Double, ParameterDirection.Input, robiTopupStockEntry.TransactionAmt);
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.Hotkey);
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("V_ENTRY_USER", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.EntryUser);
                    parameter.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input);

                    SqlMapper.Query<string>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //string newID = parameter.<string>("V_FLAG");
                    string transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (transactionNo != null)
                    {
                        //parameter.oracleParameters[1].Value = robiTopupStockEntry.FromSysCoaCode;
                        parameter.oracleParameters[1].Value = "01848471748";
                        parameter.oracleParameters[2].Value = "L40000000135";
                        parameter.oracleParameters[3].Value = robiTopupStockEntry.RowThreeFour;
                        parameter.Add("P_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transactionNo);
                        parameter.Add("V_TRANS_SL_NO", OracleDbType.Int32, ParameterDirection.Input, 3);

                        SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                        if (transactionNo != null)
                        {
                            parameter.oracleParameters[0].Value = "G2G";
                            parameter.oracleParameters[1].Value = "A40000000137";
                            parameter.oracleParameters[2].Value = "L40000000135";
                            parameter.oracleParameters[3].Value = robiTopupStockEntry.RowFiveSix;
                            parameter.oracleParameters[8].Value = transactionNo;
                            parameter.oracleParameters[9].Value = 5;

                            SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        }


                    }

                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (flag == "0")
                    {
                        successOrErrorMsg = "Failed";
                    }
                    else
                    {
                        //successOrErrorMsg = "1";
                        successOrErrorMsg = transactionNo;
                    }
                    return successOrErrorMsg;

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object saveAirtelTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            string successOrErrorMsg = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    // string newID = "";
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "A2G");
                    //parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.FromSysCoaCode);
                    parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "01638462835");
                    parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "A40000000149");
                    parameter.Add("V_AMT", OracleDbType.Double, ParameterDirection.Input, robiTopupStockEntry.TransactionAmt);
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.Hotkey);
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("V_ENTRY_USER", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.EntryUser);
                    parameter.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input);

                    SqlMapper.Query<string>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //string newID = parameter.<string>("V_FLAG");
                    string transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (transactionNo != null)
                    {
                        //parameter.oracleParameters[1].Value = robiTopupStockEntry.FromSysCoaCode;
                        parameter.oracleParameters[1].Value = "01638462835";
                        parameter.oracleParameters[2].Value = "L40000000151";
                        parameter.oracleParameters[3].Value = robiTopupStockEntry.RowThreeFour;
                        parameter.Add("P_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transactionNo);
                        parameter.Add("V_TRANS_SL_NO", OracleDbType.Int32, ParameterDirection.Input, 3);

                        SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                        if (transactionNo != null)
                        {
                            parameter.oracleParameters[0].Value = "G2G";
                            parameter.oracleParameters[1].Value = "A40000000137";
                            parameter.oracleParameters[2].Value = "L40000000151";
                            parameter.oracleParameters[3].Value = robiTopupStockEntry.RowFiveSix;
                            parameter.oracleParameters[8].Value = transactionNo;
                            parameter.oracleParameters[9].Value = 5;

                            SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        }


                    }

                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (flag == "0")
                    {
                        successOrErrorMsg = "Failed";
                    }
                    else
                    {
                        //successOrErrorMsg = "1";
                        successOrErrorMsg = transactionNo;
                    }
                    return successOrErrorMsg;

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public object saveBlinkTopupStockEntry(RobiTopupStockEntry robiTopupStockEntry)
        {
            string successOrErrorMsg = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    // string newID = "";
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "A2G");
                    //parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.FromSysCoaCode);
                    parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "01967021244");
                    parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "A40000000198");
                    parameter.Add("V_AMT", OracleDbType.Double, ParameterDirection.Input, robiTopupStockEntry.TransactionAmt);
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.Hotkey);
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("V_ENTRY_USER", OracleDbType.Varchar2, ParameterDirection.Input, robiTopupStockEntry.EntryUser);
                    parameter.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input);

                    SqlMapper.Query<string>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //string newID = parameter.<string>("V_FLAG");
                    string transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (transactionNo != null)
                    {
                        //parameter.oracleParameters[1].Value = robiTopupStockEntry.FromSysCoaCode;
                        parameter.oracleParameters[1].Value = "01967021244";
                        parameter.oracleParameters[2].Value = "L40000000200";
                        parameter.oracleParameters[3].Value = robiTopupStockEntry.RowThreeFour;
                        parameter.Add("P_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transactionNo);
                        parameter.Add("V_TRANS_SL_NO", OracleDbType.Int32, ParameterDirection.Input, 3);

                        SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                        if (transactionNo != null)
                        {
                            parameter.oracleParameters[0].Value = "G2G";
                            parameter.oracleParameters[1].Value = "A40000000137";
                            parameter.oracleParameters[2].Value = "L40000000200";
                            parameter.oracleParameters[3].Value = robiTopupStockEntry.RowFiveSix;
                            parameter.oracleParameters[8].Value = transactionNo;
                            parameter.oracleParameters[9].Value = 5;

                            SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        }


                    }

                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (flag == "0")
                    {
                        successOrErrorMsg = "Failed";
                    }
                    else
                    {
                        //successOrErrorMsg = "1";
                        successOrErrorMsg = transactionNo;
                    }
                    return successOrErrorMsg;

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object saveTtalkTopupStockEntry(RobiTopupStockEntry ttalkTopupStockEntry)
        {
            string successOrErrorMsg = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    // string newID = "";
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "A2G");
                    parameter.Add("V_DR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "01513907020");
                    parameter.Add("V_CR_AC_GL", OracleDbType.Varchar2, ParameterDirection.Input, "A40000000218");
                    parameter.Add("V_AMT", OracleDbType.Double, ParameterDirection.Input, ttalkTopupStockEntry.TransactionAmt);
                    parameter.Add("V_HOTKEY", OracleDbType.Varchar2, ParameterDirection.Input, ttalkTopupStockEntry.Hotkey);
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("V_ENTRY_USER", OracleDbType.Varchar2, ParameterDirection.Input, ttalkTopupStockEntry.EntryUser);
                    parameter.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input);

                    SqlMapper.Query<string>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //string newID = parameter.<string>("V_FLAG");
                    string transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (transactionNo != null)
                    {
                        //parameter.oracleParameters[1].Value = robiTopupStockEntry.FromSysCoaCode;
                        parameter.oracleParameters[1].Value = "01513907020";
                        parameter.oracleParameters[2].Value = "L40000000220";
                        parameter.oracleParameters[3].Value = ttalkTopupStockEntry.RowThreeFour;
                        parameter.Add("P_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transactionNo);
                        parameter.Add("V_TRANS_SL_NO", OracleDbType.Int32, ParameterDirection.Input, 3);

                        SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        transactionNo = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                        if (transactionNo != null)
                        {
                            parameter.oracleParameters[0].Value = "G2G";
                            parameter.oracleParameters[1].Value = "A40000000137";
                            parameter.oracleParameters[2].Value = "L40000000220";
                            parameter.oracleParameters[3].Value = ttalkTopupStockEntry.RowFiveSix;
                            parameter.oracleParameters[8].Value = transactionNo;
                            parameter.oracleParameters[9].Value = 5;

                            SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_BASIC_TRANSACTION_V2", param: parameter, commandType: CommandType.StoredProcedure);
                        }


                    }

                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;

                    if (flag == "0")
                    {
                        successOrErrorMsg = "Failed";
                    }
                    else
                    {
                        //successOrErrorMsg = "1";
                        successOrErrorMsg = transactionNo;
                    }
                    return successOrErrorMsg;

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object getAmountByTransNo(string transNo, string mobile)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("transNo", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
                    parameter.Add("mobile", OracleDbType.Varchar2, ParameterDirection.Input, mobile);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_GET_AmountByTransNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public object GetGLBalanceByGLSysCoaCode(string sysCoaCode)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("SysCoaCode", OracleDbType.Varchar2, ParameterDirection.Input, sysCoaCode);
                    parameter.Add("GLBalance", OracleDbType.Double, ParameterDirection.Output);
                    //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_GET_GLBalanceBySysCoaCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();
                    var result = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                    return result;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetCoaCodeBySysCoaCode(string fromSysCoaCode)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("SysCoaCode", OracleDbType.Varchar2, ParameterDirection.Input, fromSysCoaCode);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    string result = SqlMapper.Query<string>(connection, mainDbUser.DbUser + "SP_GET_GetCoaCodeBySysCoaCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();
                    //var result = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                    return result;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetCommissionGlListForDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select sys_coa_code as Value, concat(concat(concat(Coa_desc,' ('),coa_code),')') as Label  from " + mainDbUser.DbUser + "gl_coa where Sys_coa_code in ('L40000000087','L40000000046')";

                    var result = connection.Query<CustomDropDownModel>(query).ToList();
                    connection.Close();

                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetCommssionMobileList(string sysCoaCode, string entryOrApproval)
        {
            string query = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    if (entryOrApproval == "ForEntry")
                    {
                        query = @"SELECT GL.COA_CODE GlCode, R.NAME,D.MPHONE,TRUNC(SUM(NVL(CR_AMT, 0)-NVL(DR_AMT, 0)), 2) AS Amount, '' as MakeStatus FROM " + mainDbUser.DbUser + "GL_TRANS_DTL D, " + mainDbUser.DbUser + "GL_TRANS_MST M, " + mainDbUser.DbUser + "REGINFO R , " + mainDbUser.DbUser + "GL_COA GL WHERE D.SYS_COA_CODE = " + "'" + sysCoaCode + "'" + " AND D.TRANS_NO = M.TRANS_NO AND D.MPHONE = R.MPHONE AND R.REG_STATUS = 'P' AND R.STATUS IN('A','I') AND D.SYS_COA_CODE = GL.SYS_COA_CODE GROUP BY GL.COA_CODE, R.NAME,D.MPHONE HAVING TRUNC(SUM(NVL(CR_AMT, 0) - NVL(DR_AMT, 0)), 2) > 0 ORDER BY TRUNC(SUM(NVL(CR_AMT, 0) - NVL(DR_AMT, 0)), 2) DESC";
                    }
                    else
                    {
                        query = @"SELECT CPN.TRANS_NO  as TransNo, R.NAME,CPN.TRANS_TO AS MPHONE,CPN.MSG_AMT AS Amount, '' as MakeStatus FROM " + mainDbUser.DbUser + "COMMISSION_POSTING_NEW CPN INNER JOIN " + mainDbUser.DbUser + "REGINFO R ON CPN.TRANS_TO=R.MPHONE WHERE CPN.STATUS='M' AND CPN.TO_CAT_ID=" + "'" + sysCoaCode + "'";
                    }


                    var result = connection.Query<CommissionMobile>(query).ToList();
                    connection.Close();

                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SaveCommissionEntry(CommissionMobile item, string entryBy, string toCatId, string entrybrCode, string transNo)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"insert into " + mainDbUser.DbUser + "COMMISSION_POSTING_NEW (Trans_no,Trans_to,To_cat_id,Status,msg_amt,entry_user,Entry_br_code) values (" + "'" + transNo + "'," + "'" + item.Mphone + "'," + "'" + toCatId + "','M'," + "'" + item.Amount + "'," + "'" + entryBy + "'," + "'" + entrybrCode + "')";

                    var result = connection.Query<dynamic>(query).ToList();
                    connection.Close();

                    //return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string CheckPendingApproval()
        {
            string result = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select distinct Status from " + mainDbUser.DbUser + "COMMISSION_POSTING_NEW where Status='M'";

                    result = connection.Query<string>(query).FirstOrDefault();
                    connection.Close();

                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void UpdateCommissionEntry(CommissionMobile item, string entryBy)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Update " + mainDbUser.DbUser + "COMMISSION_POSTING_NEW set status='R', CHECK_USER='" + entryBy + "', CHECK_DATE=SYSDATE where Trans_no=" + "'" + item.TransNo + "'";

                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    connection.Close();

                    //return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string AproveOrRejectCommissionEntry(CommissionMobile item, string entryBy)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(item.TransNo));
                    parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, item.Mphone);
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, item.Amount);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_CHECKED_USER", OracleDbType.Varchar2, ParameterDirection.Input, entryBy);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "PROC_DISBURSE_COMMISSION", param: parameter, commandType: CommandType.StoredProcedure);
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


            }
            catch (Exception e)
            {

                throw;
            }
        }

        public string CheckData(string transNo, string mphone, double amount)
        {
            string result = null;
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    //string query = @"Select Trans_no from " + mainDbUser.DbUser + "tbl_portal_cashout  where status is null and trans_no=" + "'" + transNo + "' and Mphone=" + "'" + mphone + "' and amount=" + amount;
                    string query = @"Select Trans_no from " + mainDbUser.DbUser + "tbl_portal_cashout where status is null and trans_no=" + "'" + transNo + "' and Mphone=" + "'" + mphone + "' ";

                    result = connection.Query<string>(query).FirstOrDefault();
                    connection.Close();

                    if (result != null)
                    {
                        result = "1";
                    }

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

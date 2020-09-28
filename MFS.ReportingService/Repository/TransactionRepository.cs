using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Repository
{
    public interface ITransactionRepository : IBaseRepository<AccountStatement>
    {
        List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate);
        List<AccountStatement> GetAccountStatementListForClient(string mphone, string fromDate, string toDate);
        List<MerchantTransaction> GetMerchantTransactionReport(string mphone, string fromDate, string toDate);
        List<CurrentAffairsStatement> CurrentAffairsStatement(string date, string CurrentOrEOD);
        List<CurrentAffairsStatement> GetChartOfAccounts();
        object GetGetGlCoaCodeNameLevelDDL(string assetType);
        List<GLStatement> GetGLStatementList(string fromDate, string toDate, string assetType, string sysCoaCode);
        object GetOkServicesDDL();
        List<TransactionSummary> GetTransactionSummaryList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway);
        List<TransactionDetails> GetTransactionDetailsList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway);
        List<FundTransfer> GetFundTransferList(string tansactionType, string option, string fromDate, string toDate);
        List<MerchantTransactionSummary> MerchantTransactionSummaryReport(string mphone, string fromDate, string toDate);
        List<BranchCashinCashout> GetBranchCashinCashoutList(string branchCode, string cashinCashoutType, string option, string fromDate, string toDate);
        object GetParticularDDL();
        object GetTransactionDDLByParticular(string particular);
        List<ParticularWiseTransaction> GetParticularWiseTransList(string particular, string transaction, string fromDate, string toDate);
    }
    public class TransactionRepository : BaseRepository<AccountStatement>, ITransactionRepository
    {
        MainDbUser mainDbUser = new MainDbUser();
        public List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate)
        {
            List<AccountStatement> result = new List<AccountStatement>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("AccountNo", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<AccountStatement>(connection, mainDbUser.DbUser + "RPT_AccountStatement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }
        public List<AccountStatement> GetAccountStatementListForClient(string mphone, string fromDate, string toDate)
        {
            List<AccountStatement> result = new List<AccountStatement>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("AccountNo", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<AccountStatement>(connection, mainDbUser.DbUser + "RPT_ACCOUNTSTATEMENT_CLIENT", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<MerchantTransaction> GetMerchantTransactionReport(string mphone, string fromDate, string toDate)
        {
            List<MerchantTransaction> result = new List<MerchantTransaction>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<MerchantTransaction>(connection, mainDbUser.DbUser + "RPT_MerchantTransaction", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<CurrentAffairsStatement> CurrentAffairsStatement(string date, string CurrentOrEOD)
        {
            List<CurrentAffairsStatement> result = new List<CurrentAffairsStatement>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    //var dyParam = new OracleDynamicParameters();

                    //dyParam.Add("generatedate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(date));
                    //dyParam.Add("CurrentOrEOD", OracleDbType.Varchar2, ParameterDirection.Input, CurrentOrEOD);
                    //dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    //result = SqlMapper.Query<CurrentAffairsStatement>(connection, "RPT_CurrentAffairsStatement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    //this.CloseConnection(connection); 

                    //DateTime dt = Convert.ToDateTime(date).GetDateTimeFormats('DD-MON-YY');

                    //string query = @"SELECT A.COA_CODE AccountsCode,A.PARENT_CODE ParentCode,A.COA_DESC AccountsDesc,A.COA_LEVEL CoaLevel
                    //              ,ROUND(NVL(SUM(CR_AMT),0),8)  CrAmt ,ROUND(NVL(SUM(DR_AMT),0),8)  DrAmt,A.SYS_COA_CODE SysCoaCode
                    //              ,LEVEL_TYPE LevelType,A.ACC_TYPE AccType , 0 as Balance 
                    //              FROM GL_COA A LEFT OUTER JOIN GL_TRANS_DTL B 
                    //              ON A.SYS_COA_CODE=B.SYS_COA_CODE
                    //              WHERE TO_DATE(" + dt + ", 'DD-MON-YY') >= (CASE WHEN 'Current' =" + "'" + CurrentOrEOD + "'" + " THEN TO_DATE(B.Trans_Date, 'DD-MON-YY') ELSE TO_DATE(B.Value_Date, 'DD-MON-YY') END) GROUP BY A.COA_CODE,A.PARENT_CODE,A.COA_DESC,A.COA_LEVEL,A.SYS_COA_CODE,LEVEL_TYPE,A.ACC_TYPE";

                    List<CurrentAffairsFirst> firstList = new List<CurrentAffairsFirst>();
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("generatedate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(date));
                    dyParam.Add("CurrentOrEOD", OracleDbType.Varchar2, ParameterDirection.Input, CurrentOrEOD);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    firstList = SqlMapper.Query<CurrentAffairsFirst>(connection, mainDbUser.DbUser + "RPT_CurrentAffairsFirst", param: dyParam, commandType: CommandType.StoredProcedure).ToList();


                    string query2 = @"SELECT  SUBSTR(Coa_Code, 1, 2) as FirstGroup ,COA_LEVEL CoaLevel,COA_CODE AccountsCode,SYS_COA_CODE SysCoaCode,PARENT_CODE ParentCode,
                                       LPAD('    ', 4 * LEVEL - 1) || COA_DESC AS AccountsDesc ,ACC_TYPE AccType, 0 as Balance 
                                        FROM(
                                        SELECT A.COA_CODE,A.PARENT_CODE PARENT_CODE,A.COA_DESC,A.COA_LEVEL,A.SYS_COA_CODE SYS_COA_CODE,LEVEL_TYPE, ACC_TYPE FROM " + mainDbUser.DbUser + "GL_COA A) START WITH PARENT_CODE IS NULL CONNECT BY PRIOR SYS_COA_CODE = PARENT_CODE";

                    result = connection.Query<CurrentAffairsStatement>(query2).ToList();


                    foreach (var item in result)
                    {
                        bool clearStatic = true;
                        item.Balance = getBalanceByReversing(item.SysCoaCode, item.AccType, firstList, clearStatic);
                    }


                    this.CloseConnection(connection);

                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        double balance = 0;
        private double getBalanceByReversing(string parent, string accType, List<CurrentAffairsFirst> firstList, bool clearStatic = false)
        {


            if (clearStatic)
            {
                balance = 0;
            }

            foreach (var item in firstList)
            {
                if (item.ParentCode == parent && item.CoaLevel == 1)
                {
                    getBalanceByReversing(item.SysCoaCode, accType, firstList);
                }
                else if (item.ParentCode == parent && item.CoaLevel == 2)
                {
                    getBalanceByReversing(item.SysCoaCode, accType, firstList);
                }
                else if (item.ParentCode == parent && item.CoaLevel == 3)
                {
                    getBalanceByReversing(item.SysCoaCode, accType, firstList);
                }
                else if (item.ParentCode == parent && item.CoaLevel == 4)
                {

                    if (accType == "A" || accType == "E")
                        balance += item.DrAmt - item.CrAmt;
                    else if (accType == "L" || accType == "I")
                        balance += item.CrAmt - item.DrAmt;
                }
                else if (item.SysCoaCode == parent && item.CoaLevel == 4)
                {
                    if (accType == "A" || accType == "E")
                        balance += item.DrAmt - item.CrAmt;
                    else if (accType == "L" || accType == "I")
                        balance += item.CrAmt - item.DrAmt;
                }
            }
            return balance;
        }

        public List<CurrentAffairsStatement> GetChartOfAccounts()
        {
            List<CurrentAffairsStatement> result = new List<CurrentAffairsStatement>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();


                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<CurrentAffairsStatement>(connection, mainDbUser.DbUser + "RPT_ChartOfAccounts", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public object GetGetGlCoaCodeNameLevelDDL(string assetType)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseCompanyDDL", param: parameter, commandType: CommandType.StoredProcedure);
                    //string query = @"select t.catdesc as ""label"", t.catid as ""value"" from category t";
                    string query = @"Select  Sys_coa_code as ""Value"", CONCAT(COnCAT(CONCAT(CONCAT(CONCAT(CONCAT(coa_code, ' ('), Coa_desc), ')'), ' (Level-'), coa_level), ')') as ""Label"" from " + mainDbUser.DbUser + "gl_coa where Acc_Type =" + "'" + assetType + "'" + " and coa_level =4";

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

        public object GetOkServicesDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();

                    //string query = @"Select  Sys_coa_code as ""Value"", CONCAT(COnCAT(CONCAT(CONCAT(CONCAT(CONCAT(coa_code, ' ('), Coa_desc), ')'), ' (Level-'), coa_level), ')') as ""Label"" from gl_coa where Acc_Type =" + "'" + assetType + "'" + " and coa_level =4";
                    string query = @"Select concat(concat(concat(concat(concat(Rateconfig_for,' ('),from_cat_Id),' '),to_cat_id),')') as ""Value"" ,concat(concat(concat(concat(concat(Rateconfig_for,' ('),from_cat_Id),' '),to_cat_id),')') as ""Label"" from " + mainDbUser.DbUser + "RATECONFIG_TYPE";

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

        public List<GLStatement> GetGLStatementList(string fromDate, string toDate, string assetType, string sysCoaCode)
        {
            List<GLStatement> result = new List<GLStatement>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("assetType", OracleDbType.Varchar2, ParameterDirection.Input, assetType);
                    dyParam.Add("sysCoaCode", OracleDbType.Varchar2, ParameterDirection.Input, sysCoaCode);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<GLStatement>(connection, mainDbUser.DbUser + "RPT_GLStatement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TransactionSummary> GetTransactionSummaryList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway)
        {
            List<TransactionSummary> result = new List<TransactionSummary>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    char character = char.Parse(gateway);
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType);
                    dyParam.Add("fromCat", OracleDbType.Varchar2, ParameterDirection.Input, fromCat);
                    dyParam.Add("toCat", OracleDbType.Varchar2, ParameterDirection.Input, toCat);
                    dyParam.Add("dateType", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("gateway", OracleDbType.Char, ParameterDirection.Input, character);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<TransactionSummary>(connection, mainDbUser.DbUser + "RPT_TransactionSummary", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TransactionDetails> GetTransactionDetailsList(string tansactionType, string fromCat, string toCat, string dateType, string fromDate, string toDate, string gateway)
        {
            List<TransactionDetails> result = new List<TransactionDetails>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    char character = char.Parse(gateway);
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType);
                    dyParam.Add("fromCat", OracleDbType.Varchar2, ParameterDirection.Input, fromCat);
                    dyParam.Add("toCat", OracleDbType.Varchar2, ParameterDirection.Input, toCat);
                    dyParam.Add("dateType", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("gateway", OracleDbType.Char, ParameterDirection.Input, character);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<TransactionDetails>(connection, mainDbUser.DbUser + "RPT_TransactionDetails", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<FundTransfer> GetFundTransferList(string tansactionType, string option, string fromDate, string toDate)
        {
            List<FundTransfer> result = new List<FundTransfer>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType);
                    //dyParam.Add("fromCat", OracleDbType.Varchar2, ParameterDirection.Input, fromCat);
                    //dyParam.Add("toCat", OracleDbType.Varchar2, ParameterDirection.Input, toCat);
                    dyParam.Add("options", OracleDbType.Varchar2, ParameterDirection.Input, option);
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<FundTransfer>(connection, mainDbUser.DbUser + "RPT_FundTransfer", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<MerchantTransactionSummary> MerchantTransactionSummaryReport(string mphone, string fromDate, string toDate)
        {
            List<MerchantTransactionSummary> result = new List<MerchantTransactionSummary>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<MerchantTransactionSummary>(connection, mainDbUser.DbUser + "RPT_MerchantSummary", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<BranchCashinCashout> GetBranchCashinCashoutList(string branchCode, string cashinCashoutType, string option, string fromDate, string toDate)
        {

            List<BranchCashinCashout> result = new List<BranchCashinCashout>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_BRANCHCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    dyParam.Add("V_CASHINCASHOUTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, cashinCashoutType);
                    dyParam.Add("V_OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, option);
                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<BranchCashinCashout>(connection, mainDbUser.DbUser + "RPT_BranchCashinCashout", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetParticularDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select distinct particular as Value, particular as Label  from " + mainDbUser.DbUser + "gl_trans_mst where particular is not null";

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

        public object GetTransactionDDLByParticular(string particular)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select Trans_No as Value,Trans_No as Label from " + mainDbUser.DbUser + "gl_trans_mst where particular=" + "'" + particular + "'";

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

        public List<ParticularWiseTransaction> GetParticularWiseTransList(string particular, string transaction, string fromDate, string toDate)
        {
            List<ParticularWiseTransaction> result = new List<ParticularWiseTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_Particular", OracleDbType.Varchar2, ParameterDirection.Input, particular);
                    dyParam.Add("V_Transaction", OracleDbType.Varchar2, ParameterDirection.Input, transaction);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<ParticularWiseTransaction>(connection, mainDbUser.DbUser + "RPT_ParticularWiseTrans", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
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

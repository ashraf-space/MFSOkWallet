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
        List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate, string balanceType=null);
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
        object GetTelcoDDL();
        List<ItemWiseServices> GetItemWiseServicesList(string telcoType, string fromDate, string toDate);
        object GetRmgDDL();
        List<RmgWiseSalaryDisbursement> GetRmgWiseSalaryDisbursementList(string rmgId, string fromDate, string toDate);
        List<MasterWallet> GetMasterWalletAccountStatementList(string mphone, string fromDate, string toDate, string transNo);
        List<DisbursementUploadDetails> GetDisbursementUpload(string fileUploadDate, string batchNumber, string reportType, int companyId);
        List<IndividualDisbursement> GetIndividualDisbursement(string fromDate, string toDate, string reportType, string status, string okWalletNo, int companyId);
        List<MfsStatement> GetMfsStatement(string year, string month);
        List<JgBillDailyDetails> GetJgBillDailyDetailsList(string fromDate, string toDate);
        List<TransactionAnalysis> GetTransactinAnalysisList();
        List<BackOffTransaction> GetBackOffTransactionList(string fromDate, string toDate);
        object GetCampaignTypeDDL();
        List<ReferralCampaignDetails> GetReferralCampaignList(string tansactionType, string campaignType, string fromDate, string toDate);
        List<BtclTelephoneBill> GetBtclTelephoneBill(string fromDate, string toDate);
        List<AdmissionFeePayment> GetadmissionFeePaymentList(string fromDate, string toDate);
        List<DisbursementVoucher> GetDisbursementVoucherList(string option,string disTypeId, string fromDate, string toDate);
        List<B2BCollectionDtlSummary> GetB2BCollectionDtlSummaryList(string tansactionType, string fromCat, string toCat, string fromDate, string toDate);
    }
    public class TransactionRepository : BaseRepository<AccountStatement>, ITransactionRepository
    {
        MainDbUser mainDbUser = new MainDbUser();
        public List<AccountStatement> GetAccountStatementList(string mphone, string fromDate, string toDate, string balanceType)
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
                    dyParam.Add("BalanceType", OracleDbType.Varchar2, ParameterDirection.Input, balanceType);

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
                    dyParam.Add("V_gateway", OracleDbType.Char, ParameterDirection.Input, character);
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
                    dyParam.Add("V_gateway", OracleDbType.Char, ParameterDirection.Input, character);
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
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType == "null" ? "" : tansactionType);
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

        public List<BackOffTransaction> GetBackOffTransactionList(string fromDate, string toDate)
        {
            List<BackOffTransaction> result = new List<BackOffTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<BackOffTransaction>(connection, mainDbUser.DbUser + "RPT_BackOffTransaction", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

        public object GetTelcoDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select distinct Telco_code as Value, Telco_name as Label  from " + mainDbUser.DbUser + "telco_list";

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

        public List<ItemWiseServices> GetItemWiseServicesList(string telcoType, string fromDate, string toDate)
        {
            List<ItemWiseServices> result = new List<ItemWiseServices>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_OPT_CODE", OracleDbType.Varchar2, ParameterDirection.Input, telcoType);
                    dyParam.Add("V_START_DATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_END_DATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_RETURN", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<ItemWiseServices>(connection, mainDbUser.DbUser + "RPT_SESSION_REPORT", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetRmgDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    string query = @"Select Company_Id as Value, Company_name as Label from " + mainDbUser.DbUser + "Tbl_Disburse_Company_Info where Sal_acc='70000000020'";

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

        public List<RmgWiseSalaryDisbursement> GetRmgWiseSalaryDisbursementList(string rmgId, string fromDate, string toDate)
        {
            List<RmgWiseSalaryDisbursement> result = new List<RmgWiseSalaryDisbursement>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_RmgId", OracleDbType.Varchar2, ParameterDirection.Input, rmgId);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<RmgWiseSalaryDisbursement>(connection, mainDbUser.DbUser + "RPT_RmgWiseSalaryDisbursement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<MasterWallet> GetMasterWalletAccountStatementList(string mphone, string fromDate, string toDate, string transNo)
        {
            List<MasterWallet> result = new List<MasterWallet>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, fromDate == "null" ? DateTime.Now : Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, toDate == "null" ? DateTime.Now : Convert.ToDateTime(toDate));
                    dyParam.Add("AccountNo", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo == "null" ? null : transNo);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<MasterWallet>(connection, mainDbUser.DbUser + "RPT_ACC_STAT_MER", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<DisbursementUploadDetails> GetDisbursementUpload(string fileUploadDate, string batchNumber, string reportType, int companyId)
        {
            List<DisbursementUploadDetails> result = new List<DisbursementUploadDetails>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FileUploadDate", OracleDbType.Date, ParameterDirection.Input, fileUploadDate == "null" ? DateTime.Now : Convert.ToDateTime(fileUploadDate));
                    dyParam.Add("V_BatchNumber", OracleDbType.Varchar2, ParameterDirection.Input, batchNumber);
                    dyParam.Add("V_ReportType", OracleDbType.Varchar2, ParameterDirection.Input, reportType == "null" ? null : reportType);
                    dyParam.Add("V_CompanyId", OracleDbType.Int32, ParameterDirection.Input, companyId);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<DisbursementUploadDetails>(connection, mainDbUser.DbUser + "RPT_Disbursement_Upload", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }


        public List<IndividualDisbursement> GetIndividualDisbursement(string fromDate, string toDate, string reportType, string status, string okWalletNo, int companyId)
        {
            List<IndividualDisbursement> result = new List<IndividualDisbursement>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, fromDate == "null" ? DateTime.Now : Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, toDate == "null" ? DateTime.Now : Convert.ToDateTime(toDate));
                    dyParam.Add("V_ReportType", OracleDbType.Varchar2, ParameterDirection.Input, reportType == "null" ? null : reportType);
                    dyParam.Add("V_Status", OracleDbType.Varchar2, ParameterDirection.Input, status == "null" ? null : status);
                    dyParam.Add("V_OkWalletNo", OracleDbType.Varchar2, ParameterDirection.Input, okWalletNo == "null" ? null : okWalletNo);
                    dyParam.Add("V_CompanyId", OracleDbType.Int32, ParameterDirection.Input, companyId);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<IndividualDisbursement>(connection, mainDbUser.DbUser + "RPT_IndividualDisbursement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    //string query = null;
                    //if (reportType == "Details")
                    //{
                    //    if (status == "Success")
                    //    {
                    //        query = @"Select VI.TRANS_NO as TransNo,VI.CHK_DATE as TransDate, VI.AC_NO as TransTo, VI.AMOUNT,VI.BATCHNO,'Success' as Status, VI.REMARKS
                    //                    from " + mainDbUser.DbUser + "tbl_disburse d inner join " + mainDbUser.DbUser + "tbl_disburse_invalid_data vi on d.batchno = vi.batchno where VI.ORGANIZATION_ID = " + companyId + " and VI.AC_NO = '" + okWalletNo + "' and TO_DATE(vi.chk_date, 'DD-MON-YY') between TO_DATE(" + Convert.ToDateTime(fromDate).Date + ", 'DD-MON-YY') and TO_DATE(" + Convert.ToDateTime(toDate).Date + ", 'DD-MON-YY') and vi.C_M_Status = 'V' and d.c_m_status = 'C' and d.status = 'P'";
                    //    }
                    //    else if (status == "Declined")
                    //    {
                    //        query = @"Select VI.TRANS_NO as TransNo,VI.CHK_DATE as TransDate, VI.AC_NO as TransTo, VI.AMOUNT,VI.BATCHNO,'Declined' as Status, VI.REMARKS
                    //                    from " + mainDbUser.DbUser + "tbl_disburse d inner join " + mainDbUser.DbUser + "tbl_disburse_invalid_data vi on d.batchno = vi.batchno where VI.ORGANIZATION_ID = " + companyId + " and VI.AC_NO = '" + okWalletNo + "' and TO_DATE(vi.chk_date, 'DD-MON-YY') between TO_DATE(" + Convert.ToDateTime(fromDate) + ", 'DD-MON-YY') and TO_DATE(" + Convert.ToDateTime(toDate) + ", 'DD-MON-YY') and vi.C_M_Status = 'V' and d.c_m_status = 'C' and d.status = 'R'";
                    //    }
                    //    else
                    //    {
                    //        query = @"Select VI.TRANS_NO as TransNo,VI.CHK_DATE as TransDate, VI.AC_NO as TransTo, VI.AMOUNT,VI.BATCHNO,'Failed' as Status, VI.REMARKS
                    //                    from " + mainDbUser.DbUser + "tbl_disburse_invalid_data vi where VI.ORGANIZATION_ID = " + companyId + " and VI.AC_NO = '" + okWalletNo + "' and TO_DATE(vi.chk_date, 'DD-MON-YY') between TO_DATE(" + Convert.ToDateTime(fromDate) + ", 'DD-MON-YY')and TO_DATE(" + Convert.ToDateTime(toDate) + ", 'DD-MON-YY') and vi.C_M_Status = 'I'";
                    //    }

                    //}
                    //else
                    //{
                    //    //query = @"Select Company_Id as Value, Company_name as Label from " + mainDbUser.DbUser + "Tbl_Disburse_Company_Info where Sal_acc='70000000020'";
                    //}



                    //result = connection.Query<IndividualDisbursement>(query).ToList();
                    //connection.Close();

                    return result;
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }


        public List<MfsStatement> GetMfsStatement(string year, string month)
        {
            List<MfsStatement> result = new List<MfsStatement>();

            try
            {
                var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(startDate));
                    dyParam.Add("V_TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(endDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<MfsStatement>(connection, mainDbUser.DbUser + "RPT_MFSStatement", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<JgBillDailyDetails> GetJgBillDailyDetailsList(string fromDate, string toDate)
        {
            List<JgBillDailyDetails> result = new List<JgBillDailyDetails>();

            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<JgBillDailyDetails>(connection, mainDbUser.DbUser + "RPT_JGBillDailyDetails", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }

        public List<TransactionAnalysis> GetTransactinAnalysisList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    List<TransactionAnalysis> transactionAnalysesList = new List<TransactionAnalysis>();
                    //string query = "select ('Up to Dec ' || (EXTRACT(YEAR  FROM  SYSDATE)-1)) as Caption, ROUND(sum(Msg_Amt)/1000000,2) as TransactionAmt from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id='C' OR To_Cat_Id='C' OR (From_Cat_Id in ('A','BD','BR','BA','R','GPAY','GP','PR') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM ONE.MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM'))))  and trans_ref_no is null and EXTRACT(YEAR FROM  Trans_Date) < EXTRACT(YEAR FROM  SYSDATE)";

                    string analysisQry = "Select Transaction_Type as TransactionType,Sub_Type as SubType, Where_Condition as WhereCondition, Serial  from one.analysis_config order by Serial";
                    transactionAnalysesList = connection.Query<TransactionAnalysis>(analysisQry).ToList();
                    foreach (var item in transactionAnalysesList)
                    {
                        DateTime opDate = DateTime.Now.AddMonths(-3);
                        string captionName = null;
                        string query = null;
                        for (int i = 1; i <= 5; i++)
                        {
                            int year = opDate.Year;
                            int month = opDate.Month;
                            captionName = opDate.ToString("MMM") + "-" + (year % 100);

                            if (i == 1)
                            {
                                item.CaptionOne = captionName;
                                item.MonthDaysOne = DateTime.DaysInMonth(year, month);
                                query = "Select  NVL(Sum(Msg_Amt),0)  as VolumeOne, count(*) as CountOne from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP','CM','AMBD','ABD','ABR','ABM','ABMC') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM','CM','AMBD','ABD','ABR','ABM','ABMC')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " and " + item.WhereCondition;
                                var result = connection.Query<dynamic>(query).FirstOrDefault();
                                item.VolumeOne = Convert.ToDouble(result.VOLUMEONE);
                                item.CountOne = Convert.ToInt32(result.COUNTONE);
                                opDate = opDate.AddMonths(1);
                            }
                            else if (i == 2)
                            {
                                item.CaptionTwo = captionName;
                                item.MonthDaysTwo = DateTime.DaysInMonth(year, month);
                                query = "Select  NVL(Sum(Msg_Amt),0)  as VolumeTwo, count(*) as CountTwo from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP','CM','AMBD','ABD','ABR','ABM','ABMC') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM','CM','AMBD','ABD','ABR','ABM','ABMC')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " and " + item.WhereCondition;
                                var result = connection.Query<dynamic>(query).FirstOrDefault();
                                item.VolumeTwo = Convert.ToDouble(result.VOLUMETWO);
                                item.CountTwo = Convert.ToInt32(result.COUNTTWO);
                                opDate = opDate.AddMonths(1);
                            }
                            else if (i == 3)
                            {
                                item.CaptionThree = captionName;
                                item.MonthDaysThree = DateTime.DaysInMonth(year, month);
                                query = "Select  NVL(Sum(Msg_Amt),0)  as VolumeThree, count(*) as CountThree from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP','CM','AMBD','ABD','ABR','ABM','ABMC') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM','CM','AMBD','ABD','ABR','ABM','ABMC')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " and " + item.WhereCondition;
                                var result = connection.Query<dynamic>(query).FirstOrDefault();
                                item.VolumeThree = Convert.ToDouble(result.VOLUMETHREE);
                                item.CountThree = Convert.ToInt32(result.COUNTTHREE);
                                opDate = opDate.AddMonths(1);
                            }
                            else if (i == 4)
                            {
                                captionName = "1-" + DateTime.Now.Day.ToString() + DateTime.Now.ToString("MMM") + "-" + (year % 100);
                                item.CaptionFour = captionName;
                                item.MonthDaysFour = DateTime.Now.Day;
                                query = " Select  NVL(Sum(Msg_Amt),0)  as VolumeFour, count(*) as CountFour from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP','CM','AMBD','ABD','ABR','ABM','ABMC') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM','CM','AMBD','ABD','ABR','ABM','ABMC')))) and trans_ref_no is null and trunc(Trans_Date) between To_Date('" + (new DateTime(year, month, 1)).ToString("dd-MM-yyyy") + "','DD-MM-RRRR') and To_Date('" + DateTime.Now.ToString("dd-MM-yyyy") + "','DD-MM-RRRR') and " + item.WhereCondition;
                                var result = connection.Query<dynamic>(query).FirstOrDefault();
                                item.VolumeFour = Convert.ToDouble(result.VOLUMEFOUR);
                                item.CountFour = Convert.ToInt32(result.COUNTFOUR);
                            }
                            else 
                            {
                                captionName = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MMM") + "-" + (year % 100);
                                item.CaptionFive = captionName;
                                query = "Select  NVL(Sum(Msg_Amt),0)  as VolumeFive, count(*) as CountFive from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP','CM','AMBD','ABD','ABR','ABM','ABMC') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM','CM','AMBD','ABD','ABR','ABM','ABMC')))) and trans_ref_no is null and trunc(Trans_Date)= To_Date('" + DateTime.Now.ToString("dd-MM-yyyy") + "','DD-MM-RRRR') and " + item.WhereCondition;
                                var result = connection.Query<dynamic>(query).FirstOrDefault();
                                item.VolumeFive = Convert.ToDouble(result.VOLUMEFIVE);
                                item.CountFive = Convert.ToInt32(result.COUNTFIVE);
                            }

                        }
                    }

                    //DateTime opDate = DateTime.Now.AddMonths(-3);                    
                    //string captionName = null;
                    //string query = null;
                    //for (int i = 1; i <= 5; i++)
                    //{
                    //    int year = opDate.Year;
                    //    int month = opDate.Month;
                    //    captionName = opDate.ToString("MMM") + "-" + (year % 100);

                    //    if (i == 1)
                    //    {
                    //        query = "select FROM_CAT_ID || '-' || TO_CAT_ID as SubType, '" + captionName + "' as Caption, sum(Msg_Amt) as VolumeOne,0 as VolumeTwo, 0 as VolumeThree, 0 as VolumeFour, 0 as VolumeFive  from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " GROUP BY FROM_CAT_ID || '-' || TO_CAT_ID";
                    //        opDate = opDate.AddMonths(1);
                    //    }
                    //    if (i == 2)
                    //    {
                    //        query = query + " Union all select FROM_CAT_ID || '-' || TO_CAT_ID as SubType, '" + captionName + "' as Caption, 0 as VolumeOne,sum(Msg_Amt) as VolumeTwo, 0 as VolumeThree, 0 as VolumeFour, 0 as VolumeFive from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " GROUP BY FROM_CAT_ID || '-' || TO_CAT_ID" + "";
                    //        opDate = opDate.AddMonths(1);
                    //    }
                    //    if (i == 3)
                    //    {
                    //        query = query + " Union all select FROM_CAT_ID || '-' || TO_CAT_ID as SubType, '" + captionName + "' as Caption, 0 as VolumeOne,0 as VolumeTwo,sum(Msg_Amt) as VolumeThree, 0 as VolumeFour, 0 as VolumeFive from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and trans_ref_no is null and EXTRACT(MONTH FROM Trans_Date)= " + month + " and EXTRACT(YEAR FROM Trans_Date)= " + year + " GROUP BY FROM_CAT_ID || '-' || TO_CAT_ID" + "";
                    //        opDate = opDate.AddMonths(1);
                    //    }
                    //    if (i == 4)
                    //    {
                    //        captionName = "1-" + DateTime.Now.Day.ToString() + DateTime.Now.ToString("MMM") + "-" + (year % 100);
                    //        query = query + " Union all select FROM_CAT_ID || '-' || TO_CAT_ID as SubType, '" + captionName + "' as Caption, 0 as VolumeOne,0 as VolumeTwo, 0 as VolumeThree,sum(Msg_Amt) as VolumeFour, 0 as VolumeFive from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and trans_ref_no is null and Trans_Date between To_Date('" + (new DateTime(year, month, 1)).ToShortDateString() + "','MM-DD-RRRR') and To_Date('" + DateTime.Now.ToShortDateString() + "','MM-DD-RRRR') GROUP BY FROM_CAT_ID || '-' || TO_CAT_ID" + "";
                    //    }
                    //    if (i == 5)
                    //    {
                    //        captionName = DateTime.Now.Day.ToString() + DateTime.Now.ToString("MMM") + "-" + (year % 100);
                    //        query = query + " Union all select FROM_CAT_ID || '-' || TO_CAT_ID as SubType, '" + captionName + "' as Caption, 0 as VolumeOne,0 as VolumeTwo, 0 as VolumeThree,0 as VolumeFour, sum(Msg_Amt) as VolumeFive from " + mainDbUser.DbUser + "GL_TRANS_MST WHERE (From_Cat_Id = 'C' OR To_Cat_Id = 'C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + mainDbUser.DbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + mainDbUser.DbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and trans_ref_no is null and Trans_Date= To_Date('" + DateTime.Now.ToShortDateString() + "','MM-DD-RRRR') GROUP BY FROM_CAT_ID || '-' || TO_CAT_ID" + "";
                    //    }

                    //}

                    //transactionAnalysesList = connection.Query<TransactionAnalysis>(query).ToList();

                    this.CloseConnection(connection);

                    return transactionAnalysesList;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object GetCampaignTypeDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();

                    //string query = @"Select  Sys_coa_code as ""Value"", CONCAT(COnCAT(CONCAT(CONCAT(CONCAT(CONCAT(coa_code, ' ('), Coa_desc), ')'), ' (Level-'), coa_level), ')') as ""Label"" from gl_coa where Acc_Type =" + "'" + assetType + "'" + " and coa_level =4";
                    string query = @"Select Campaign_Type as ""Value"" ,Campaign_Name as ""Label"" from " + mainDbUser.DbUser + "tbl_campaign_type";

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

        public List<ReferralCampaignDetails> GetReferralCampaignList(string tansactionType, string campaignType, string fromDate, string toDate)
        {
            List<ReferralCampaignDetails> result = new List<ReferralCampaignDetails>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType);
                    dyParam.Add("campaignType", OracleDbType.Varchar2, ParameterDirection.Input, campaignType == "null" ? "" : campaignType);
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<ReferralCampaignDetails>(connection, mainDbUser.DbUser + "RPT_RefCompaignDetails", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<BtclTelephoneBill> GetBtclTelephoneBill(string fromDate, string toDate)
        {
            List<BtclTelephoneBill> result = new List<BtclTelephoneBill>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                   
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<BtclTelephoneBill>(connection, mainDbUser.DbUser + "RPT_BtclTelephoneBill", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AdmissionFeePayment> GetadmissionFeePaymentList(string fromDate, string toDate)
        {
            List<AdmissionFeePayment> result = new List<AdmissionFeePayment>();
            try
            {
                using (var connection = this.GetConnection())
                {
                   
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));                  
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<AdmissionFeePayment>(connection, mainDbUser.DbUser + "RPT_AdmissionFeePayment", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DisbursementVoucher> GetDisbursementVoucherList(string option,string disTypeId, string fromDate, string toDate)
        {
            List<DisbursementVoucher> result = new List<DisbursementVoucher>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_Option", OracleDbType.Varchar2, ParameterDirection.Input, option);
                    dyParam.Add("V_DisTypeId", OracleDbType.Varchar2, ParameterDirection.Input, disTypeId);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<DisbursementVoucher>(connection, mainDbUser.DbUser + "RPT_DisbursementVoucher", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<B2BCollectionDtlSummary> GetB2BCollectionDtlSummaryList(string tansactionType, string fromCat, string toCat, string fromDate, string toDate)
        {
            List<B2BCollectionDtlSummary> result = new List<B2BCollectionDtlSummary>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("tansactionType", OracleDbType.Varchar2, ParameterDirection.Input, tansactionType);
                    dyParam.Add("fromCat", OracleDbType.Varchar2, ParameterDirection.Input, fromCat);
                    dyParam.Add("toCat", OracleDbType.Varchar2, ParameterDirection.Input, toCat);
                    dyParam.Add("FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<B2BCollectionDtlSummary>(connection, mainDbUser.DbUser + "RPT_B2BCollectionDtlSummary", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

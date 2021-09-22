using Dapper;
using MFS.TransactionService.Models;
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
    public interface IDisbursementRepository : IBaseRepository<TblDisburseCompanyInfo>
    {
        object GetDisbursementCompanyList();
        object GetMaxCompanyId();
        object getDisburseCompanyList();
        object getDisburseNameCodeList();
        object DataInsertToTransMSTandDTL(TblDisburseAmtDtlMake objTblDisburseAmtDtlMake);
        object AproveRefundDisburseAmount(string TransNo, string PhoneNo, string branchCode, TblDisburseCompanyInfo objTblDisburseCompanyInfo);
        object GetCompnayNameById(int companyId);
        object GetDisburseTypeList();
        object GetDisburseTypeListForOnline();
        object getBatchNo(int id, string tp);
        string Process(string batchno, string catId);
        object getCompanyAndBatchNoList(string forPosting);
        object getBatchNoDDLByCompanyId(int companyId, string fileUploadDate);
        object getCompanyAndBatchNoListByCmpId(string forPosting, int companyId);
        bool checkProcess(string batchno);
        List<TblDisburseInvalidData> getValidOrInvalidData(string processBatchNo, string validOrInvalid, string forPosting);
        string SendToPostingLevel(string processBatchNo, double totalSum);
        string AllSend(string processBatchNo, string brCode, string checkerId, double totalSum);
        string BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum);
        object GetAccountDetails(string accountNo);
        string GetTargetCatIdByCompany(string onlyCompanyName);
        TblDisburseCompanyInfo GetCompanyInfoByCompanyId(int companyId);
        object GetDisbursementAmountStatusList(int companyId);
        object GetDataForDisbursementDashboard(int companyId);
    }
    public class DisbursementRepository : BaseRepository<TblDisburseCompanyInfo>, IDisbursementRepository
    {
        MainDbUser mainDbUser = new MainDbUser();

        public object GetDisbursementCompanyList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();

                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<TblDisburseCompanyInfo>(connection, mainDbUser.DbUser + "SP_Get_DisbursementCompanyList", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public object GetMaxCompanyId()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = "Select Max(company_id) from" + mainDbUser.DbUser + "tbl_disburse_company_info";
                    var maxCompanyId = connection.QueryFirstOrDefault<int>(query);

                    connection.Close();
                    return maxCompanyId;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object getDisburseCompanyList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetDisburseCompanyDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetDisburseTypeList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetDisburseTypeDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetDisburseTypeListForOnline()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = "Select  Disburse_value as Value,CONCAT(CONCAT(CONCAT(disburse_type, ' ('), Disburse_value), ')') as Label from " + mainDbUser.DbUser + "TBL_DISBURSE_TYPE where Status = 'O'";
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

        public object getDisburseNameCodeList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetDisburseAccountDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object DataInsertToTransMSTandDTL(TblDisburseAmtDtlMake objTblDisburseAmtDtlMake)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(objTblDisburseAmtDtlMake.Tranno));
                    parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.AccNo);
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, objTblDisburseAmtDtlMake.AmountCr);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.BrCode);
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.CheckerId);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Disburse_Amount_Posting", param: parameter, commandType: CommandType.StoredProcedure);
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

        public object AproveRefundDisburseAmount(string TransNo, string PhoneNo, string branchCode, TblDisburseCompanyInfo objTblDisburseCompanyInfo)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(TransNo));
                    parameter.Add("V_FR_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, PhoneNo);
                    parameter.Add("V_BALANCE_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, "M");
                    parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, objTblDisburseCompanyInfo.refund_amt);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_TO_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    parameter.Add("V_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, "C");
                    parameter.Add("V_COMPANYID", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseCompanyInfo.CompanyId);
                    parameter.Add("V_CHECKEDUSER", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseCompanyInfo.entry_user);
                    parameter.Add("V_DISBURSETYPE", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseCompanyInfo.disburse_type);

                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Disburse_Amount_Refunding", param: parameter, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    string flag = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
                    string successOrErrorMsg = null;
                    if (flag == "0")
                    {
                        successOrErrorMsg = parameter.oracleParameters[6].Value != null ? parameter.oracleParameters[6].Value.ToString() : null;
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

        public object GetCompnayNameById(int companyId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = "Select Company_name from " + mainDbUser.DbUser + "tbl_disburse_company_info where company_Id=" + companyId;
                    var companyName = connection.QueryFirstOrDefault<string>(query);

                    return companyName;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object getBatchNo(int companyId, string disburseType)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = null;
                    query = "Select " + disburseType + "_ACC from " + mainDbUser.DbUser + "tbl_disburse_company_info where Company_Id=" + companyId;
                    var accountNo = connection.QueryFirstOrDefault<string>(query);



                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CompanyId", OracleDbType.Int16, ParameterDirection.Input, companyId);
                    parameter.Add("DisburseType", OracleDbType.Varchar2, ParameterDirection.Input, disburseType);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_GetBatchNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.AccountNo = accountNo;
                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string Process(string batchno, string catId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("BatchNumber", OracleDbType.Varchar2, ParameterDirection.Input, batchno);
                    parameter.Add("CategoryId", OracleDbType.Varchar2, ParameterDirection.Input, catId);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Disbursement_Process", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string successOrErrorMsg = null;
                    successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;

                    return successOrErrorMsg;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object getCompanyAndBatchNoList(string forPosting)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("forPosting", OracleDbType.Varchar2, ParameterDirection.Input, forPosting);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetCompanyAndBatchNoDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public object getBatchNoDDLByCompanyId(int companyId, string fileUploadDate)
        {
            try
            {
                //string query = null;
                using (var connection = this.GetConnection())
                {
                    //query = @"Select distinct batchno as Value ,batchno as Label from " + mainDbUser.DbUser + "tbl_disburse_invalid_data where organization_id =" + companyId + " and TO_DATE(EntryDate,'DD-MM-RRRR')=TO_DATE(" + Convert.ToDateTime(fileUploadDate) + ",'DD-MM-RRRR')";
                    //var result = connection.Query<CustomDropDownModel>(query);
                    //this.CloseConnection(connection);
                    //return result;
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("V_CompanyId", OracleDbType.Int32, ParameterDirection.Input, companyId);
                    parameter.Add("V_FileUploadDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fileUploadDate));
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GETBatchDDLByCmpIdDate", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object getCompanyAndBatchNoListByCmpId(string forPosting, int companyId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    //var parameter = new OracleDynamicParameters();
                    //parameter.Add("forPosting", OracleDbType.Varchar2, ParameterDirection.Input, forPosting);
                    //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    //var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_GetCompanyAndBatchNoDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    //connection.Close();

                    //return result;
                    string query = null;
                    if (forPosting == "null")
                    {
                        //query = @"Select distinct tmp.batchno as Value, CONCAT(CONCAT(CONCAT(c.company_name, ' ('), tmp.batchno), ')') as Label from " + mainDbUser.DbUser + "Tbl_Disburse_Tmp tmp inner join " + mainDbUser.DbUser + "tbl_disburse_company_info c on tmp.Organization_Id = c.company_id where tmp.c_m_status = 'M' and tmp.Batchno not in (Select distinct BatchNo from " + mainDbUser.DbUser + "Tbl_Disburse_Invalid_Data where Trans_No is not null) and c.company_id =" + companyId;
                        query = @"select distinct newTable.batchno as Value, CONCAT(CONCAT(CONCAT(c.company_name, ' ('), newTable.batchno), ')') as Label from ( (Select distinct tmp.batchno,tmp.organization_id,tmp.c_m_status from " + mainDbUser.DbUser + "Tbl_Disburse_Tmp tmp where tmp.organization_id=" + companyId + ") union (Select distinct vi.batchno,vi.organization_id,vi.c_m_status from " + mainDbUser.DbUser + "Tbl_Disburse_Invalid_Data vi where Trans_No is null and vi.organization_id=" + companyId + ") ) newTable  inner join " + mainDbUser.DbUser + "tbl_disburse_company_info c on newTable.Organization_Id = c.company_id where  batchNo not in (select batchNo from " + mainDbUser.DbUser + "tbl_disburse d where d.batchno= batchNo and d.organization_id = organization_id)";
                    }
                    else
                    {
                        query = @"Select distinct d.batchno as Value ,CONCAT(CONCAT(CONCAT(c.company_name, ' ('), d.batchno), ')') as Label from " + mainDbUser.DbUser + "Tbl_Disburse d inner join " + mainDbUser.DbUser + "tbl_disburse_company_info c on d.Organization_Id = c.company_id where d.status = 'N' and c.company_id =" + companyId;
                    }

                    var result = connection.Query<CustomDropDownModel>(query);
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool checkProcess(string batchNumber)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = "Select * from " + mainDbUser.DbUser + "tbl_disburse_invalid_data where BatchNo=" + "'" + batchNumber + "'";
                    var model = connection.QueryFirstOrDefault<dynamic>(query);
                    if (model != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TblDisburseInvalidData> getValidOrInvalidData(string processBatchNo, string validOrInvalid, string forPosting)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    List<TblDisburseInvalidData> result = new List<TblDisburseInvalidData>();
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                    parameter.Add("validOrInvalid", OracleDbType.Varchar2, ParameterDirection.Input, validOrInvalid);
                    parameter.Add("forPosting", OracleDbType.Varchar2, ParameterDirection.Input, forPosting);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    result = SqlMapper.Query<TblDisburseInvalidData>(connection, mainDbUser.DbUser + "SP_getValidOrInvalidData", param: parameter, commandType: CommandType.StoredProcedure).ToList();

                    connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string SendToPostingLevel(string processBatchNo, double totalSum)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    List<TblDisburseInvalidData> result = new List<TblDisburseInvalidData>();
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                    parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Disbursement_Posting", param: parameter, commandType: CommandType.StoredProcedure);

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

        public string AllSend(string processBatchNo, string brCode, string checkerId, double totalSum)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                    parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                    parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "E");
                    parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, brCode);
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, checkerId);
                    parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_Disbursement_AllSend", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string flag = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;
                    string successOrErrorMsg = null;
                    if (flag == "0")
                    {
                        successOrErrorMsg = parameter.oracleParameters[3].Value != null ? parameter.oracleParameters[3].Value.ToString() : null;
                    }
                    else
                    {
                        successOrErrorMsg = flag;

                    }
                    return successOrErrorMsg;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, checkerId);
                    parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                    SqlMapper.Query<dynamic>(connection, mainDbUser.DbUser + "SP_BatchDelete", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string successOrErrorMsg = null;
                    successOrErrorMsg = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                    return successOrErrorMsg;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetAccountDetails(string accountNo)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select Mphone from " + mainDbUser.DbUser + "reginfo where Cat_id='E' and Reg_status='P' and Mphone= " + "'" + accountNo + "'" + "";
                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public string GetTargetCatIdByCompany(string onlyCompanyName)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select TARGET_CAT_ID from " + mainDbUser.DbUser + "TBL_DISBURSE_COMPANY_INFO where COMPANY_NAME= " + "'" + onlyCompanyName + "'" + "";
                    var result = connection.Query<string>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public TblDisburseCompanyInfo GetCompanyInfoByCompanyId(int companyId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select Company_Id as CompanyId, Company_Name as CompanyName, Address, Phone, Fax, Sal_Acc as SalAcc,
                        REM_ACC as RemAcc, Cab_Acc as CabAcc, Cat_Acc as CatAcc, RWD_Acc as RwdAcc, Inc_Acc as IncAcc, Eft_Acc as EftAcc, Target_Cat_Id as TargetCatId,
                        (select Sum(Amount_cr) - sum(amount_dr) from " + mainDbUser.DbUser + "tbl_disburse_amt_dtl where company_id=c.company_id group by company_id) as Bala_nce from " + mainDbUser.DbUser +
                        "tbl_disburse_company_info c where company_id = " + companyId;
                    var result = connection.Query<TblDisburseCompanyInfo>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object GetDisbursementAmountStatusList(int companyId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select Entry_date as EntryDate,
                        Check_Time as CheckedDate, Amount_Cr as Amount,
                         case when status ='M' then 'Processing'
                           when status='C' then 'Approved'
                             else 'Rejected'
                           end OverallStatus from " + mainDbUser.DbUser + "tbl_disburse_amt_dtl_make where Company_id=" + companyId + " order by Entry_Date desc";
                    var result = connection.Query<dynamic>(query);
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }


        public object GetDataForDisbursementDashboard(int companyId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select  (Select  SUM(amount) from " + mainDbUser.DbUser + "tbl_disburse where organization_id =" + companyId + ") as Total, (Select  SUM(amount) from " + mainDbUser.DbUser + "tbl_disburse where organization_id = " + companyId + " and EXTRACT(month FROM ckeck_Date) = EXTRACT(month from SYSDATE)) as CurrentMonth, NVL((Select SUM(amount) from " + mainDbUser.DbUser + "tbl_disburse_invalid_data where organization_id = " + companyId + " and C_M_Status = 'V' and BatchNo not in (select BatchNo from " + mainDbUser.DbUser + "tbl_disburse where c_m_status in ('C', 'V', 'B'))),0)  as OnProcessAmt from dual";
                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

    }
}

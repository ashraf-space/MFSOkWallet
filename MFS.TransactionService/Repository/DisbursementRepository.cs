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
        object GetCompnayNameById(int companyId);
        object GetDisburseTypeList();
        object getBatchNo(int id, string tp);
        object Process(string batchno, string catId);
        object getCompanyAndBatchNoList(string forPosting);
        bool checkProcess(string batchno);
        List<TblDisburseInvalidData> getValidOrInvalidData(string processBatchNo, string validOrInvalid, string forPosting);
        string SendToPostingLevel(string processBatchNo, double totalSum);
        object AllSend(string processBatchNo, string brCode, string checkerId, double totalSum);
        object BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum);
        object GetAccountDetails(string accountNo);
    }
    public class DisbursementRepository : BaseRepository<TblDisburseCompanyInfo>, IDisbursementRepository
    {
        
        public object GetDisbursementCompanyList()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();

                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<TblDisburseCompanyInfo>(connection, "SP_Get_DisbursementCompanyList", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();

                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<TblDisburseCompanyInfo>(connection, "SP_Get_DisbursementCompanyList", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
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
                    string query = "Select Max(company_id) from tbl_disburse_company_info";
                    var maxCompanyId = connection.QueryFirstOrDefault<int>(query);

                    return maxCompanyId;
                }
                //string query = "Select Max(company_id) from tbl_disburse_company_info";
                //var maxCompanyId = connection.QueryFirstOrDefault<int>(query);

                //return maxCompanyId;
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
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseCompanyDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseCompanyDDL", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
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
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseTypeDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseTypeDDL", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
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
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseAccountDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDisburseAccountDDL", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
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

                    SqlMapper.Query<dynamic>(connection, "SP_Disburse_Amount_Posting", param: parameter, commandType: CommandType.StoredProcedure);
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
                //parameter.Add("V_TRANS_NO", OracleDbType.Double, ParameterDirection.InputOutput, Convert.ToDouble(objTblDisburseAmtDtlMake.Tranno));
                //parameter.Add("V_TO_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.AccNo);
                //parameter.Add("V_MSG_AMT", OracleDbType.Double, ParameterDirection.Input, objTblDisburseAmtDtlMake.AmountCr);
                //parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                //parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "S");
                //parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.BrCode);
                //parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, objTblDisburseAmtDtlMake.CheckerId);

                //SqlMapper.Query<dynamic>(connection, "SP_Disburse_Amount_Posting", param: parameter, commandType: CommandType.StoredProcedure);
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
                    string query = "Select Company_name from tbl_disburse_company_info where company_Id=" + companyId;
                    var companyName = connection.QueryFirstOrDefault<string>(query);

                    return companyName;
                }
                //string query = "Select Company_name from tbl_disburse_company_info where company_Id=" + companyId;
                //var companyName = connection.QueryFirstOrDefault<string>(query);

                //return companyName;
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        public object getBatchNo(int id, string tp)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CompanyId", OracleDbType.Int16, ParameterDirection.Input, id);
                    parameter.Add("DisburseType", OracleDbType.Varchar2, ParameterDirection.Input, tp);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(connection, "SP_GetBatchNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("CompanyId", OracleDbType.Int16, ParameterDirection.Input, id);
                //parameter.Add("DisburseType", OracleDbType.Varchar2, ParameterDirection.Input, tp);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<dynamic>(connection, "SP_GetBatchNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //connection.Close();

                //return result;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public object Process(string batchno, string catId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("BatchNumber", OracleDbType.Varchar2, ParameterDirection.Input, batchno);
                    parameter.Add("CategoryId", OracleDbType.Varchar2, ParameterDirection.Input, catId);
                    parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    SqlMapper.Query<dynamic>(connection, "SP_Disbursement_Process", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string successOrErrorMsg = null;
                    successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;

                    return successOrErrorMsg;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("BatchNumber", OracleDbType.Varchar2, ParameterDirection.Input, batchno);               
                //parameter.Add("CategoryId", OracleDbType.Varchar2, ParameterDirection.Input, catId);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //SqlMapper.Query<dynamic>(connection, "SP_Disbursement_Process", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();
                //string successOrErrorMsg = null;
                //successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;

                //return successOrErrorMsg;
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
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetCompanyAndBatchNoDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("forPosting", OracleDbType.Varchar2, ParameterDirection.Input, forPosting);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetCompanyAndBatchNoDDL", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
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
                    string query = "Select * from tbl_disburse_invalid_data where BatchNo=" + "'" + batchNumber + "'";
                    var model = connection.QueryFirstOrDefault<dynamic>(query);
                    if (model != null)
                        return true;
                    else
                        return false;
                }
                //string query = "Select * from tbl_disburse_invalid_data where BatchNo=" + "'" + batchNumber + "'";
                //var model = connection.QueryFirstOrDefault<dynamic>(query);
                //if (model != null)
                //    return true;
                //else
                //    return false;
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
                    result = SqlMapper.Query<TblDisburseInvalidData>(connection, "SP_getValidOrInvalidData", param: parameter, commandType: CommandType.StoredProcedure).ToList();

                    connection.Close();

                    return result;
                }
                //List<TblDisburseInvalidData> result = new List<TblDisburseInvalidData>();
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                //parameter.Add("validOrInvalid", OracleDbType.Varchar2, ParameterDirection.Input, validOrInvalid);
                //parameter.Add("forPosting", OracleDbType.Varchar2, ParameterDirection.Input, forPosting);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //result = SqlMapper.Query<TblDisburseInvalidData>(connection, "SP_getValidOrInvalidData", param: parameter, commandType: CommandType.StoredProcedure).ToList();

                //connection.Close();

                //return result;
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
                    SqlMapper.Query<dynamic>(connection, "SP_Disbursement_Posting", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;

                    return successOrErrorMsg;
                }
                //List<TblDisburseInvalidData> result = new List<TblDisburseInvalidData>();
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                //parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //SqlMapper.Query<dynamic>(connection, "SP_Disbursement_Posting", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();
                //string successOrErrorMsg = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;
               
                //return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public object AllSend(string processBatchNo, string brCode, string checkerId, double totalSum)
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
                    SqlMapper.Query<dynamic>(connection, "SP_Disbursement_AllSend", param: parameter, commandType: CommandType.StoredProcedure);

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

                //var parameter = new OracleDynamicParameters();
                //parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                //parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
                //parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //parameter.Add("V_FROM_CATID", OracleDbType.Varchar2, ParameterDirection.Input, "E");
                //parameter.Add("V_REF_PHONE", OracleDbType.Varchar2, ParameterDirection.Input, brCode);
                //parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, checkerId);
                //parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                //SqlMapper.Query<dynamic>(connection, "SP_Disbursement_AllSend", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();
                //string flag = parameter.oracleParameters[2].Value != null ? parameter.oracleParameters[2].Value.ToString() : null;
                //string successOrErrorMsg = null;
                //if (flag == "0")
                //{
                //    successOrErrorMsg = parameter.oracleParameters[3].Value != null ? parameter.oracleParameters[3].Value.ToString() : null;
                //}
                //else
                //{
                //    successOrErrorMsg = flag;
                //}
                //return successOrErrorMsg;


            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public object BatchDelete(string processBatchNo, string brCode, string checkerId, double totalSum)
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
                    SqlMapper.Query<dynamic>(connection, "SP_BatchDelete", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();
                    string successOrErrorMsg = null;
                    successOrErrorMsg = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                    return successOrErrorMsg;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("processBatchNo", OracleDbType.Varchar2, ParameterDirection.Input, processBatchNo);
                //parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                //parameter.Add("CheckedUser", OracleDbType.Varchar2, ParameterDirection.Input, checkerId);
                //parameter.Add("totalSum", OracleDbType.Double, ParameterDirection.Input, totalSum);
                //SqlMapper.Query<dynamic>(connection, "SP_BatchDelete", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();
                //string successOrErrorMsg = null;
                //successOrErrorMsg = parameter.oracleParameters[1].Value != null ? parameter.oracleParameters[1].Value.ToString() : null;

                //return successOrErrorMsg;


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
                    string query = @"Select Mphone from reginfo where Cat_id='E' and Reg_status='P' and Mphone= " + "'" + accountNo + "'" + "";
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

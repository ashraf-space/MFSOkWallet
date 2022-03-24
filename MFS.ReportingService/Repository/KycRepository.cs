using Dapper;
using MFS.DistributionService.Models;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Repository
{
    public interface IKycRepository : IBaseRepository<RegistrationReport>
    {
        object GetAccountCategory();
        List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory, string accCategorySub);
        List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options);
        List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory, string accCategorySub);
        List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory);
        object GetClientInfoByMphone(string mphone);
		object GetMerchantKycInfoByMphone(string mPhone);
		object GetChainMerchantMphoneByCode(string chainMerchantCode);
		List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus, string fromHour, string toHour);
		List<RegInfoReport> GetRegReportByCategory(string fromDate, string toDate, string regSource, string status, string accCategory, string regStatus);
		object GetCurrentBalance(string mphone);
		object GetComissionBalance(string mphone);
		string GetCompanyNameByMphone(string mphone);
		List<CashBackReport> CashBackDetails(string mphone, string fromDate, string toDate, string cbType);
		object GetCashbackCategory();
		string GetCashBackName(string cbType);
		List<CashBackReport> CashBackSummaryReport(string mphone, string fromDate, string toDate, string cbType);
        List<SourceWiseRegistration> SourceWiseRegistration(string fromDate, string toDate, string regStatus, string status, string regSource, string branchCode);
        List<BranchWiseCount> BranchWiseCount(string branchCode, string userId, string option, string fromDate, string toDate);
		object GetSubAccountCategory();
		List<CommissionReport> CommissionReport(string mphone, string fromDate, string toDate);
		List<MerchantTransaction> GetTransactionById(string transNo, string refNo, string mphone);
		List<ChannelBankInfo> ChannelBankInfoReport(string fromDate, string toDate, string accNo, string catId);
        List<EmerchantSettlementInfo> GetEmerchantSettlementInfoList(string fromDate, string toDate);
        List<DormantAgent> GetDormantAgentList(string fromDate, string toDate, string type);
		List<MerchantBankInfo> MerchantBankInfoReport(string fromDate, string toDate, string accNo, string catId);
		List<KycCommission> GetRptkycCommissionsList(string reportName, string regFromDate, string regToDate, string commissionStatus, string authFromDate, string authToDate, string distributorNo, string agentNo, string transNo);
		BanglaQr GetBanglaQrInfo(string mphone, string catId);
	}
    public class KycRepository : BaseRepository<RegistrationReport>, IKycRepository
    {
        private readonly string dbUser;
        public KycRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }
        public object GetAccountCategory()
        {
            using (var connection = this.GetConnection())
            {
                string query = @"select t.catdesc as ""label"", t.catid as ""value"" from " + dbUser + "category t";

                var result = connection.Query<CustomDropDownModel>(query).ToList();

                this.CloseConnection(connection);
                return result;
            }

        }

        public List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory, string accCategorySub)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
				dyParam.Add("V_ACC_SUB_CAT", OracleDbType.Varchar2, ParameterDirection.Input, accCategorySub == "All" ? null : accCategorySub);
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                List<AgentInformation> result = SqlMapper.Query<AgentInformation>(connection, dbUser + "RPT_AGENT_INFORMATION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                this.CloseConnection(connection);
                this.CloseConnection(connection);
                return result;
            }
        }

		public object GetChainMerchantMphoneByCode(string chainMerchantCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.mphone as ""Mphone"" from one.merchant_config t where t.mcode = '"+chainMerchantCode+"'";

					var result = connection.Query<dynamic>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					var Heading = ((IDictionary<string, object>)result).Keys.ToArray();
					var details = ((IDictionary<string, object>)result);
					var values = details[Heading[0]];
					return values.ToString();
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory, string accCategorySub)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus);
                dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                dyParam.Add("BASEDON", OracleDbType.Varchar2, ParameterDirection.Input, basedOn);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
				dyParam.Add("ACCCATEGORYSUB", OracleDbType.Varchar2, ParameterDirection.Input, accCategorySub =="All"?null: accCategorySub);
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                List<RegistrationReport> result = SqlMapper.Query<RegistrationReport>(connection, dbUser + "RPT_REGISTRATIONDETAILS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                this.CloseConnection(connection);
                return result;
            }

        }

        public List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                List<RegistrationSummary> result = SqlMapper.Query<RegistrationSummary>(connection, dbUser + "RPT_REGISTRATIONSUMMARY", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                this.CloseConnection(connection);
                return result;
            }

        }


        public List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus);
                dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                dyParam.Add("ACCNO", OracleDbType.Varchar2, ParameterDirection.Input, accNo);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
                dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                List<KycBalance> result = SqlMapper.Query<KycBalance>(connection, dbUser + "RPT_KYCBALANCE", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                this.CloseConnection(connection);
                return result;
            }

        }

        public object GetClientInfoByMphone(string mphone)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select * from " + dbUser + "RegInfoView where mphone= '" + mphone + "' ";

                    var result = connection.Query<Reginfo>(query).FirstOrDefault();

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

		public object GetMerchantKycInfoByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select t.company_name as ""CompanyName""  from " + dbUser + "reginfo t where t.mphone= '" + mPhone + "' ";

					var result = connection.Query<Reginfo>(query).FirstOrDefault();

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

		public List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo, string regStatus, string fromHour, string toHour)
		{
			try
			{				
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input,fromDate=="null"?DateTime.Now: Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input,toDate== "null" ? DateTime.Now:Convert.ToDateTime(toDate));
					dyParam.Add("V_CATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, category=="null"?null:category);
					dyParam.Add("V_REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus == "null" ? null : regStatus);
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, accNo=="null"?null:accNo);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<OnlineRegistration> result = SqlMapper.Query<OnlineRegistration>(connection, dbUser + "RPT_ONLINE_REG", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		}

		public List<RegInfoReport> GetRegReportByCategory(string fromDate, string toDate, string regSource, string status, string accCategory, string regStatus)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
					dyParam.Add("V_REGSOURCE", OracleDbType.Varchar2, ParameterDirection.Input, regSource);
					dyParam.Add("V_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, status);
					dyParam.Add("V_REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<RegInfoReport> result = SqlMapper.Query<RegInfoReport>(connection, dbUser + "RPT_REG_DTL_BY_CAT", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GetCurrentBalance(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select one.func_get_balance('"+mphone+"','M') as Balance from dual";

					var result = connection.Query<dynamic>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					var Heading = ((IDictionary<string, object>)result).Keys.ToArray();
					var details = ((IDictionary<string, object>)result);
					var values = details[Heading[0]];					
					return values;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GetComissionBalance(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select round(t.balance_c,2) from one.reginfo t where t.mphone = '" + mphone+"'";

					var result = connection.Query<dynamic>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					var Heading = ((IDictionary<string, object>)result).Keys.ToArray();
					var details = ((IDictionary<string, object>)result);
					var values = details[Heading[0]];
					return values;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetCompanyNameByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select NVL(NVL(COMPANY_NAME, NAME), ACCOUNT_NAME) as ""Name""  from " + dbUser + "reginfo t where t.mphone= '" + mphone + "' ";
					var result = connection.Query<dynamic>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					var Heading = ((IDictionary<string, object>)result).Keys.ToArray();
					var details = ((IDictionary<string, object>)result);
					var values = details[Heading[0]];
					return values.ToString();
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<CashBackReport> CashBackDetails(string mphone, string fromDate, string toDate, string cbType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_CB_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, cbType == "A" ? null : cbType);					
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CashBackReport> result = SqlMapper.Query<CashBackReport>(connection, dbUser + "RPT_CASHBACK", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object GetCashbackCategory()
		{
			using (var connection = this.GetConnection())
			{
				string query = @"SELECT t.init_type as label, t.from_cat_id as value FROM ONE.TRANS_TYPE t WHERE FROM_CAT_ID LIKE 'CB%'";

				var result = connection.Query<CustomDropDownModel>(query).ToList() ;

				this.CloseConnection(connection);

				CustomDropDownModel customDropDownModel = new CustomDropDownModel
				{
					label = "All",
					value = "A"
				};
				result.Add(customDropDownModel);
				return result;
			}

		}

		public string GetCashBackName(string cbType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"SELECT T.INIT_TYPE FROM ONE.TRANS_TYPE T WHERE FROM_CAT_ID = '"+cbType+"'";

					var result = connection.Query<dynamic>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					var Heading = ((IDictionary<string, object>)result).Keys.ToArray();
					var details = ((IDictionary<string, object>)result);
					var values = details[Heading[0]];
					return values.ToString();
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<CashBackReport> CashBackSummaryReport(string mphone, string fromDate, string toDate, string cbType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_CB_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, cbType == "A" ? null : cbType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CashBackReport> result = SqlMapper.Query<CashBackReport>(connection, dbUser + "RPT_CASHBACK_SUM", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public List<SourceWiseRegistration> SourceWiseRegistration(string fromDate, string toDate, string regStatus, string status, string regSource, string branchCode)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus);
                    dyParam.Add("V_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, status);
                    dyParam.Add("V_REGSOURCE", OracleDbType.Varchar2, ParameterDirection.Input, regSource);
                    dyParam.Add("V_BRANCHCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    List<SourceWiseRegistration> result = SqlMapper.Query<SourceWiseRegistration>(connection, dbUser + "RPT_SourceWiseRegistration", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BranchWiseCount> BranchWiseCount(string branchCode, string userId, string option, string fromDate, string toDate)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_BRANCHCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    dyParam.Add("V_USERID", OracleDbType.Varchar2, ParameterDirection.Input, userId);
                    dyParam.Add("V_OPTION", OracleDbType.Varchar2, ParameterDirection.Input, option);
                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    List<BranchWiseCount> result = SqlMapper.Query<BranchWiseCount>(connection, dbUser + "RPT_BranchWiseCount", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public object GetSubAccountCategory()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.acc_type_code_sub as ""value"", t.name ""label"" from ONE.PRODUCT_SETUP_SUB t";

					var result = connection.Query<CustomDropDownModel>(query).ToList();

					this.CloseConnection(connection);
					return result;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public List<CommissionReport> CommissionReport(string mphone, string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();					
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CommissionReport> result = SqlMapper.Query<CommissionReport>(connection, dbUser + "RPT_COMMISSION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<MerchantTransaction> GetTransactionById(string transNo, string refNo, string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo == "undefined" ? null : transNo);
					dyParam.Add("V_REFNO", OracleDbType.Varchar2, ParameterDirection.Input, refNo == "undefined" ? null : refNo);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<MerchantTransaction> result = SqlMapper.Query<MerchantTransaction>(connection, dbUser + "GET_MER_TRANBYID", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<ChannelBankInfo> ChannelBankInfoReport(string fromDate, string toDate, string accNo, string catId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, accNo == "null" ? null : accNo);
					dyParam.Add("V_CAT_ID", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<ChannelBankInfo> result = SqlMapper.Query<ChannelBankInfo>(connection, dbUser + "RPT_CHANNEL_BANK", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public List<EmerchantSettlementInfo> GetEmerchantSettlementInfoList(string fromDate, string toDate)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    List<EmerchantSettlementInfo> result = SqlMapper.Query<EmerchantSettlementInfo>(connection, dbUser + "RPT_EmerchantSettlementInfo", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DormantAgent> GetDormantAgentList(string fromDate, string toDate, string type)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_FromDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("V_ToDate", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("V_Type", OracleDbType.Varchar2, ParameterDirection.Input, type == "null" ? "" : type);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    List<DormantAgent> result = SqlMapper.Query<DormantAgent>(connection, dbUser + "RPT_DormantAgent", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public List<MerchantBankInfo> MerchantBankInfoReport(string fromDate, string toDate, string accNo, string catId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_CAT_ID", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, accNo == "null" ? null : accNo);
					dyParam.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<MerchantBankInfo> result = SqlMapper.Query<MerchantBankInfo>(connection, dbUser + "RPT_MERCHANT_BANK", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<KycCommission> GetRptkycCommissionsList(string reportName, string regFromDate, string regToDate, string commissionStatus, string authFromDate, string authToDate, string distributorNo, string agentNo, string transNo)
		{
			using (var connection = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();

				dyParam.Add("REPORTNAME", OracleDbType.Varchar2, ParameterDirection.Input, reportName);
				dyParam.Add("REGFROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(regFromDate));
				dyParam.Add("REGTODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(regToDate));
				dyParam.Add("COMMISSIONSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, commissionStatus == "null" ? null : commissionStatus);
				dyParam.Add("AUTHFROMDATE", OracleDbType.Date, ParameterDirection.Input, authFromDate == "null" ? DateTime.Now.AddYears(-20) : Convert.ToDateTime(authFromDate));
				dyParam.Add("AUTHTODATE", OracleDbType.Date, ParameterDirection.Input, authFromDate == "null" ? DateTime.Now : Convert.ToDateTime(authToDate));
				dyParam.Add("V_DISTRIBUTORNO", OracleDbType.Varchar2, ParameterDirection.Input, distributorNo == "null" ? null : distributorNo);
				dyParam.Add("V_AGENTNO", OracleDbType.Varchar2, ParameterDirection.Input, agentNo == "null" ? null : agentNo);
				dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo == "null" ? null : transNo);

				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

				List<KycCommission> result = SqlMapper.Query<KycCommission>(connection, dbUser + "RPT_KYC_COMMISSION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				return result;
			}
		}

		public BanglaQr GetBanglaQrInfo(string mphone, string catId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("V_CAT", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					BanglaQr result = SqlMapper.Query<BanglaQr>(connection, dbUser + "SP_GET_BANQR_INFO", param: dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}

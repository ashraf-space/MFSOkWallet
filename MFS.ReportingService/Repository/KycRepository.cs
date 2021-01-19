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
        List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory);
        List<RegistrationSummary> GetRegistrationReportSummary(string fromDate, string toDate, string options);
        List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory);
        List<KycBalance> GetKycBalance(string regStatus, string fromDate, string toDate, string accNo, string options, string accCategory);
        object GetClientInfoByMphone(string mphone);
		object GetMerchantKycInfoByMphone(string mPhone);
		object GetChainMerchantMphoneByCode(string chainMerchantCode);
		List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo);
		List<RegInfoReport> GetRegReportByCategory(string fromDate, string toDate, string regSource, string status, string accCategory, string regStatus);
		object GetCurrentBalance(string mphone);
		object GetComissionBalance(string mphone);
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

        public List<AgentInformation> GetAgentInfo(string fromDate, string toDate, string options, string accCategory)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
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

		public List<RegistrationReport> GetRegistrationReports(string regStatus, string fromDate, string toDate, string basedOn, string options, string accCategory)
        {
            using (var connection = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, regStatus);
                dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                dyParam.Add("BASEDON", OracleDbType.Varchar2, ParameterDirection.Input, basedOn);
                dyParam.Add("OPTIONS", OracleDbType.Varchar2, ParameterDirection.Input, options);
                dyParam.Add("ACCCATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, accCategory);
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

		public List<OnlineRegistration> GetOnlineRegReport(string fromDate, string toDate, string category, string accNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input,fromDate=="null"?DateTime.Now: Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input,toDate== "null" ? DateTime.Now:Convert.ToDateTime(toDate));
					dyParam.Add("CATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, category=="null"?null:category);
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
	}
}

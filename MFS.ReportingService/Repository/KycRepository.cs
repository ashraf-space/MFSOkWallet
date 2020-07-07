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
	}
	public class KycRepository : BaseRepository<RegistrationReport>, IKycRepository
	{
		
		public object GetAccountCategory()
		{
			using (var connection = this.GetConnection())
			{
				string query = @"select t.catdesc as ""label"", t.catid as ""value"" from category t";

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

				List<AgentInformation> result = SqlMapper.Query<AgentInformation>(connection, "RPT_AGENT_INFORMATION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				this.CloseConnection(connection);
				return result;
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

				List<RegistrationReport> result = SqlMapper.Query<RegistrationReport>(connection, "RPT_REGISTRATIONDETAILS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

				List<RegistrationSummary> result = SqlMapper.Query<RegistrationSummary>(connection, "RPT_REGISTRATIONSUMMARY", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

				List<KycBalance> result = SqlMapper.Query<KycBalance>(connection, "RPT_KYCBALANCE", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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
					string query = @"Select * from RegInfoView where mphone= '" + mphone + "' ";

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
	}
}

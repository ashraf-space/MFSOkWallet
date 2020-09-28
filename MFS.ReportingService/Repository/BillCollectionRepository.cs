using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;

namespace MFS.ReportingService.Repository
{
	public interface IBillCollectionRepository : IBaseRepository<BillCollection>
	{
		List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
		List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate);
		List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate);
		List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
		List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
		List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType);
	}
	public class BillCollectionRepository : BaseRepository<BillCollection>, IBillCollectionRepository
	{
		private readonly string dbUser;
		public BillCollectionRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}

		

		public List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CreditCardReport> result = SqlMapper.Query<CreditCardReport>(connection, dbUser + "RPT_CREDITCARDINFO_BEFTN", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CreditCardReport> result = SqlMapper.Query<CreditCardReport>(connection, dbUser + "RPT_CREDITCARDINFO", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("UTILITY", OracleDbType.Varchar2, ParameterDirection.Input, utility);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input,Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, gateway == "All" ? null : gateway);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<BillCollection> result = SqlMapper.Query<BillCollection>(connection, dbUser + "RPT_DESCODPDCBILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					//List<BillCollection> result = null;
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("UTILITY", OracleDbType.Varchar2, ParameterDirection.Input, utility);
					dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
					dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
					dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, gateway == "All" ? null : gateway);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<JalalabadGasBillPayment> result = SqlMapper.Query<JalalabadGasBillPayment>(connection, dbUser + "RPT_JGTD_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					//List<BillCollection> result = null;
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("UTILITY", OracleDbType.Varchar2, ParameterDirection.Input, utility);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, gateway == "All" ? null : gateway);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<WasaBillPayment> result = SqlMapper.Query<WasaBillPayment>(connection, dbUser + "RPT_WASA_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					//List<BillCollection> result = null;
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("STUCODE", OracleDbType.Varchar2, ParameterDirection.Input, studentId == "null" ? null : studentId);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, null);
					dyParam.Add("INSTCODE", OracleDbType.Varchar2, ParameterDirection.Input, instituteId == "null" ? null : instituteId);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<EdumanBillPayment> result = SqlMapper.Query<EdumanBillPayment>(connection, dbUser + "RPT_EDUMAN_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					//List<BillCollection> result = null;
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

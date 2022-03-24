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
		List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate, string branchCode);
		List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate, string branchCode);
		List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<EdumanBillPayment> EdumanBillReport(string studentId, string fromDate, string toDate, string instituteId, string dateType, string catType);
		List<NescoRpt> NescoDailyDetailReport(string transNo, string fromDate, string toDate, string branchCode);
		List<NescoRpt> NescoDSSReport(string fromDate, string toDate);
		List<NescoRpt> NescoMDSReport(string fromDate, string toDate);
		List<NidBill> GetNidReports(string transNo, string fromDate, string toDate, string branchCode);
		List<LankaBanglaCredit> GetLbcReports(string transNo, string fromDate, string toDate, string branchCode);
		List<BillCollection> GetWzpdcl(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<MrpModel> GetMrpReport(string transNo, string fromDate, string toDate, string branchCode, string catType);
		List<NescoPrepaid> NescoPrepaidReport(string transNo, string fromDate, string toDate, string branchCode);
		List<MmsReport> GetMmsReport(string fromDate, string toDate, string transNo, string memberId, string orgId, string branchCode);
		List<CreditCardReport> GetCreditPaymentReportOblOnline(string transNo, string fromDate, string toDate, string branchCode);
		List<GpReport> GetGpTransSummaryReport(string fromDate, string toDate, string selectedReportType, string selectedDateType);
		List<FosterPayment> GetFosterIspReport(string transNo, string fromDate, string toDate, string branchCode);
		List<DescoPrepaid> GetDescoPrepaidReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<BankConnectivity> GetbankConnectivitiesReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType);
		List<BankConnectivity> GetbankConnectivitiesSumReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType);
		List<BgdclBillPayment> GetBgdclReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<KwasaBillPayment> GetKwasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<WzpdclBillPayment> GetWzpdclPoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<Ekpay> GetEkpaysConnectivitiesReport(string fromDate, string toDate, string dateType);
		List<PgclBillReport> GetPgclBillreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<LandTaxBill> GetLandTaxreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode);
		List<Ivac> GetSslReport(string fromDate, string toDate, string reportType);
		List<Rlic> GetrlicsReport(string fromDate, string toDate);
	}
	public class BillCollectionRepository : BaseRepository<BillCollection>, IBillCollectionRepository
	{
		private readonly string dbUser;
		public BillCollectionRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}

		

		public List<CreditCardReport> GetCreditBeftnPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim() == "null" ? null : transNo.Trim());
					//dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
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

		public List<CreditCardReport> GetCreditPaymentReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim() == "null" ? null : transNo.Trim());
					//dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
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

		public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
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

		public List<JalalabadGasBillPayment> GetJgtdReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("UTILITY", OracleDbType.Varchar2, ParameterDirection.Input, utility);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input,Convert.ToDateTime(toDate));
					dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, gateway == "All" ? null : gateway);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<JalalabadGasBillPayment> result = SqlMapper.Query<JalalabadGasBillPayment>(connection,
						dbUser + "RPT_JGTD_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();					
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<WasaBillPayment> GetWasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);

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

		public List<NescoRpt> NescoDailyDetailReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo=="null"?null:transNo);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<NescoRpt> result = SqlMapper.Query<NescoRpt>(connection, dbUser + "RPT_NESCO_DDR", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<NescoRpt> NescoDSSReport(string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();					
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					List<NescoRpt> result = SqlMapper.Query<NescoRpt>(connection, dbUser + "RPT_NESCO_DSS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<NescoRpt> NescoMDSReport(string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<NescoRpt> result = SqlMapper.Query<NescoRpt>(connection, dbUser + "RPT_NESCO_MDS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();

					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<NidBill> GetNidReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim()=="null"?null:transNo.Trim());
					//dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<NidBill> result = SqlMapper.Query<NidBill>(connection, dbUser + "RPT_NID_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<LankaBanglaCredit> GetLbcReports(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim() == "null" ? null : transNo.Trim());
					//dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<LankaBanglaCredit> result = SqlMapper.Query<LankaBanglaCredit>(connection, dbUser + "RPT_LBC_BILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<BillCollection> GetWzpdcl(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<BillCollection> result = SqlMapper.Query<BillCollection>(connection, dbUser + "RPT_WZPDCL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<MrpModel> GetMrpReport(string transNo, string fromDate, string toDate, string branchCode, string catType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));			
					dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType == "All" ? null : catType);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim() == "null" ? null : transNo);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<MrpModel> result = SqlMapper.Query<MrpModel>(connection, dbUser + "RPT_MRP", param: dyParam, commandType: CommandType.StoredProcedure).ToList();					
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<NescoPrepaid> NescoPrepaidReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo == "null" ? null : transNo);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<NescoPrepaid> result = SqlMapper.Query<NescoPrepaid>(connection, dbUser + "RPT_NESCO_PREPAID", param: dyParam, commandType: CommandType.StoredProcedure).ToList();

					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<MmsReport> GetMmsReport(string fromDate, string toDate, string transNo, string memberId, string orgId, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANSNO", OracleDbType.Varchar2, ParameterDirection.Input, transNo == "null" ? null : transNo);
					dyParam.Add("V_MEMBERID", OracleDbType.Varchar2, ParameterDirection.Input, memberId == "null" ? null : memberId);
					dyParam.Add("V_ORGID", OracleDbType.Varchar2, ParameterDirection.Input, orgId == "null" ? null : orgId);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<MmsReport> result = SqlMapper.Query<MmsReport>(connection, dbUser + "RPT_MMS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<CreditCardReport> GetCreditPaymentReportOblOnline(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo=="null"?null:transNo);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<CreditCardReport> result = SqlMapper.Query<CreditCardReport>(connection, dbUser + "RPT_CC_OBL_ONLINE", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<GpReport> GetGpTransSummaryReport(string fromDate, string toDate, string selectedReportType, string selectedDateType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("RPTYPE", OracleDbType.Varchar2, ParameterDirection.Input, selectedReportType);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, selectedDateType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<GpReport> result = SqlMapper.Query<GpReport>(connection, dbUser + "RPT_GP", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<FosterPayment> GetFosterIspReport(string transNo, string fromDate, string toDate, string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var date = Convert.ToDateTime(fromDate);
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TRANS_NO", OracleDbType.Varchar2, ParameterDirection.Input, transNo.Trim() == "null" ? null : transNo.Trim());
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<FosterPayment> result = SqlMapper.Query<FosterPayment>(connection, dbUser + "RPT_FPISP", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<DescoPrepaid> GetDescoPrepaidReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<DescoPrepaid> result = SqlMapper.Query<DescoPrepaid>(connection, dbUser + "RPT_DESCO_PREPAID", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<BankConnectivity> GetbankConnectivitiesReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_FROM_CAT", OracleDbType.Varchar2, ParameterDirection.Input, fromCatId);
					dyParam.Add("V_TO_CAT", OracleDbType.Varchar2, ParameterDirection.Input, toCatId);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<BankConnectivity> result = SqlMapper.Query<BankConnectivity>(connection, dbUser + "RPT_BANK_CONNECTIVITIES", param: dyParam, commandType: CommandType.StoredProcedure).ToList();				
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<BankConnectivity> GetbankConnectivitiesSumReport(string fromDate, string toDate, string fromCatId, string toCatId, string dateType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_FROM_CAT", OracleDbType.Varchar2, ParameterDirection.Input, fromCatId);
					dyParam.Add("V_TO_CAT", OracleDbType.Varchar2, ParameterDirection.Input, toCatId);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<BankConnectivity> result = SqlMapper.Query<BankConnectivity>(connection, dbUser + "RPT_BANK_CONNECT_SUM", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<BgdclBillPayment> GetBgdclReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<BgdclBillPayment> result = SqlMapper.Query<BgdclBillPayment>(connection, dbUser + "RPT_BGDCL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<KwasaBillPayment> GetKwasaReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<KwasaBillPayment> result = SqlMapper.Query<KwasaBillPayment>(connection, dbUser + "RPT_KWASA", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<WzpdclBillPayment> GetWzpdclPoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<WzpdclBillPayment> result = SqlMapper.Query<WzpdclBillPayment>(connection, dbUser + "RPT_WZPDCL_PO", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<Ekpay> GetEkpaysConnectivitiesReport(string fromDate, string toDate, string dateType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, null);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<Ekpay> result = SqlMapper.Query<Ekpay>(connection, dbUser + "RPT_EKPAY_CON", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<PgclBillReport> GetPgclBillreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<PgclBillReport> result = SqlMapper.Query<PgclBillReport>(connection, dbUser + "RPT_PGCL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<LandTaxBill> GetLandTaxreport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType, string branchCode)
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
					dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "0000" ? null : branchCode);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					List<LandTaxBill> result = SqlMapper.Query<LandTaxBill>(connection, dbUser + "RPT_LAND_TAX", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

		public List<Ivac> GetSslReport(string fromDate, string toDate, string reportType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					List<Ivac> result = SqlMapper.Query<Ivac>(connection, dbUser + "RPT_IVAC", param: dyParam, commandType: CommandType.StoredProcedure).ToList();					
					this.CloseConnection(connection);
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<Rlic> GetrlicsReport(string fromDate, string toDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					//dyParam.Add("V_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					List<Rlic> result = SqlMapper.Query<Rlic>(connection, dbUser + "RPT_RLIC", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
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

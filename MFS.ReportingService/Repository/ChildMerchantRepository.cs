using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Repository
{
	public interface IChildMerchantRepository : IBaseRepository<ChildMerchantTransaction>
	{
		List<ChildMerchantTransaction> GetChildMerchantTransactionReport(string mphone, string fromDate, string toDate);
		List<MerchantTransactionSummary> ChainMerTransSummReportByTd(string mphone, string fromDate, string toDate);
		List<OutletSummaryTransaction> ChainMerTransSummReportByOutlet(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType);
		List<OutletDailySummaryTransaction> ChildMerDailySumReport(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType);
	}
	public class ChildMerchantRepository : BaseRepository<ChildMerchantTransaction>, IChildMerchantRepository
	{
		private readonly string dbUser;
		public ChildMerchantRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}

		public List<OutletSummaryTransaction> ChainMerTransSummReportByOutlet(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType)
		{
			List<OutletSummaryTransaction> result = new List<OutletSummaryTransaction>();

			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("CHILDMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, childMerchantCode);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					//dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					result = SqlMapper.Query<OutletSummaryTransaction>(connection, dbUser + "RPT_CHILD_OUTLETSUMMARYTRANSACTION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
				}

			}
			catch (Exception e)
			{

				throw;
			}
			return result;
		}

		public List<MerchantTransactionSummary> ChainMerTransSummReportByTd(string mphone, string fromDate, string toDate)
		{
			List<MerchantTransactionSummary> result = new List<MerchantTransactionSummary>();

			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					result = SqlMapper.Query<MerchantTransactionSummary>(connection, dbUser + "RPT_CHILD_MER_OSTR_TD", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
				}

			}
			catch (Exception e)
			{

				throw;
			}
			return result;
		}

		public List<OutletDailySummaryTransaction> ChildMerDailySumReport(string mphone, string childMerchantCode, string fromDate, string toDate, string dateType)
		{
			List<OutletDailySummaryTransaction> result = new List<OutletDailySummaryTransaction>();

			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("CHILDMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, childMerchantCode);
					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					//dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					result = SqlMapper.Query<OutletDailySummaryTransaction>(connection, dbUser + "RPT_CHILD_OUTDAILY_SUMTRANS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
				}

			}
			catch (Exception e)
			{

				throw;
			}
			return result;
		}

		public List<ChildMerchantTransaction> GetChildMerchantTransactionReport(string mphone, string fromDate, string toDate)
		{
			List<ChildMerchantTransaction> result = new List<ChildMerchantTransaction>();

			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();

					dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
					dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

					result = SqlMapper.Query<ChildMerchantTransaction>(connection, dbUser + "RPT_CHILD_MER_ODTR", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);
				}

			}
			catch (Exception e)
			{

				throw;
			}
			return result;
		}
	}
}

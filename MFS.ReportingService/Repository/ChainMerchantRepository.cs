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
    public interface IChainMerchantRepository : IBaseRepository<OutletDetailsTransaction>
    {
        List<OutletDetailsTransaction> GetOutletDetailsTransactionList(string chainMerchantCode,string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType);
        List<OutletSummaryTransaction> GetOutletSummaryTransactionList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType);
        List<OutletDailySummaryTransaction> GetOutletDailySummaryTransList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string fromDate, string toDate, string dateType);
        List<OutletSummaryTransaction> GetOutletToParentTransSummaryList(string chainMerchantCode, string chainMerchantNo, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType);
		string GetChainMerchantCodeByMphone(string mphone);
	}
    public class ChainMerchantRepository:BaseRepository<OutletDetailsTransaction>,IChainMerchantRepository
	{
        private static string dbUser;
        public ChainMerchantRepository(MainDbUser mainDbUser)
        {
            dbUser = mainDbUser.DbUser;
        }

        public List<OutletDetailsTransaction> GetOutletDetailsTransactionList(string chainMerchantCode,string outletAccNo, string outletCode, string reportType,
            string reportViewType, string fromDate, string toDate, string dateType)
        {
            List<OutletDetailsTransaction> result = new List<OutletDetailsTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("CHAINMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, chainMerchantCode);
                    dyParam.Add("OUTLETACCNO", OracleDbType.Varchar2, ParameterDirection.Input, outletAccNo);
                    dyParam.Add("OUTLETCODE", OracleDbType.Varchar2, ParameterDirection.Input, outletCode);
                    dyParam.Add("REPORTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
                    dyParam.Add("REPORTVIEWTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportViewType);
                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<OutletDetailsTransaction>(connection, dbUser + "RPT_OUTLETDETAILSTRANSACTION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public List<OutletSummaryTransaction> GetOutletSummaryTransactionList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType)
        {
            List<OutletSummaryTransaction> result = new List<OutletSummaryTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("CHAINMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, chainMerchantCode);
                    dyParam.Add("OUTLETACCNO", OracleDbType.Varchar2, ParameterDirection.Input, outletAccNo);
                    dyParam.Add("OUTLETCODE", OracleDbType.Varchar2, ParameterDirection.Input, outletCode);
                    dyParam.Add("REPORTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
                    dyParam.Add("REPORTVIEWTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportViewType);
                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<OutletSummaryTransaction>(connection, dbUser + "RPT_OUTLETSUMMARYTRANSACTION", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public List<OutletDailySummaryTransaction> GetOutletDailySummaryTransList(string chainMerchantCode, string outletAccNo, string outletCode, string reportType, string fromDate, string toDate, string dateType)
        {
            List<OutletDailySummaryTransaction> result = new List<OutletDailySummaryTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("CHAINMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, chainMerchantCode);
                    dyParam.Add("OUTLETACCNO", OracleDbType.Varchar2, ParameterDirection.Input, outletAccNo);
                    dyParam.Add("OUTLETCODE", OracleDbType.Varchar2, ParameterDirection.Input, outletCode);
                    dyParam.Add("REPORTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<OutletDailySummaryTransaction>(connection, dbUser + "RPT_OUTLETDAILY_SUMMARYTRANS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public List<OutletSummaryTransaction> GetOutletToParentTransSummaryList(string chainMerchantCode, string chainMerchantNo, string outletAccNo, string outletCode, string reportType, string reportViewType, string fromDate, string toDate, string dateType)
        {
            List<OutletSummaryTransaction> result = new List<OutletSummaryTransaction>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("CHAINMERCHANTCODE", OracleDbType.Varchar2, ParameterDirection.Input, chainMerchantCode);
                    dyParam.Add("CHAINMERCHANTNO", OracleDbType.Varchar2, ParameterDirection.Input, chainMerchantNo);
                    dyParam.Add("OUTLETACCNO", OracleDbType.Varchar2, ParameterDirection.Input, outletAccNo);
                    dyParam.Add("OUTLETCODE", OracleDbType.Varchar2, ParameterDirection.Input, outletCode);
                    dyParam.Add("REPORTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportType);
                    dyParam.Add("REPORTVIEWTYPE", OracleDbType.Varchar2, ParameterDirection.Input, reportViewType);
                    dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
                    dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
                    dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    result = SqlMapper.Query<OutletSummaryTransaction>(connection, dbUser + "RPT_OutletToParentTransSummary", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

		public string GetChainMerchantCodeByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.mcode from one.merchant_config t where t.mphone = '"+mphone+"'";

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
	}
}

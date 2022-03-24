
using Dapper;
using MFS.DistributionService.Models;
using MFS.EnvironmentService.Models;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Repository
{
	public interface IMerchantRepository : IBaseRepository<Reginfo>
	{
		object GetMerchantListData();
		object GetDistributorDataByDistributorCode(string distributorCode);
		string GeneratePinNo(int fourDigitRandomNo);
		void UpdatePinNo(string mphone, string fourDigitRandomNo);
		object GetMerchantCodeList();
		object GetMerchantBankBranchList();
		object GetDistrictByBank(string bankCode);
		object GetBankBranchListByBankCodeAndDistCode(string eftBankCode, string eftDistCode);
		object GenerateMerchantCode(string selectedCategory);
		object GetRoutingNo(string eftBankCode, string eftDistCode, string eftBranchCode);
		object GetChainMerchantList();
		object GetChildCountByMcode(string mphone);
		object GetMerchantList(string filterId);
		object GetMerChantConfigByMphone(string mPhone);
		object GetMerchantUserList();
		object GetMerChantUserByMphone(string mphone);
		object GetMerchantListForUser();
		object CheckSnameExist(string orgCode);
		object GetRetailList(string filterId);
	}
	public class MerchantRepository : BaseRepository<Reginfo>, IMerchantRepository
	{
		private readonly string dbUser;
		public MerchantRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object GetMerchantListData()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, "M");
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_Get_RegIngo_ByCatType", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{
				throw;
			}



		}

		public object GetDistributorDataByDistributorCode(string distributorCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("disCode", OracleDbType.Varchar2, ParameterDirection.Input, distributorCode);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_Get_DistData_ByDistCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}

		public string GeneratePinNo(int fourDigitRandomNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("USRPASS", OracleDbType.Varchar2, ParameterDirection.Input, fourDigitRandomNo);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<string>(connection, dbUser + "SP_GeneratePinNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}

		public void UpdatePinNo(string mphone, string fourDigitRandomNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("mobliePhone", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					parameter.Add("fourDigitRandomNo", OracleDbType.Varchar2, ParameterDirection.Input, fourDigitRandomNo);
					var result = SqlMapper.Query(connection, dbUser + "SP_Update_PIN_No", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);

				}

			}
			catch (Exception)
			{

				throw;
			}

		}

		public object GetMerchantCodeList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("cur_MerchantCodeList", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, dbUser + "SP_GetMerchantCodeList", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}

		public object GetMerchantBankBranchList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select distinct t.bank_name as ""label"", t.bank_code as ""value"" from " + dbUser + "eft_bank t";

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

		public object GetDistrictByBank(string bankCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select distinct t.dist_name as ""label"", t.dist_code as ""value"" from " + dbUser + "eft_bank t where t.bank_code = '" + bankCode + "'";

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

		public object GetBankBranchListByBankCodeAndDistCode(string eftBankCode, string eftDistCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select distinct t.branch_name as ""label"", t.branch_code as ""value"" from " + dbUser + "eft_bank t where t.bank_code = '" + eftBankCode + "' and t.dist_code = '" + eftDistCode + "'";

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

		public object GenerateMerchantCode(string selectedCategory)
		{
			try
			{
				if (selectedCategory == "C" || selectedCategory == "ABM")
				{
					using (var connection = this.GetConnection())
					{
						string query = @"SELECT TO_CHAR(SYSDATE,'RRMMDD') || MAX(SUBSTR(MCODE,7,6))+1 || '0000' AS M_CODE FROM " + dbUser + "MERCHANT_CONFIG t where t.category = 'C' or t.category = 'ABM'";
						var result = connection.Query(query).FirstOrDefault();
						this.CloseConnection(connection);
						return result;
					}

				}
				else
				{


					using (var connection = this.GetConnection())
					{
						//string query = @"SELECT TO_CHAR(SYSDATE,'RRMMDD') || MAX(SUBSTR(MCODE,6,6))+1 AS M_CODE FROM " + dbUser + "MERCHANT_CONFIG";
						string query = @"SELECT TO_CHAR(SYSDATE, 'RRMMDD') || MAX(SUBSTR(MCODE, 7, 6)) + 1 AS M_CODE
										FROM ONE.MERCHANT_CONFIG T WHERE LENGTH(T.MCODE)=12";
						var result = connection.Query(query).FirstOrDefault();
						this.CloseConnection(connection);
						return result;
					}

				}
			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public object GetRoutingNo(string eftBankCode, string eftDistCode, string eftBranchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.routing_no as ""routing_no""  from " + dbUser + "eft_bank t where t.dist_code = '" + eftDistCode + "' and t.bank_code = '" + eftBankCode + "' and t.branch_code = '" + eftBranchCode + "'";
					var result = connection.Query(query).FirstOrDefault();

					this.CloseConnection(connection);
					return result;
				}

			}

			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetChainMerchantList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					//string query = @"select t.mphone as ""value"", t.mphone as ""label"" from " + dbUser + "merchant_config t where t.category = 'C'";
					string query = @"select t.mphone as ""value"", t.mphone as ""label""
									from one.merchant_config t join one.reginfo r 
									on t.mphone = r.mphone 
									where t.category = 'C' OR r.cat_id = 'ABM'";
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

		public object GetChildCountByMcode(string mcode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					//string query = @"select count(*) as count from " + dbUser + "merchant_config t where t.mcode like '%" + mcode + "%' and t.category = 'M'";
					string query = @"SELECT  TO_CHAR(MAX(SUBSTR(MCODE,1,16)) + 1) AS M_CODE
										FROM ONE.MERCHANT_CONFIG T
								WHERE T.CATEGORY IN ('M','C','B') AND substr(t.mcode,1,12) = substr(" + mcode + ",1,12)";
					var result = connection.Query<string>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public object GetMerchantList(string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CATTYPE", OracleDbType.Varchar2, ParameterDirection.Input, "M");
					parameter.Add("BRCODE", OracleDbType.Varchar2, ParameterDirection.Input, "0000");
					parameter.Add("REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, "L");
					parameter.Add("FILTEROPTION", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_GET_MERCHANT_BYFILTER", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}
		public object GetRetailList(string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CATTYPE", OracleDbType.Varchar2, ParameterDirection.Input, "CM");
					parameter.Add("BRCODE", OracleDbType.Varchar2, ParameterDirection.Input, "0000");
					parameter.Add("REGSTATUS", OracleDbType.Varchar2, ParameterDirection.Input, "L");
					parameter.Add("FILTEROPTION", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_GET_RETAIL_BYFILTER", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetMerChantConfigByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"SELECT T.COMPANYNAME AS ""_COMPANYNAME"", M.MCODE AS ""MCODE"", M.MPHONE AS ""MPHONE"",
							 M.MERCHANT_SMS_NOTIFICATION AS ""MERCHANTSMSNOTIFICATION"", M.CATEGORY as ""Category"",
							 M.SNAME, M.CUSTOMER_SERVICE_CHARGE_MAX AS ""CUSTOMERSERVICECHARGEMAX"",M.CUSTOMER_SERVICE_CHARGE_MIN AS ""CUSTOMERSERVICECHARGEMIN"",
							 M.STATUS AS ""STATUS"", M.MAX_TRANS_AMT AS ""MAXTRANSAMT"",
							 M.MIN_TRANS_AMT AS ""MINTRANSAMT"", M.CUSTOMER_SERVICE_CHARGE_PER*100 ""CUSTOMERSERVICECHARGEPER""
							 FROM " + dbUser + "MERCHANT_CONFIG M  INNER JOIN " + dbUser + "REGINFOVIEW T ON T.MPHONE = M.MPHONE  AND M.MPHONE = '"
							 + mPhone + "' AND (T.CATID = 'M' OR T.CATID = 'CM' OR T.CatId = 'EMSM' OR t.CatId = 'EMSC' OR T.CatId = 'MMSM' OR t.CatId = 'MMSC')";
					var result = connection.Query<MerchantConfig>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetMerchantUserList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.*, r.company_name from " + dbUser + "merchant_user t left join " + dbUser + "reginfo r on t.mobile_no = r.mphone";
					var result = connection.Query<dynamic>(query).ToList();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public object GetMerChantUserByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.* from " + dbUser + "merchant_user t where t.mobile_no = '" + mphone + "'";
					var result = connection.Query<MerchantUser>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public object GetMerchantListForUser()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @" SELECT T.MPHONE AS ""Value"", T.COMPANY_NAME AS ""Label"" FROM " + dbUser + "REGINFO T WHERE T.CAT_ID = 'M'";
					var result = connection.Query<CustomDropDownModel>(query).ToList();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public object CheckSnameExist(string orgCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"SELECT COUNT(*) FROM ONE.MERCHANT_CONFIG T WHERE T.SNAME = CONCAT('EMS.','" + orgCode.Trim() + "')";

					var result = connection.Query<int>(query).FirstOrDefault();
					this.CloseConnection(connection);
					connection.Dispose();
					if (Convert.ToUInt32(result) > 0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}

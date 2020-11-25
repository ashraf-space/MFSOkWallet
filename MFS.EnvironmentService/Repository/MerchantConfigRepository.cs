using Dapper;
using MFS.EnvironmentService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Repository
{

    public interface IMerchantConfigRepository : IBaseRepository<MerchantConfig>
    {

        object GetMerchantConfigListForDDL();
        object GetMerchantConfigDetails(string mphone);
        object GetParentInfoByChildMcode(string mcode);
        object GetAllMerchant();
		void OnMerchantConfigUpdate(MerchantConfig merchantConfig);
		object GetMerchantConfigDetails(string mphone, string mcode);
	}

    public class MerchantConfigRepository : BaseRepository<MerchantConfig>, IMerchantConfigRepository
    {
        private readonly string dbUser;
        public MerchantConfigRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }
        public object GetMerchantConfigListForDDL()
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("CUR_MerchantConfig", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, dbUser + "SP_Get_MerchantConfigForDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object GetMerchantConfigDetails(string mphone)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIENO", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<MerchantConfig>(connection, dbUser + "SP_GET_MERCHANTCONFIGDETAILS", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetParentInfoByChildMcode(string mcode)
        {
            using (var connection = this.GetConnection())
            {
                string query = @"select t.mphone from "+ dbUser + "merchant_config t where t.mcode like '%" + mcode + "%' and t.category = 'C'";
                var result = connection.Query<string>(query).FirstOrDefault();
                this.CloseConnection(connection);
                return result;
            }


        }

        public object GetAllMerchant()
        {

            using (var connection = this.GetConnection())
            {
                string query = @"SELECT T.MPHONE       AS MPHONE,
								T.NAME         AS NAME,
								T.REG_DATE     AS REGDATE,
								T.DIST_CODE    AS DISTCODE,
								T.CON_MOB      AS CONMOB,
								T.COMPANY_NAME AS COMPANYNAME,
								T.OFF_ADDR     AS OFFADDR,
								T.PHOTO_ID     AS PHOTOID,
								T.REG_STATUS   AS REGSTATUS 
									FROM "+ dbUser +"REGINFO T INNER JOIN "+ dbUser + 
									"MERCHANT_CONFIG M ON T.MPHONE = M.MPHONE AND (T.CAT_ID = 'M' OR T.CAT_ID = 'EMSM' OR T.CAT_ID = 'EMSC' OR M.CATEGORY = 'E') ORDER BY T.REG_DATE DESC";

                var result = connection.Query(query);

                this.CloseConnection(connection);
                return result;
            }

        }

		public void OnMerchantConfigUpdate(MerchantConfig merchantConfig)
		{
			try
			{
				using (var _connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("V_CATEGORY", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.Category);
					parameter.Add("V_MCODE", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.Mcode);
					parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.Mphone);
					parameter.Add("V_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.Status);
					parameter.Add("V_MAXAMT", OracleDbType.Double, ParameterDirection.Input, merchantConfig.MaxTransAmt);
					parameter.Add("V_MINAMT", OracleDbType.Double, ParameterDirection.Input, merchantConfig.MinTransAmt);
					parameter.Add("V_CUST_MAXAMT", OracleDbType.Double, ParameterDirection.Input, merchantConfig.CustomerServiceChargeMax);
					parameter.Add("V_CUST_MINAMT", OracleDbType.Double, ParameterDirection.Input, merchantConfig.CustomerServiceChargeMin);
					parameter.Add("V_CUST_SERV_CHARG", OracleDbType.Double, ParameterDirection.Input, merchantConfig.CustomerServiceChargePer);
					parameter.Add("V_MER_SMS_NOTI", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.MerchantSmsNotification);
					parameter.Add("V_UPDATE_BY", OracleDbType.Varchar2, ParameterDirection.Input, merchantConfig.UpdateBy);
					var result = SqlMapper.Query(_connection, dbUser + "SP_UPDATE_MER_CONF", param: parameter,
						commandType: CommandType.StoredProcedure).FirstOrDefault();

					_connection.Close();
				}

			}
			catch (Exception e)
			{
				throw;
			}
		}

		public object GetMerchantConfigDetails(string mphone, string mcode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"SELECT T.STATUS,
								   T.MAX_TRANS_AMT AS ""MaxTransAmt"",
								   T.MIN_TRANS_AMT AS ""MinTransAmt"",
								   T.CUSTOMER_SERVICE_CHARGE_PER AS ""CustomerServiceChargePer"",
								   T.MERCHANT_SMS_NOTIFICATION AS ""MerchantSmsNotification"",
								   T.UPDATE_BY AS ""UpdateBy"",
								   T.UPDATE_TIME AS ""UpdateTime"",
								   T.UPDATE_TIME FROM ONE.MERCHANT_CONFIG T WHERE T.MPHONE = '" + mphone+"' AND T.MCODE = '"+mcode+"'";
					var result = connection.Query<MerchantConfig>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}

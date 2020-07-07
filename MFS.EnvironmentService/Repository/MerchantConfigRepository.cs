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
	}

    public class MerchantConfigRepository : BaseRepository<MerchantConfig>, IMerchantConfigRepository
    {
              
        public object GetMerchantConfigListForDDL()
        {
            try
            {
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_MerchantConfig", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_Get_MerchantConfigForDDL", param: parameter, commandType: CommandType.StoredProcedure);

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
					var result = SqlMapper.Query<MerchantConfig>(connection, "SP_GET_MERCHANTCONFIGDETAILS", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
				string query = @"select t.mphone from merchant_config t where t.mcode like '%" + mcode + "%' and t.category = 'C'";
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
									FROM REGINFO T
										INNER JOIN MERCHANT_CONFIG M
											ON T.MPHONE = M.MPHONE AND T.CAT_ID = 'M'";

				var result = connection.Query(query);

				this.CloseConnection(connection);
				return result;
			}
				
		}
	}
}

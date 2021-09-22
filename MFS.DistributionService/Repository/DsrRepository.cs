
using Dapper;
using MFS.DistributionService.Models;
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
    public interface IDsrRepository : IBaseRepository<Reginfo>
    {
        object GetDsrListData();
        object GetDistributorDataByDistributorCode(string distributorCode);
        string GeneratePinNo(int fourDigitRandomNo);
        void UpdatePinNo(string mphone, string fourDigitRandomNo);
		object GetB2bDistributorDataByDistributorCode(string distributorCode);
	}
    public class DsrRepository : BaseRepository<Reginfo>, IDsrRepository
    {
		private readonly string dbUser;
		public DsrRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}

		public object GetDsrListData()
        {
            try
            {
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, "R");
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser+"SP_Get_RegIngo_ByCatType", param: parameter, commandType: CommandType.StoredProcedure);

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

		public object GetB2bDistributorDataByDistributorCode(string distributorCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("DISCODE", OracleDbType.Varchar2, ParameterDirection.Input, distributorCode);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, dbUser + "SP_GET_B2BDISTDATA_BYCODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

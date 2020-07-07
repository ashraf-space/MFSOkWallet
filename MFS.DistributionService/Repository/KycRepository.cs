using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.DistributionService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;

namespace MFS.DistributionService.Repository
{
	public interface IKycRepository : IBaseRepository<Reginfo>
	{
		void UpdatePinNo(string mphone, string fourDigitRandomNo);
		DateTime? GetRegDataByMphoneCatID(string mphone, string category);
		object CheckIsDistCodeExist(string distCode);
		object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status, string filterId);
		object GetRegInfoByMphone(string mPhone);
		object GetChainMerchantList(string filterId);
		object CheckNidValid(string photoid, string type);
		object GetOccupationList();
		object GetClientDistLocationInfo(string distCode, string locationCode);
		object GetPhotoIdTypeByCode(string photoIdTypeCode);
		object GetBranchNameByCode(string branchCode);
		Task<object> GetRegInfoListByOthersBranchCode(string branchCode, string catId, string status, string roleId);
		object CheckPinStatus(string mphone);
	}
	public class KycRepository : BaseRepository<Reginfo>, IKycRepository
	{
		

		public object CheckIsDistCodeExist(string distCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select count(*) from reginfo t where t.dist_code = '" + distCode + "' and t.cat_id = 'D'";
					var regdate = connection.QueryFirstOrDefault<int>(query);
					this.CloseConnection(connection);
					connection.Dispose();
					return regdate;
				}
					
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public DateTime? GetRegDataByMphoneCatID(string mphone, string category)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select reg_date from reginfo where mphone = '" + mphone + "' and cat_id  = '" + category + "' ";
					var regdate = connection.QueryFirstOrDefault<DateTime>(query);
					this.CloseConnection(connection);
					connection.Dispose();
					return Convert.ToDateTime(regdate);
				}
					
			}
			catch(Exception ex)
			{				
				throw ex;
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
					var result = SqlMapper.Query(connection, "SP_Update_PIN_No", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);
					connection.Dispose();
				}					
			}
			catch (Exception ex)
			{				
				throw ex;
			}
		}

		public object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status, string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
					parameter.Add("RegStatus", OracleDbType.Varchar2, ParameterDirection.Input, status);
					parameter.Add("FilterOption", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_Get_RegInfo_ByCatType", param: parameter, commandType: CommandType.StoredProcedure);

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

		public object GetRegInfoByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select * from RegInfoView where mphone= '" + mPhone + "' ";

					var result = connection.Query<Reginfo>(query).FirstOrDefault();

					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}					
			}
			catch(Exception ex)
			{
				throw ex;
			}
			


		}

		public object GetChainMerchantList(string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, "M");
					parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, "0000");
					parameter.Add("RegStatus", OracleDbType.Varchar2, ParameterDirection.Input, "L");
					parameter.Add("FilterOption", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_GET_CHAINMER_BYFILTER", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);
					connection.Dispose();
					return result;					
				}
					
			}
			catch(Exception ex)
			{
				throw ex; 
			}
			
			
		}

		public object CheckNidValid(string photoid, string type)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select count(*) as ""total"" from reginfo t where t.photo_id = '" + photoid + "' and t.cat_id = '" + type + "'";

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

		public object GetOccupationList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.oname as ""label"", t.ovalue ""value"" from occupation_list t";
					var result = connection.Query<CustomDropDownModel>(query).ToList();

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

		public object GetClientDistLocationInfo(string distCode, string locationCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("DISTCODE", OracleDbType.Varchar2, ParameterDirection.Input, distCode == "null" ? null : distCode);
					parameter.Add("LOC_CODE", OracleDbType.Varchar2, ParameterDirection.Input, locationCode == "null" ? null : locationCode);
					parameter.Add("CUR_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);
					//parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);				
					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_DISTLOC_INFO", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

		public object GetPhotoIdTypeByCode(string photoIdTypeCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"SELECT T.TYPE_NAME as ""value"" FROM PHOTO_ID_TYPE_LIST T WHERE T.ID = " + photoIdTypeCode + "";
					var result = connection.Query<dynamic>(query).FirstOrDefault();

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

		public object GetBranchNameByCode(string branchCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.branchname as ""value"" from bankbranch t where t.branchcode = '" + branchCode + "'";
					var result = connection.Query<dynamic>(query).FirstOrDefault();
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

		public async Task<object> GetRegInfoListByOthersBranchCode(string branchCode, string catId, string status, string filterId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
					parameter.Add("RegStatus", OracleDbType.Varchar2, ParameterDirection.Input, status);
					parameter.Add("FilterOption", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = await  SqlMapper.QueryAsync<dynamic>(connection, "SP_GET_REGINFO_BYOTHERS", param: parameter, commandType: CommandType.StoredProcedure);

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

		public object CheckPinStatus(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.reg_status as ""value"" from reginfo t where t.mphone= '" + mphone + "'";

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

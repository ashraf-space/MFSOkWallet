﻿using System;
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
		object GetBalanceInfoByMphone(string mphone);
		void StatusChangeBasedOnDemand(string mphone, string demand, string updateBy, string remarks = null);
		void AddOrRemoveLien(Reginfo reginfo, string remarks);
		object GetUserBranchCodeByUserId(string v);
		object GetCustomerByMphone(string mphone, string catId);
		void OnReleaseBindDevice(string mphone, string updateBy);
		object GetSubCatNameById(string mphone);
		CLoseReginfo GetCloseInfoByMphone(string mphone);
		object GetCloseAccount();
		object CloseAccount(string mphone, string updateBy, string remarks);
		DeviceInformation GetDeviceInformationByMphone(string mphone);
		Reginfo GetCloseRegInfoByMphone(string mphone);
		object CheckIsDistCodeExistForB2b(string distCode);
		object CheckNidValidWithIdType(string photoid, string type, int? idType);
	}
	public class KycRepository : BaseRepository<Reginfo>, IKycRepository
	{
		private readonly string dbUser;
		public KycRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object CheckIsDistCodeExist(string distCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select count(*) from " + dbUser + "reginfo t where t.dist_code = '" + distCode + "' and t.cat_id = 'D'";
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
					string query = "select reg_date from " + dbUser + "reginfo where mphone = '" + mphone + "' and cat_id  = '" + category + "' ";
					var regdate = connection.QueryFirstOrDefault<DateTime>(query);
					this.CloseConnection(connection);
					connection.Dispose();
					return Convert.ToDateTime(regdate);
				}

			}
			catch (Exception ex)
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
					var result = SqlMapper.Query(connection, dbUser + "SP_Update_PIN_No", param: parameter, commandType: CommandType.StoredProcedure);
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
					dynamic result;
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
					parameter.Add("RegStatus", OracleDbType.Varchar2, ParameterDirection.Input, status);
					parameter.Add("FilterOption", OracleDbType.Varchar2, ParameterDirection.Input, filterId);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					if (catId == "C")
					{
						result = SqlMapper.Query<dynamic>(connection, dbUser + "SP_Get_RegInfo_ByCatType", param: parameter, commandType: CommandType.StoredProcedure);
					}
					else
					{
						result = SqlMapper.Query<dynamic>(connection, dbUser + "SP_GET_REGINFO_HEADOFFICE", param: parameter, commandType: CommandType.StoredProcedure);

					}

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
					string query = @"Select * from " + dbUser + "RegInfoView where mphone= '" + mPhone + "' ";

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
					var result = SqlMapper.Query<dynamic>(connection, dbUser + "SP_GET_CHAINMER_BYFILTER", param: parameter, commandType: CommandType.StoredProcedure);

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

		public object CheckNidValid(string photoid, string type)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = string.Empty;
					if (type == "C")
					{
						query = @"select count(*) as ""total"" from " + dbUser + "reginfo t where t.photo_id = '" + photoid.Trim() + "' and t.status <> 'C'";
					}
					else
					{
						query = @"select count(*) as ""total"" from " + dbUser + "reginfo t where t.photo_id = '" + photoid.Trim() + "' and t.cat_id = '" + type + "' and t.status <> 'C'";
					}

					var result = connection.Query<int>(query).FirstOrDefault();
					this.CloseConnection(connection);
					connection.Dispose();
					if (Convert.ToUInt32(result) == 0)
					{
						return true;
					}
					else
					{
						return false;
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
					string query = @"select t.oname as ""label"", t.ovalue ""value"" from " + dbUser + "occupation_list t";
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
					var result = SqlMapper.Query<DistrictLocation>(connection, dbUser + "PR_GET_DISTLOC_INFO", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
					string query = @"SELECT T.TYPE_NAME as ""value"" FROM " + dbUser + "PHOTO_ID_TYPE_LIST T WHERE T.ID = " + photoIdTypeCode + "";
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
					string query = @"select t.branchname as ""value"" from " + dbUser + "bankbranch t where t.branchcode = '" + branchCode + "'";
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
					var result = await SqlMapper.QueryAsync<dynamic>(connection, dbUser + "SP_GET_REGINFO_BYOTHERS", param: parameter, commandType: CommandType.StoredProcedure);

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
					string query = @"select t.reg_status as ""value"" from " + dbUser + "reginfo t where t.mphone= '" + mphone + "'";

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

		public object GetBalanceInfoByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MOBLIENO", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<BalanceInfo>(connection, dbUser + "SP_GET_BLNC_INFO_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void StatusChangeBasedOnDemand(string mphone, string demand, string updateBy, string remarks)
		{
			try
			{
				if (mphone != null)
				{
					using (var connection = this.GetConnection())
					{
						var parameter = new OracleDynamicParameters();
						parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
						parameter.Add("ON_DEMAND", OracleDbType.Varchar2, ParameterDirection.Input, demand);
						parameter.Add("V_REMARKS", OracleDbType.Varchar2, ParameterDirection.Input, remarks);
						parameter.Add("V_UPDATEBY", OracleDbType.Varchar2, ParameterDirection.Input, updateBy);
						var result = SqlMapper.Query(connection, dbUser + "SP_CHNG_STATUS_ON_DEMAND", param: parameter, commandType: CommandType.StoredProcedure);
						this.CloseConnection(connection);
					}

				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void AddOrRemoveLien(Reginfo reginfo, string remarks)
		{
			try
			{
				if (reginfo.Mphone != null)
				{
					using (var connection = this.GetConnection())
					{
						var parameter = new OracleDynamicParameters();
						parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, reginfo.Mphone);
						parameter.Add("V_REMARKS", OracleDbType.Varchar2, ParameterDirection.Input, remarks);
						parameter.Add("V_UPDATEBY", OracleDbType.Varchar2, ParameterDirection.Input, reginfo.UpdateBy);
						var result = SqlMapper.Query(connection, dbUser + "SP_ADD_REMV_LIEN", param: parameter, commandType: CommandType.StoredProcedure);
						this.CloseConnection(connection);
					}

				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public object GetUserBranchCodeByUserId(string userId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.branch_code as ""branch_code"" from one.application_user t where t.id =" + Convert.ToInt32(userId) + "";

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

		public object GetCustomerByMphone(string mphone, string catId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select * from " + dbUser + "RegInfoView where mphone= '" + mphone + "' and CatId = '" + catId + "'";

					var result = connection.Query<Reginfo>(query).ToList();

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

		public void OnReleaseBindDevice(string mphone, string updateBy)
		{
			try
			{
				if (mphone != null)
				{
					using (var connection = this.GetConnection())
					{
						var parameter = new OracleDynamicParameters();
						parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);						
						parameter.Add("V_UPDATEBY", OracleDbType.Varchar2, ParameterDirection.Input, updateBy);
						var result = SqlMapper.Query(connection, dbUser + "SP_RELEASE_DEVICE", param: parameter, commandType: CommandType.StoredProcedure);
						this.CloseConnection(connection);
					}

				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public object GetSubCatNameById(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select p.name from one.reginfo t
										inner join one.product_setup_sub p
										on p.acc_type_code_sub = t.acc_type_code_sub
										where t.mphone = '"+mphone+"'";

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
		public CLoseReginfo GetCloseInfoByMphone(string mphone)
		{
			CLoseReginfo cLoseReginfo = new CLoseReginfo();
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.reg_date as ""Regdate"", t.mphone_old as ""MphoneOld"", t.close_date as ""CloseDate"" from one.reginfo t where t.mphone = '"+mphone+"'";
					cLoseReginfo = connection.QueryFirstOrDefault<CLoseReginfo>(query);
					connection.Close();
					return cLoseReginfo;
				}
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetCloseAccount()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.mphone,t.name,t.photo_id from one.reginfo t where t.status = 'C'";
					var result = connection.Query<dynamic>(query).ToList();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public object CloseAccount(string mphone, string updateBy, string remarks)
		{
			try
			{
				if (mphone != null)
				{
					using (var connection = this.GetConnection())
					{
						var parameter = new OracleDynamicParameters();
						parameter.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
						parameter.Add("V_REMARKS", OracleDbType.Varchar2, ParameterDirection.Input, remarks);
						parameter.Add("V_UPDATE_BY", OracleDbType.Varchar2, ParameterDirection.Input, updateBy);
						parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
						parameter.Add("V_MSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);			
						var result = SqlMapper.Query(connection, dbUser + "PROC_ACC_CLOSE", param: parameter, commandType: CommandType.StoredProcedure);
						string flag = parameter.oracleParameters[3].Value != null ? parameter.oracleParameters[3].Value.ToString() : null;
						string successOrErrorMsg = parameter.oracleParameters[4].Value != null ? parameter.oracleParameters[4].Value.ToString() : null;
						this.CloseConnection(connection);
						return  Tuple.Create(flag, successOrErrorMsg); ;
					}

				}
				else
				{
					return "Please Input Valid Account";
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public DeviceInformation GetDeviceInformationByMphone(string mphone)
		{
			DeviceInformation deviceInformation = new DeviceInformation();
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.device_otp as ""DeviceOtp"", t.device_id as ""DeviceId"" from one.reginfo t where t.mphone = '" + mphone + "'";
					deviceInformation = connection.QueryFirstOrDefault<DeviceInformation>(query);
					connection.Close();
					return deviceInformation;
				}
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public Reginfo GetCloseRegInfoByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @" select * from one.RegInfoView t where  t.mphone like '%"+mphone+"%' and t.status = 'C' order by rowid desc";

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

		public object CheckIsDistCodeExistForB2b(string distCode)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "select count(*) from " + dbUser + "reginfo t where t.dist_code = '" + distCode + "'";
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

		public object CheckNidValidWithIdType(string photoid, string type, int? idType)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = string.Empty;
					//int idTypeCode = Convert.ToInt32(idType);
					if (type == "C")
					{
						query = @"select count(*) as ""total"" from " + dbUser + "reginfo t where t.photo_id = '" + photoid.Trim() + "' and  t.photo_id_type_code = '"+ idType + "'  and t.status <> 'C'";
					}
					else
					{
						query = @"select count(*) as ""total"" from " + dbUser + "reginfo t where t.photo_id = '" + photoid.Trim() + "' and t.cat_id = '" + type + "' and t.status <> 'C'";
					}

					var result = connection.Query<int>(query).FirstOrDefault();
					this.CloseConnection(connection);
					connection.Dispose();
					if (Convert.ToUInt32(result) == 0)
					{
						return true;
					}
					else
					{
						return false;
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

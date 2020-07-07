
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
	public interface IDistributorRepository : IBaseRepository<Reginfo>
	{
		object GetDistributorListData();
		object GetDistributorListForDDL();
		object GetTotalAgentByMobileNo(string ExMobileNo);
		object GetDistributorByMphone(string mPhone);
		object GetDistcodeAndNameByMphone(string mPhone);
		string GeneratePinNo(int fourDigitRandomNo);
		object GetDistributorCodeByPhotoId(string pid);
		object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status);
		CompanyAndHolderName GetCompanyAndHolderName(string acNo);
		object GetDistributorAcList();
		object getRegInfoDetailsByMphone(string mphone);
		object getReginfoCashoutByMphone(string mphone);
		object GetDistCodeByPmhone(string pmphhone);
		object ExecuteReplace(DistributorReplace distributorReplace);
		bool IsExistsByMpohne(string mphone);
		bool IsExistsByCatidPhotoId(string catId, string photoId);
	}
	public class DistributorRepository : BaseRepository<Reginfo>, IDistributorRepository
	{
		
		public object GetDistributorListData()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, "PKG_ENVIORONMENT.PR_GetDistributorListData", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}



		}

		public object GetDistributorListForDDL()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_GetDistributorListForDDL", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}



		}

		public object GetTotalAgentByMobileNo(string ExMobileNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("ExMobileNo", OracleDbType.Varchar2, ParameterDirection.Input, ExMobileNo);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_GetTotalAgentByMobileNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}



		}


		public object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CatType", OracleDbType.Varchar2, ParameterDirection.Input, catId);
					parameter.Add("BrCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
					parameter.Add("RegStatus", OracleDbType.Varchar2, ParameterDirection.Input, status);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, "SP_Get_RegInfo_ByCatType", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}



		}
		public object GetDistributorByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MoblieNo", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Reginfo>(connection, "SP_GetDistributorByMphone", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{				
				throw;
			}

		}

		public object GetDistcodeAndNameByMphone(string mPhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select Dist_code,Name from Reginfo where mphone = " + "'" + mPhone + "'" + "";
					var result = connection.Query<dynamic>(query).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception ex)
			{				
				return ex.ToString();
			}

		}

		public CompanyAndHolderName GetCompanyAndHolderName(string acNo)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("acNo", OracleDbType.Varchar2, ParameterDirection.Input, acNo);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CompanyAndHolderName>(connection, "SP_GetCompanyAndHolderName", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
					var result = SqlMapper.Query<string>(connection, "SP_GeneratePinNo", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);
					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}

		}

		public object GetDistributorCodeByPhotoId(string pid)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("PID", OracleDbType.Varchar2, ParameterDirection.Input, pid);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<string>(connection, "SP_GET_DISTRIBUTOR_CODE_BY_PID", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}

		}
		public object GetDistributorAcList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_Get_DistributorAcList", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}



		}

		public object getRegInfoDetailsByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MoblieNo", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_GET_RegInfoDetail_ByMPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}
		}

		public object getReginfoCashoutByMphone(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("MoblieNo", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "SP_RegInfo_Cashout_ByMPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

					this.CloseConnection(connection);

					return result;
				}

			}
			catch (Exception)
			{				
				throw;
			}
		}

		public object GetDistCodeByPmhone(string pmphhone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("PARENT_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, pmphhone);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<string>(connection, "SP_GET_DIST_CODE_BY_PMPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					this.CloseConnection(connection);

					return result;
				}
			}
			catch (Exception)
			{				
				throw;
			}
		}

		public object ExecuteReplace(DistributorReplace distributorReplace)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("ExMobileNo", OracleDbType.Varchar2, ParameterDirection.Input, distributorReplace.ExMobileNo);
					parameter.Add("NewMobileNo", OracleDbType.Varchar2, ParameterDirection.Input, distributorReplace.NewMobileNo);
					parameter.Add("isWithDSR", OracleDbType.Varchar2, ParameterDirection.Input, distributorReplace.IsWithDSR);
					parameter.Add("MSGID", OracleDbType.Varchar2, ParameterDirection.Input, "999999999");
					parameter.Add("V_FLAG", OracleDbType.Double, ParameterDirection.Output);
					parameter.Add("OUTMSG", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
					//var result = SqlMapper.Query<string>(connection, "SP_Execute_Replacement", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					SqlMapper.Query(connection, "SP_Execute_Replacement", param: parameter, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);

					string flag = parameter.oracleParameters[4].Value != null ? parameter.oracleParameters[4].Value.ToString() : null;
					string successOrErrorMsg = null;
					if (flag == "0")
					{
						successOrErrorMsg = parameter.oracleParameters[5].Value != null ? parameter.oracleParameters[5].Value.ToString() : null;
					}
					else
					{
						successOrErrorMsg = flag;
					}
					return successOrErrorMsg;
				}


			}
			catch (Exception)
			{				
				throw;
			}
		}

		public bool IsExistsByMpohne(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select mphone from Reginfo where mphone = " + "'" + mphone + "'" + "";

					string result = connection.Query<string>(query).FirstOrDefault();

					this.CloseConnection(connection);
					if (!string.IsNullOrEmpty(result))
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
				throw;
			}
		}

		public bool IsExistsByCatidPhotoId(string catId, string photoId)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"Select mphone from Reginfo where Cat_Id = " + "'" + catId + "'" + " and Photo_Id = " + "'" + photoId + "'" + "";

				    string result =  connection.Query<string>(query).FirstOrDefault();

					this.CloseConnection(connection);
					if (!string.IsNullOrEmpty(result))
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
				throw;
			}
		}
	}
}

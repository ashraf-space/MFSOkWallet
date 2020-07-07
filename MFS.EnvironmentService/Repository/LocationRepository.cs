using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.EnvironmentService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;

namespace MFS.EnvironmentService.Repository
{
	public interface ILocationRepository : IBaseRepository<Location>
	{
		object GetRegionDropdownList();
		object GetAreaDDLByRegion(string code);
		object GetTerritoriesByArea(string code);
		object GetDivisionDropdownList();
		object GetChildDataByParent(string code);
		object GetBankBranchListForDDL();
		object GenerateDistributorCode(string territoryCode);
		string GetAreaCode(string code);
		object GetAllAreas();
		object GetAreabyid(string code);
		object SaveArea(Location aLocation);
		//object GetRegionDropdownListAreaComponent();	
		object GetPhotoIDTypeList();

		object GetTerritoryCode(string code);
		object SaveTerritory(Location aLocation);
		object GetTerritories();
		object GetTerritorieById(string code);
		object GetAreaByAreaCode(string code);
		object GetAreasDDL();

		object GetAllClusters();
		object SaveCluster(Location aLocation);
		object GetTerritoryDDL();
		object GetClusterCode(string code);
		object GetClusterById(string code);
		object GetClustersDDL();
	}
	public class LocationRepository : BaseRepository<Location>, ILocationRepository
	{
		
		public object GetRegionDropdownList()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_REGION", OracleDbType.RefCursor, ParameterDirection.Output);
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GET_REGION", param: parameter, commandType: CommandType.StoredProcedure).ToList();
					return result;
				}

			}
			catch (Exception e)
			{
				throw e;
			}


		}

		public string GetAreaCode(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("REGION_PARENT_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_AREA_CODE", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PKG_ENVIORONMENT.PR_GET_LATEST_AREA_CODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					var areaCode = result?.CODE;
					return areaCode?.ToString();
				}

			}
			catch (Exception e)
			{				
				throw;
			}

		}

		public object GetAllAreas()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
					parameter.Add("CUR_AREAS", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "PKG_ENVIORONMENT.PR_GET_AREAS", param: parameter, commandType: CommandType.StoredProcedure);
					return result;
				}
			}
			catch (Exception e)
			{				
				throw;
			}

		}

		public object GetAreabyid(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("AREA_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_AREA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<dynamic>(connection, "PKG_ENVIORONMENT.PR_GET_AREA_BY_ID", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					return result;
				}

			}
			catch (Exception e)
			{				
				throw;
			}


		}

		public object SaveArea(Location aLocation)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("V_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Code);
					parameter.Add("V_NAME", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Name);
					parameter.Add("V_PARENT_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Parent);
					parameter.Add("V_SELF_LEVEL", OracleDbType.Int32, ParameterDirection.Input, aLocation.Selflevel);
					parameter.Add("V_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.CreatedBy);
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
					if (aLocation.IsEdit)
					{
						SqlMapper.Query<dynamic>(connection, "PKG_ENVIORONMENT.PR_UPDATE_AREA", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
					else
					{
						SqlMapper.Query<dynamic>(connection, "PKG_ENVIORONMENT.PR_SAVE_AREA", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
				}

			}
			catch (Exception e)
			{
				throw;
			}

		}

		//public object GetRegionDropdownList()
		//{
		//	//var connection = this.GetConnection();
		//	//if (connection.State == ConnectionState.Closed)
		//	//{
		//	//	connection.Open();
		//	//}
		//	var parameter = new OracleDynamicParameters();
		//	parameter.Add("CUR_REGION", OracleDbType.RefCursor, ParameterDirection.Output);
		//	parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
		//	var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GET_REGION", param: parameter, commandType: CommandType.StoredProcedure);
		//	return result;

		//}

		public object GetBankBranchListForDDL()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("CUR_BANKBRANCH", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GetBankBranchListForDDL", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		public object GetPhotoIDTypeList()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("cur_photoIdList", OracleDbType.RefCursor, ParameterDirection.Output);
				//var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GetPhotoIDTypeList", param: parameter, commandType: CommandType.StoredProcedure);
				var result = SqlMapper.Query<DropdownListModel>(connection, "PKG_ENVIORONMENT.PR_GetPhotoIDTypeList", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}


		}

		public object GetDivisionDropdownList()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("CUR_DIVISION", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GET_DIVISION", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}
		}
		public object GetAreaDDLByRegion(string code)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("REGIONCODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
				parameter.Add("CUR_AREA", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GETAREABYREGIONCODE", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		public object GetTerritoriesByArea(string code)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("REGIONCODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
				parameter.Add("CUR_AREA", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GETAREABYREGIONCODE", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		public object GetChildDataByParent(string code)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("REGIONCODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
				parameter.Add("cur_data", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PKG_ENVIORONMENT.PR_GetChildDataByParentForDDL", param: parameter, commandType: CommandType.StoredProcedure);
				return result;
			}
		}

		public object GenerateDistributorCode(string territoryCode)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("DigitNo", OracleDbType.Int32, ParameterDirection.Input, territoryCode.Length);
				parameter.Add("TerritoryCode", OracleDbType.Varchar2, ParameterDirection.Input, territoryCode);
				parameter.Add("DistributorCode_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<dynamic>(connection, "pr_GenerateDistributorCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
				return result;
			}
		}

		public object GetTerritoryCode(string code)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("AREA_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
				parameter.Add("TERRITORY_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<dynamic>(connection, "PR_GENERATETERRITORYCODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
				return result;
			}
		}

		public object SaveTerritory(Location aLocation)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("V_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Code);
					parameter.Add("V_NAME", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Name);
					parameter.Add("V_PARENT_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Parent);
					parameter.Add("V_SELF_LEVEL", OracleDbType.Int32, ParameterDirection.Input, aLocation.Selflevel);
					parameter.Add("V_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.CreatedBy);
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);

					if (aLocation.IsEdit)
					{
						SqlMapper.Query<dynamic>(connection, "PR_UPDATE_TERRITORY", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
					else
					{
						SqlMapper.Query<dynamic>(connection, "PR_SAVE_TERRITORY", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
				}

			}
			catch (Exception e)
			{
				throw;
			}
		}

		public object GetTerritories()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
					parameter.Add("CUR_TERRITORY", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_TERRITORIES", param: parameter, commandType: CommandType.StoredProcedure);
					return result;
				}					
			}
			catch (Exception e)
			{				
				throw;
			}
		}

		public object GetTerritorieById(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("TERRITORY_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_TERRITORY_BY_ID", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					return result;
				}					
			}
			catch (Exception e)
			{				
				throw;
			}
		}

		public object GetAreaByAreaCode(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("AREA_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_AREA", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_AREA_BY_AREA_CODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					return result;
				}
					
			}
			catch (Exception e)
			{				
				throw;
			}
		}

		public object GetAreasDDL()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("CUR_AREA", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PR_GET_AREAS", param: parameter, commandType: CommandType.StoredProcedure).ToList();
				return result;
			}
				
		}

		public object GetAllClusters()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);
					parameter.Add("CUR_CLUSTERS", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_CLUSTERS", param: parameter, commandType: CommandType.StoredProcedure);
					return result;
				}
					
			}
			catch (Exception e)
			{				
				throw;
			}
		}

		public object SaveCluster(Location aLocation)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("V_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Code);
					parameter.Add("V_NAME", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Name);
					parameter.Add("V_PARENT_CODE", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.Parent);
					parameter.Add("V_SELF_LEVEL", OracleDbType.Int32, ParameterDirection.Input, aLocation.Selflevel);
					parameter.Add("V_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input, aLocation.CreatedBy);
					parameter.Add("OUT_MESSEGE", OracleDbType.Varchar2, ParameterDirection.Output);

					if (aLocation.IsEdit)
					{
						SqlMapper.Query<dynamic>(connection, "PR_UPDATE_CLUSTER", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
					else
					{
						SqlMapper.Query<dynamic>(connection, "PR_SAVE_CLUSTER", param: parameter, commandType: CommandType.StoredProcedure);
						return HttpStatusCode.OK;
					}
				}
					
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public object GetTerritoryDDL()
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("CUR_CLUSTER", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<CustomDropDownModel>(connection, "PR_GET_TERRITORY_DDL", param: parameter, commandType: CommandType.StoredProcedure).ToList();
				return result;
			}
				
		}

		public object GetClusterCode(string code)
		{
			using (var connection = this.GetConnection())
			{
				var parameter = new OracleDynamicParameters();
				parameter.Add("TERRITORY_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
				parameter.Add("CLUSTER_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
				var result = SqlMapper.Query<dynamic>(connection, "PR_GENERTE_CLUSTER_CODE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
				return result;
			}
				
		}

		public object GetClusterById(string code)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CLUSTER_CODE", OracleDbType.Varchar2, ParameterDirection.Input, code);
					parameter.Add("CUR_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_CLUSTER_BY_ID", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					return result;
				}					
			}
			catch (Exception e)
			{				
				throw;
			}
		}

		public object GetClustersDDL()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_CLUSTER", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, "PR_GET_CLUSTERS_DDL", param: parameter, commandType: CommandType.StoredProcedure).ToList();
					connection.Close();
					return result;
				}					
			}
			catch (Exception e)
			{				
				throw;
			}

		}
	}
}

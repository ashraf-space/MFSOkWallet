using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Repository;
using OneMFS.SharedResources;

namespace MFS.EnvironmentService.Service
{
	public interface ILocationService : IBaseService<Location>
	{
		object GetRegionDropdownList();
        object GetAreaDDLByRegion(string code);
        object GetTerritoriesByArea(string code);
        object GetDivisionDropdownList();
        object GetChildDataByParent(string code);
        object GetBankBranchListForDDL();
        object GenerateDistributorCode(string territoryCode);
		object GenerateB2bDistributorCode(string territoryCode);
		string GetAreaCode(string code);
		object SaveArea(Location aLocation);
		object SaveArea(Location aLocation, string newAreaCode);
		object GetAllAreas();
		object GetAreabyid(string code);
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
	public class LocationService: BaseService<Location>, ILocationService
	{
		private ILocationRepository _repository;
		public LocationService(ILocationRepository repository)
		{
			_repository = repository;
		}
		public object GetRegionDropdownList()
		{
			try
			{
				var code = _repository.GetRegionDropdownList();
				return code;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return e;
			}

		}

		public string GetAreaCode(string code)
		{
			try
			{
				string result = _repository.GetAreaCode(code);
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return e.ToString();
			}

		}

		public object SaveArea(Location aLocation)
		{
			try
			{
				aLocation.Selflevel = 2;
				var code = _repository.SaveArea(aLocation);
				return code;
			}
			catch (Exception e)
			{
				return e.ToString();
			}
			
		}

		public object SaveArea(Location aLocation, string newAreaCode)
		{
			try
			{
				aLocation.Selflevel = 2;
				aLocation.Code = newAreaCode;
				_repository.SaveArea(aLocation);
				return HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

		}


		public object GetAllAreas()
		{
			try
			{
				var areas = _repository.GetAllAreas();
				if (areas != null)
				{
					return areas;
				}
				else
				{
					return "Not found";
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return e.ToString();
			}

		}

		public object GetAreabyid(string code)
		{
			try
			{
				var area = _repository.GetAreabyid(code);
				return area;
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
		

        public object GetPhotoIDTypeList()
        {
            return _repository.GetPhotoIDTypeList();
        }

        public object GetDivisionDropdownList()
        {
            return _repository.GetDivisionDropdownList();
        }

        public object GetAreaDDLByRegion(string code)
        {
            return _repository.GetAreaDDLByRegion(code);
        }

        public object GetTerritoriesByArea(string code)
        {
            return _repository.GetTerritoriesByArea(code);
        }

        public object GetChildDataByParent(string code)
        {
            return _repository.GetChildDataByParent(code);
        }

        public object GetBankBranchListForDDL()
        {
            return _repository.GetBankBranchListForDDL();
        }

        public object GenerateDistributorCode(string territoryCode)
        {
            return _repository.GenerateDistributorCode(territoryCode);
        }
		public object GenerateB2bDistributorCode(string territoryCode)
		{
			return _repository.GenerateB2bDistributorCode(territoryCode);
		}
		public object GetTerritoryCode(string code)
		{
			return _repository.GetTerritoryCode(code);
		}

		public object SaveTerritory(Location aLocation)
		{		
			try
			{
				aLocation.Selflevel = 3;
				return _repository.SaveTerritory(aLocation);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}							
		}

		public object GetTerritories()
		{
			return _repository.GetTerritories();
		}

		public object GetTerritorieById(string code)
		{
			return _repository.GetTerritorieById(code);
		}

		public object GetAreaByAreaCode(string code)
		{
			return _repository.GetAreaByAreaCode(code);
		}

		public object GetAreasDDL()
		{
			return _repository.GetAreasDDL();
		}

		public object GetAllClusters()
		{
			return _repository.GetAllClusters();
		}

		public object SaveCluster(Location aLocation)
		{
			try
			{
				aLocation.Selflevel = 4;
				return _repository.SaveCluster(aLocation);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			
		}

		public object GetTerritoryDDL()
		{
			return _repository.GetTerritoryDDL();
		}

		public object GetClusterCode(string code)
		{
			return _repository.GetClusterCode(code);
		}

		public object GetClusterById(string code)
		{
			return _repository.GetClusterById(code);
		}

		public object GetClustersDDL()
		{
			return _repository.GetClustersDDL();
		}
	}
}

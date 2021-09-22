
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Service
{
    public interface IDsrService : IBaseService<Reginfo>
    {
        object GetDsrListData();
        object GetDistributorDataByDistributorCode(string distributorCode);
        string GeneratePinNo(int fourDigitRandomNo);
        void UpdatePinNo(string mphone, string fourDigitRandomNo);
		object GetB2bDistributorDataByDistributorCode(string distributorCode);
	}
    public class DsrService : BaseService<Reginfo>,IDsrService
    {
        private readonly IDsrRepository _DsrRepository;
        public DsrService(IDsrRepository DsrRepository)
        {
            this._DsrRepository = DsrRepository;
        }

        public object GetDsrListData()
        {
            return _DsrRepository.GetDsrListData();
        }

        public object GetDistributorDataByDistributorCode(string distributorCode)
        {
            try
            {
                return _DsrRepository.GetDistributorDataByDistributorCode(distributorCode);
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
                return _DsrRepository.GeneratePinNo(fourDigitRandomNo);
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
                _DsrRepository.UpdatePinNo(mphone, fourDigitRandomNo);
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
				return _DsrRepository.GetB2bDistributorDataByDistributorCode(distributorCode);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}

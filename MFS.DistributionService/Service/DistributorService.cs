
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
    public interface IDistributorService : IBaseService<Reginfo>
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
    public class DistributorService : BaseService<Reginfo>,IDistributorService
    {
        private readonly IDistributorRepository _distributorRepository;
        public DistributorService(IDistributorRepository distributorRepository)
        {
            this._distributorRepository = distributorRepository;
        }

        public object GetDistributorListData()
        {
            return _distributorRepository.GetDistributorListData();
        }

        public object GetDistributorListForDDL()
        {
            return _distributorRepository.GetDistributorListForDDL();
        }
        public object GetTotalAgentByMobileNo(string ExMobileNo)
        {
            return _distributorRepository.GetTotalAgentByMobileNo(ExMobileNo);
        }

        public object GetRegInfoListByCatIdBranchCode(string branchCode, string catId, string status)
        {
            return _distributorRepository.GetRegInfoListByCatIdBranchCode(branchCode, catId, status);
        }

        public object GetDistributorByMphone(string mPhone)
        {
            try
            {
                return _distributorRepository.GetDistributorByMphone(mPhone);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public object GetDistcodeAndNameByMphone(string mPhone)
        {
            try
            {
                return _distributorRepository.GetDistcodeAndNameByMphone(mPhone);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CompanyAndHolderName GetCompanyAndHolderName(string acNo)
        {
            try
            {
                return _distributorRepository.GetCompanyAndHolderName( acNo);
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
                return _distributorRepository.GeneratePinNo(fourDigitRandomNo);
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
			    return _distributorRepository.GetDistributorCodeByPhotoId(pid);

		    }
		    catch (Exception e)
		    {
			    Console.WriteLine(e);
			    throw;
		    }
	    }
        public object GetDistributorAcList()
        {
            try
            {
                return _distributorRepository.GetDistributorAcList();
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
                return _distributorRepository.getRegInfoDetailsByMphone(mphone);
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
                return _distributorRepository.getReginfoCashoutByMphone(mphone);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public object GetDistCodeByPmhone(string pmphhone)
		{
			return _distributorRepository.GetDistCodeByPmhone(pmphhone);
		}

        public object  ExecuteReplace(DistributorReplace distributorReplace)
        {
            try
            {
                return _distributorRepository.ExecuteReplace(distributorReplace);
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
                return _distributorRepository.IsExistsByMpohne(mphone);
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
                return _distributorRepository.IsExistsByCatidPhotoId( catId,  photoId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}


using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
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
		object GetDistributorListWithDistCodeForDDL();
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
        object GetRegionDetailsByMobileNo(string mobileNo);
		object GetB2bDistributorListWithDistCodeForDDL();
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
				Base64Conversion base64Conversion = new Base64Conversion();
				Reginfo reginfo = (Reginfo) _distributorRepository.GetDistributorByMphone(mPhone);
				if (reginfo != null)
				{
					if (base64Conversion.IsBase64(reginfo.FatherName))
					{
						reginfo.FatherName = base64Conversion.DecodeBase64(reginfo.FatherName);
					}
					if (base64Conversion.IsBase64(reginfo.MotherName))
					{
						reginfo.MotherName = base64Conversion.DecodeBase64(reginfo.MotherName);
					}
					if (base64Conversion.IsBase64(reginfo.SpouseName))
					{
						reginfo.SpouseName = base64Conversion.DecodeBase64(reginfo.SpouseName);
					}
					if (base64Conversion.IsBase64(reginfo.PreAddr))
					{
						reginfo.PreAddr = base64Conversion.DecodeBase64(reginfo.PreAddr);
					}
					if (base64Conversion.IsBase64(reginfo.PerAddr))
					{
						reginfo.PerAddr = base64Conversion.DecodeBase64(reginfo.PerAddr);
					}
					//
					if (base64Conversion.IsBase64(reginfo._FatherNameBangla))
					{
						reginfo._FatherNameBangla = base64Conversion.DecodeBase64(reginfo._FatherNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._MotherNameBangla))
					{
						reginfo._MotherNameBangla = base64Conversion.DecodeBase64(reginfo._MotherNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._SpouseNameBangla))
					{
						reginfo._SpouseNameBangla = base64Conversion.DecodeBase64(reginfo._SpouseNameBangla);
					}
					if (base64Conversion.IsBase64(reginfo._PreAddrBangla))
					{
						reginfo._PreAddrBangla = base64Conversion.DecodeBase64(reginfo._PreAddrBangla);
					}
					if (base64Conversion.IsBase64(reginfo._PerAddrBangla))
					{
						reginfo._PerAddrBangla = base64Conversion.DecodeBase64(reginfo._PerAddrBangla);
					}

				}
				return reginfo;
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

        public object GetRegionDetailsByMobileNo(string mobileNo)
        {
            try
            {
                return _distributorRepository.GetRegionDetailsByMobileNo(mobileNo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

		public object GetDistributorListWithDistCodeForDDL()
		{
			try
			{
				return _distributorRepository.GetDistributorListWithDistCodeForDDL();
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public object GetB2bDistributorListWithDistCodeForDDL()
		{
			try
			{
				return _distributorRepository.GetB2bDistributorListWithDistCodeForDDL();
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}

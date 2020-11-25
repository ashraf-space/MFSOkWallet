using MFS.EnvironmentService.Models;
using MFS.EnvironmentService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Service
{

    public interface IMerchantConfigService : IBaseService<MerchantConfig>
    {
        object GetMerchantConfigListForDDL();
        object GetMerchantConfigDetails(string mphone);
		object GetParentInfoByChildMcode(string v);
		object GetAllMerchant();
		void OnMerchantConfigUpdate(MerchantConfig merchantConfig);
		object GetMerchantConfigDetails(string mphone, string mcode);
	}

    public class MerchantConfigService : BaseService<MerchantConfig>, IMerchantConfigService
    {
        public IMerchantConfigRepository MerchantConfigRepo;
        public MerchantConfigService(IMerchantConfigRepository _MerchantConfigRepo)
        {
            MerchantConfigRepo = _MerchantConfigRepo;
        }

        public object GetMerchantConfigListForDDL()
        {
            try
            {
                return MerchantConfigRepo.GetMerchantConfigListForDDL();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetMerchantConfigDetails(string mphone)
        {
            try
            {
                return MerchantConfigRepo.GetMerchantConfigDetails(mphone);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public object GetParentInfoByChildMcode(string mcode)
		{
			return MerchantConfigRepo.GetParentInfoByChildMcode(mcode);
		}

		public object GetAllMerchant()
		{
			return MerchantConfigRepo.GetAllMerchant();
		}

		public void OnMerchantConfigUpdate(MerchantConfig merchantConfig)
		{
			 MerchantConfigRepo.OnMerchantConfigUpdate(merchantConfig);
		}

		public object GetMerchantConfigDetails(string mphone, string mcode)
		{
			return MerchantConfigRepo.GetMerchantConfigDetails(mphone, mcode);
		}
	}
}

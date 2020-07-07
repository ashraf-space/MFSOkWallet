using MFS.TransactionService.Models;
using MFS.TransactionService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Service
{
    public interface IRateconfigMstService : IBaseService<RateconfigMst>
    {
        IEnumerable<RateconfigMst> GetRateConfigMasterList(int configId = 0);
    }
    public class RateconfigMstService : BaseService<RateconfigMst>, IRateconfigMstService
    {
        private IRateconfigMstRepository repo;
        public RateconfigMstService(IRateconfigMstRepository _repo)
        {
            this.repo = _repo;
        }

        public IEnumerable<RateconfigMst> GetRateConfigMasterList(int configId = 0)
        {
            return repo.GetRateConfigMasterList(configId);
        }
    }
}

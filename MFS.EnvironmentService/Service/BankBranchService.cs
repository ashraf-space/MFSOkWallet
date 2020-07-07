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

    public interface IBankBranchService : IBaseService<Bankbranch>
    {
        object GetBankBranchDropdownList();
        object GetBankBranchByBranchCode(string branchCode);
    }

    public class BankBranchService : BaseService<Bankbranch>, IBankBranchService
    {
        public IBankBranchRepository bankBranchRepo;
        public BankBranchService(IBankBranchRepository _bankBranchRepo)
        {
            bankBranchRepo = _bankBranchRepo;
        }

        public object GetBankBranchDropdownList()
        {
            return bankBranchRepo.GetBankBranchDropdownList();
        }

        public object GetBankBranchByBranchCode(string branchCode)
        {
            try
            {
                return bankBranchRepo.GetBankBranchByBranchCode(branchCode);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}

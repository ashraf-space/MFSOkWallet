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
    public interface IDormantAccService : IBaseService<DormantAcc>
    {
    }

    public class DormantAccService: BaseService<DormantAcc>,IDormantAccService
    {
        private IDormantAccRepository repo;

        public DormantAccService(IDormantAccRepository _repo)
        {
            this.repo = _repo;
        }
    }
}

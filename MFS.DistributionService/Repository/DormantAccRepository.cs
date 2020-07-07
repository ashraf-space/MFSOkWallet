using MFS.DistributionService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Repository
{
    public interface IDormantAccRepository : IBaseRepository<DormantAcc>
    {
        
    }

    public class DormantAccRepository : BaseRepository<DormantAcc>, IDormantAccRepository
    {

    }
}

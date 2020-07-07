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
    public interface IGlCoaService : IBaseService<GlCoa>
    {
        
    }
    public class GlCoaService : BaseService<GlCoa>, IGlCoaService
    {
        private IGlCoaRepository repo;

        public GlCoaService(IGlCoaRepository _repo)
        {
            repo = _repo;
        }
    }
}

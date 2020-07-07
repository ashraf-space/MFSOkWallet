using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface IGlCoaRepository : IBaseRepository<GlCoa>
    {
        
    }
    public class GlCoaRepository : BaseRepository<GlCoa>, IGlCoaRepository
    {
    }   
}

using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface ITblDisburseTmpRepository : IBaseRepository<TblDisburseTmp>
    {
        
    }
    public class TblDisburseTmpRepository : BaseRepository<TblDisburseTmp>, ITblDisburseTmpRepository
    {
        

        

    }
}

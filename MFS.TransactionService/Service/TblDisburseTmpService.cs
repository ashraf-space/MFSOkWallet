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
    public interface ITblDisburseTmpService : IBaseService<TblDisburseTmp>
    {        
    }
    public class TblDisburseTmpService : BaseService<TblDisburseTmp>, ITblDisburseTmpService
    {
        private readonly ITblDisburseTmpRepository _TblDisburseTmpRepository;
        public TblDisburseTmpService(ITblDisburseTmpRepository objTblDisburseTmpRepository)
        {
            this._TblDisburseTmpRepository = objTblDisburseTmpRepository;
        }

       
    }
}

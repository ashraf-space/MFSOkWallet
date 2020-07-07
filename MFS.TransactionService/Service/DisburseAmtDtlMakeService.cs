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
    public interface IDisburseAmtDtlMakeService : IBaseService<TblDisburseAmtDtlMake>
    {
        object GetTransactionList(double transAmtLimt);
    }
    public class DisburseAmtDtlMakeService : BaseService<TblDisburseAmtDtlMake>, IDisburseAmtDtlMakeService
    {
        private readonly IDisburseAmtDtlMakeRepository _DisburseAmtDtlMakeRepository;
        public DisburseAmtDtlMakeService(IDisburseAmtDtlMakeRepository objDisburseAmtDtlMakeRepository)
        {
            this._DisburseAmtDtlMakeRepository = objDisburseAmtDtlMakeRepository;
        }
        public object GetTransactionList(double transAmtLimt)
        {
            try
            {
                return _DisburseAmtDtlMakeRepository.GetTransactionList(transAmtLimt);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

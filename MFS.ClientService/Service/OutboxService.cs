using MFS.ClientService.Models;
using MFS.ClientService.Models.Views;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IOutboxService : IBaseService<Outbox>
    {
        IList<OutboxViewModel> GetOutboxList(DateTime? fromDate, DateTime? toDate, string mPhone);
    }

    public class OutboxService : BaseService<Outbox>, IOutboxService
    {
        public IOutboxRepository  repo;
        public OutboxService(IOutboxRepository _repo)
        {
            repo = _repo;
        }

        public IList<OutboxViewModel> GetOutboxList(DateTime? fromDate, DateTime? toDate, string mPhone)
        {
            return repo.GetOutboxList(fromDate, toDate, mPhone);
        }
    }
}

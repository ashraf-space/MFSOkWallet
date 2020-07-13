using Dapper;
using MFS.ClientService.Models;
using MFS.ClientService.Models.Views;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IOutboxRepository : IBaseRepository<Outbox>
    {
        IList<OutboxViewModel> GetOutboxList(DateTime? fromDate, DateTime? toDate, string mPhone);
    }

    public class OutboxRepository : BaseRepository<Outbox>, IOutboxRepository
    {
		private readonly string dbUser;
		public OutboxRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public IList<OutboxViewModel> GetOutboxList(DateTime? fromDate, DateTime? toDate, string mPhone)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROM_DATE", OracleDbType.Date, ParameterDirection.Input, fromDate);
					dyParam.Add("UPTO_DATE", OracleDbType.Date, ParameterDirection.Input, toDate);
					dyParam.Add("AC_NO", OracleDbType.Varchar2, ParameterDirection.Input, mPhone);
					dyParam.Add("OUTBOX", OracleDbType.RefCursor, ParameterDirection.Output);

					IList<OutboxViewModel> result = SqlMapper.Query<OutboxViewModel>(connection, dbUser+"PR_MFS_GETOUTBOXMSG", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);

					return result;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}				
        }
    }
}

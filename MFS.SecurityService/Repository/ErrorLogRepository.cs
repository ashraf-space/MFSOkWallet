using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IErrorLogRepository : IBaseRepository<Errorlog>
    {
        object GetErrorByFiltering(DateRangeModel date, string user);
    }
    public class ErrorLogRepository : BaseRepository<Errorlog>, IErrorLogRepository
    {
		private readonly string dbUser;
		public ErrorLogRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object GetErrorByFiltering(DateRangeModel date, string user)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("FROM_DATE", OracleDbType.Date, ParameterDirection.Input, date.FromDate);
                    dyParam.Add("UPTO_DATE", OracleDbType.Date, ParameterDirection.Input, date.ToDate);
                    dyParam.Add("USERID", OracleDbType.Varchar2, ParameterDirection.Input, user.Trim());
                    dyParam.Add("LOGS", OracleDbType.RefCursor, ParameterDirection.Output);

                    var result = SqlMapper.Query<dynamic>(connection, dbUser + "PR_GET_ERRORLOG_BYFILTER", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

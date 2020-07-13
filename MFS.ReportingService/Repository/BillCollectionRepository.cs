using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;

namespace MFS.ReportingService.Repository
{
    public interface IBillCollectionRepository : IBaseRepository<BillCollection>
    {
        List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType);
    }
    public class BillCollectionRepository : BaseRepository<BillCollection>, IBillCollectionRepository
    {
        private readonly string dbUser;
        public BillCollectionRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }

        public List<BillCollection> GetDpdcDescoReport(string utility, string fromDate, string toDate, string gateway, string dateType, string catType)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("UTILITY", OracleDbType.Varchar2, ParameterDirection.Input, utility);
                    dyParam.Add("FROMDATE", OracleDbType.Varchar2, ParameterDirection.Input, fromDate);
                    dyParam.Add("TODATE", OracleDbType.Varchar2, ParameterDirection.Input, toDate);
                    dyParam.Add("P_GATEWAY", OracleDbType.Varchar2, ParameterDirection.Input, gateway);
                    dyParam.Add("DATETYPE", OracleDbType.Varchar2, ParameterDirection.Input, dateType);
                    dyParam.Add("CUSTTYPE", OracleDbType.Varchar2, ParameterDirection.Input, catType);
                    dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

                    List<BillCollection> result = SqlMapper.Query<BillCollection>(connection, dbUser + "RPT_DESCODPDCBILL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    //List<BillCollection> result = null;
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

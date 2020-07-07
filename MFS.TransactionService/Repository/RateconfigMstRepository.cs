using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface IRateconfigMstRepository : IBaseRepository<RateconfigMst>
    {
        IEnumerable<RateconfigMst> GetRateConfigMasterList(int configId);
    }
    public class RateconfigMstRepository : BaseRepository<RateconfigMst>, IRateconfigMstRepository
    {
        
        public IEnumerable<RateconfigMst> GetRateConfigMasterList(int configId)
        {
            //var conn = this.GetConnection();
            //if (conn.State == ConnectionState.Closed)
            //{
            //    conn.Open();
            //}

            //string query = "Select " + this.GetCamelCaseColumnList(new RateconfigMst()) + ",Rateconfig_for, Telco_config from rateconfig_view";

            //query = configId == 0 ? query : query + " where Config_id=" + configId;

            //var result = conn.Query<RateconfigMst>(query);

            //this.CloseConnection(conn);

            //return result;

            try
            {
                using (var conn = this.GetConnection())
                {
                    string query = "Select " + this.GetCamelCaseColumnList(new RateconfigMst()) + ",Rateconfig_for, Telco_config from rateconfig_view";

                    query = configId == 0 ? query : query + " where Config_id=" + configId;

                    var result = conn.Query<RateconfigMst>(query);

                    //this.CloseConnection(conn);
                    conn.Close();

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

using Dapper;
using MFS.TransactionService.Models;
using MFS.TransactionService.Models.Views;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Repository
{
    public interface ITransactionDetailRepository : IBaseRepository<GlTransDtl>
    {
        dynamic GetTransactionDetailList(string transactionNumber);
    }
    public class TransactionDetailRepository : BaseRepository<GlTransDtl>, ITransactionDetailRepository
    {
       
        public dynamic GetTransactionDetailList(string transactionNumber)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var result = conn.Query<TransactionDetailView>("select * from GLTransDTLView where transNo = '" + transactionNumber + "'");
                    //this.CloseConnection(conn);
                    conn.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            //var conn = this.GetConnection();
            //if (conn.State == ConnectionState.Closed)
            //{
            //    conn.Open();
            //}

            //var result = conn.Query<TransactionDetailView>("select * from GLTransDTLView where transNo = '"+ transactionNumber + "'");
            //this.CloseConnection(conn);

            //return result;
        }
    }
}

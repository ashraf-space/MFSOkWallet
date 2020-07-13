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
    public interface IDisburseAmtDtlMakeRepository : IBaseRepository<TblDisburseAmtDtlMake>
    {
        object GetTransactionList(double transAmtLimt);
    }
    public class DisburseAmtDtlMakeRepository : BaseRepository<TblDisburseAmtDtlMake>, IDisburseAmtDtlMakeRepository
    {
        MainDbUser mainDbUser = new MainDbUser();

        public object GetTransactionList(double transAmtLimt)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("transAmtLimt", OracleDbType.Double, ParameterDirection.Input, transAmtLimt);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<CustomDropDownModel>(connection, mainDbUser.DbUser + "SP_Get_DisburseTransDDL", param: parameter, commandType: CommandType.StoredProcedure);

                    connection.Close();

                    return result;
                }
                //var parameter = new OracleDynamicParameters();
                //parameter.Add("transAmtLimt", OracleDbType.Double, ParameterDirection.Input, transAmtLimt);
                //parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                //var result = SqlMapper.Query<CustomDropDownModel>(connection, "SP_Get_DisburseTransDDL", param: parameter, commandType: CommandType.StoredProcedure);

                //connection.Close();

                //return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

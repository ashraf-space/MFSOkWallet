using Dapper;
using MFS.EnvironmentService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Repository
{

    public interface IBankBranchRepository : IBaseRepository<Bankbranch>
    {
        object GetBankBranchDropdownList();
        object GetBankBranchByBranchCode(string branchCode);
    }

    public class BankBranchRepository : BaseRepository<Bankbranch>, IBankBranchRepository
    {
		private readonly string dbUser;
		public BankBranchRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public object GetBankBranchDropdownList()
        {
            try
            {
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("CUR_BANKBRANCH", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<CustomDropDownModel>(connection, dbUser+"PKG_ENVIORONMENT.PR_GetBankBranchListForDDL", param: parameter, commandType: CommandType.StoredProcedure);

					this.CloseConnection(connection);
					return result;
				}
					
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object GetBankBranchByBranchCode(string branchCode)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("branCode", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<Bankbranch>(connection, dbUser + "SP_Get_BankBranch_ByBranchCode", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
					connection.Close();
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

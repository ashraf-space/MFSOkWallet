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
    public interface IBillCollectionCommonRepository : IBaseRepository<TblCashEntry>
    {
        object GetFeaturePayDetails(int featureId);
        object GetSubMenuDDL(int featureId);
        object GetBillPayCategoriesDDL(int userId);
        object GetDataForCommonGrid(string username, string methodName, int? countLimit, string billNo);
        object GetTitleSubmenuTitleByMethod(string methodName);
    }
    public class BillCollectionCommonRepository : BaseRepository<TblCashEntry>, IBillCollectionCommonRepository
    {

        MainDbUser mainDbUser = new MainDbUser();

        public object GetFeaturePayDetails(int featureId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    //string query = @"Select * from " + mainDbUser.DbUser + "FEATURE_PAY where FEATURE_ID= " + featureId;
                    string query = @"Select fp.*, 
                        case  fc.name when 'Utility Bill Collection' then 11
                          when 'Tuition Fee Collection' then 12
                            when 'Credit Card Bill Collection' then 13
                              when 'Other Bill/Fee Collection' then 14
                          end as ParentPenuId from " + mainDbUser.DbUser + "FEATURE_PAY fp inner join " + mainDbUser.DbUser + "feature f on fp.feature_id = f.id inner join" + mainDbUser.DbUser + "feature_category fc on f.category_id = fc.id where FEATURE_ID= " + featureId;
                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetSubMenuDDL(int featureId)
        {
            //List<CustomDropDownModel> monthYearList = new List<CustomDropDownModel>();
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select name as label,subMenuId as value from " + mainDbUser.DbUser + "feature_pay_submenu where Status='A' and FEATURE_ID= " + featureId + " order by serial,name";
                    var result = connection.Query<CustomDropDownModel>(query);
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetBillPayCategoriesDDL(int userid)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select f.alias as label,FP.METHODNAME as value from  one.feature f 
                                    INNER JOIN  one.Permission p on p.feature_id = f.id 
                                    INNER JOIN  one.feature_category fc on fc.id = f.category_id 
                                    INNER JOIN ONE.FEATURE_PAY fp on FP.FEATURE_ID = F.ID
                                    where (f.Category_id in (31, 32, 33, 34))
                                     and p.role_id = (select role_id from  one.application_user where id = " + userid + ") and p.is_view_permitted = 'y' order by f.category_id , f.order_no";
                    var result = connection.Query<CustomDropDownModel>(query);
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetDataForCommonGrid(string username, string methodName, int? countLimit, string billNo)
        {
            try
            {
                string billNumber = billNo == "null" ? null : billNo;
                List<BranchPortalReceipt> result = new List<BranchPortalReceipt>();
                using (var connection = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_PAID_BY_OR_BC", OracleDbType.Varchar2, ParameterDirection.Input, username);
                    dyParam.Add("V_METHOD", OracleDbType.Varchar2, ParameterDirection.Input, methodName);
                    dyParam.Add("V_LIMIT", OracleDbType.Double, ParameterDirection.Input, countLimit);
                    dyParam.Add("V_RETURN", OracleDbType.RefCursor, ParameterDirection.Output);
                    dyParam.Add("V_BILLNO", OracleDbType.Varchar2, ParameterDirection.Input, billNumber);


                    result = SqlMapper.Query<BranchPortalReceipt>(connection, mainDbUser.DbUser + "PROC_GET_RECENT_BP ", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
                    this.CloseConnection(connection);
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public object GetTitleSubmenuTitleByMethod(string methodName)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"Select BillTitle,SubmenuTitle from  " + mainDbUser.DbUser + "FEATURE_PAY where MethodName = '" + methodName + "'";
                    var result = connection.Query<dynamic>(query).FirstOrDefault();
                    this.CloseConnection(connection);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

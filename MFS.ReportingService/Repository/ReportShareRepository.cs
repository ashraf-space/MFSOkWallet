using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Repository
{
    public interface IReportShareRepository : IBaseRepository<ReportInfo>
    {
        int GetReportIdByNameCat(string reportType, string reportName);
        object SaveReportRole(int item, int id);
        IEnumerable<dynamic> GetReportRolesById(int id);
        object DeleteReportRole(int id);
		List<ApplicationUserReport> GetApplicationUserReports(string branchCode, string userName, string name, string mobileNo, string fromDate, string toDate, string roleName);
		List<AuditTrailReport> GetAuditTrailReport(string branchCode, string user, string parentMenu, string action, string fromDate, string toDate, string auditId);
	}
    public class ReportShareRepository : BaseRepository<ReportInfo>, IReportShareRepository
    {
        private readonly string dbUser;
        public ReportShareRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }
        public int GetReportIdByNameCat(string reportType, string reportName)
        {
            using (var connection = this.GetConnection())
            {
                string query = @"select t.id from " + dbUser + "report_info t where t.report_name = '" + reportName + "' and t.report_type = '" + reportType + "'";

                var result = connection.Query<int>(query).FirstOrDefault();

                this.CloseConnection(connection);
                return Convert.ToInt32(result);
            }
        }

        public object SaveReportRole(int item, int id)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"insert into " + dbUser + "report_role  (report_id,role_id) values (" + id + "," + item + ")";

                    connection.Query(query);

                    this.CloseConnection(connection);
                    return 1;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IEnumerable<dynamic> GetReportRolesById(int id)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"select t.role_id from " + dbUser + "report_role t where t.report_id = " + id + "";

                    var result = connection.Query(query);
                    List<dynamic> roles = new List<dynamic>();
                    foreach (var item in result)
                    {
                        roles.Add(item.ROLE_ID);
                    }
                    this.CloseConnection(connection);
                    return roles;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeleteReportRole(int id)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query = @"delete from " + dbUser + "report_role t where t.report_id = " + id + " ";

                    connection.Query(query);

                    this.CloseConnection(connection);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public List<ApplicationUserReport> GetApplicationUserReports(string branchCode, string userName, string name, string mobileNo, string fromDate, string toDate, string roleId)
		{
			using (var connection = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();

				dyParam.Add("V_BRANCHCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "" ? null : branchCode);
				dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
				dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
				dyParam.Add("V_USERNAME", OracleDbType.Varchar2, ParameterDirection.Input, userName == ""?null:userName);
				dyParam.Add("V_NAME", OracleDbType.Varchar2, ParameterDirection.Input, name == ""?null:name);
				dyParam.Add("V_MOBILENO", OracleDbType.Varchar2, ParameterDirection.Input, mobileNo==""?null:mobileNo);
				dyParam.Add("V_ROLEID", OracleDbType.Varchar2, ParameterDirection.Input, roleId==""?null: roleId);
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

				List<ApplicationUserReport> result = SqlMapper.Query<ApplicationUserReport>(connection, dbUser + "RPT_APP_USER", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				return result;
			}
		}

		public List<AuditTrailReport> GetAuditTrailReport(string branchCode, string user, string parentMenu, string action, string fromDate, string toDate, string auditId)
		{
			using (var connection = this.GetConnection())
			{
				var dyParam = new OracleDynamicParameters();
				dyParam.Add("FROMDATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(fromDate));
				dyParam.Add("TODATE", OracleDbType.Date, ParameterDirection.Input, Convert.ToDateTime(toDate));
				dyParam.Add("V_AUDIT_ID", OracleDbType.Varchar2, ParameterDirection.Input, auditId == "null" ? null : auditId);
				dyParam.Add("V_USERNAME", OracleDbType.Varchar2, ParameterDirection.Input, user == "null" ? null : user);
				dyParam.Add("V_BCODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode == "null" ? null : branchCode);
				dyParam.Add("V_ACTION", OracleDbType.Varchar2, ParameterDirection.Input, action == "null" ? null : action);
				dyParam.Add("V_PARENT_MENU", OracleDbType.Varchar2, ParameterDirection.Input, parentMenu == "null" ? null : parentMenu);
				dyParam.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);

				List<AuditTrailReport> result = SqlMapper.Query<AuditTrailReport>(connection, dbUser + "RPT_AUDIT_TRAIL", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
				this.CloseConnection(connection);
				return result;
			}
		}
	}
}

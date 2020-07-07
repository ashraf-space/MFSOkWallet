using Dapper;
using MFS.ReportingService.Models;
using OneMFS.SharedResources;
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
	}
	public class ReportShareRepository : BaseRepository<ReportInfo>, IReportShareRepository
	{
		
		public int GetReportIdByNameCat(string reportType, string reportName)
		{
			using (var connection = this.GetConnection())
			{
				string query = @"select t.id from report_info t where t.report_name = '" + reportName + "' and t.report_type = '" + reportType + "'";

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
					string query = @"insert into report_role  (report_id,role_id) values (" + id + "," + item + ")";

					connection.Query(query);

					this.CloseConnection(connection);
					return 1;
				}
					
			}
			catch(Exception ex)
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
					string query = @"select t.role_id from report_role t where t.report_id = " + id + "";

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
					string query = @"delete from report_role t where t.report_id = " + id + " ";

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
	}
}

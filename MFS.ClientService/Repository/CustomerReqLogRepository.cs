using Dapper;
using MFS.ClientService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.ClientService.Repository
{
    public interface ICustomerReqLogRepository : IBaseRepository<CustomerReqLog>
    {
        void updateRequestLog(CustomerRequest model);
        void deleteRequestLog(CustomerRequest model);
		object GetAllOnProcessRequestByCustomer(string mphone);
		object GetCustomerRequestHistoryByCat(string status, string mphone);
		object GetCustomerRequestHistoryByCatAndDate(string status, string mphone, DateTime? regdate, DateTime? closeDate);
	}

    public class CustomerReqLogRepository : BaseRepository<CustomerReqLog>, ICustomerReqLogRepository
    {
		private readonly string dbUser;
		public CustomerReqLogRepository(MainDbUser objMainDbUser)
		{
			dbUser = objMainDbUser.DbUser;
		}
		public void deleteRequestLog(CustomerRequest model)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "delete from " + dbUser + "Customer_Req_Log where mphone='" + model.Mphone + "' and Request='" + model.Request + "' and status='" + model.Prev_status + "' and Gid='" + model.Gid + "'";

					var result = this.ExecuteScaler(query);

					this.CloseConnection(connection);
				}
			}
			catch(Exception ex)
			{
				throw;
			}
			
				
            
        }

		public object GetAllOnProcessRequestByCustomer(string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"
					select t.req_date as ""reqDate"",
					   t.remarks  as ""remarks"",
					   t.request  as ""request"",
					   t.mphone   as ""mphone"",
					   t.status   as ""status""
				  from " + dbUser + "CUSTOMER_REQ_LOG t where t.mphone = '" + mphone + "' and t.status = 'P'";

					var result = connection.Query<dynamic>(query);

					this.CloseConnection(connection);

					return result;
				}
			}
			catch(Exception ex)
			{
				throw;
			}
			
		}

		public object GetCustomerRequestHistoryByCat(string status, string mphone)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = string.Empty;
					if (status == "A")
					{
						 query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone from one.customer_request t where t.mphone = '" + mphone + "'";						
					}
					else if (status == "D")
					{
						query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone
								 from one.customer_request t where t.mphone = '" + mphone + "' and (t.request = 'Dormant' or t.request = 'Dormant Withdraw')";

					}
					else 
					{
						query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone
								 from one.customer_request t where t.mphone = '" + mphone + "' and (t.request = 'Pin Reset' or t.request = 'Pin Reset/Unlock')";
					}
					var result = connection.Query<CustomerRequest>(query).ToList().OrderByDescending(e => e.ReqDate);
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}


		}

		public object GetCustomerRequestHistoryByCatAndDate(string status, string mphone, DateTime? regdate, DateTime? closeDate)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = string.Empty;
					if (status == "A")
					{
						query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone from one.customer_request t where t.mphone = '" + mphone + "'";
					}
					else if (status == "D")
					{
						query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone
								 from one.customer_request t where t.mphone = '" + mphone + "' and (t.request = 'Dormant' or t.request = 'Dormant Withdraw')";

					}
					else
					{
						query = @"select t.req_date as reqdate,t.handled_by as handledby, t.request,t.status,t.remarks,t.mphone
								 from one.customer_request t where t.mphone = '" + mphone + "' and (t.request = 'Pin Reset' or t.request = 'Pin Reset/Unlock')";
					}
					var result = connection.Query<CustomerRequest>(query).ToList().Where(e => e.ReqDate >= regdate).Where(e => e.ReqDate <= closeDate).OrderByDescending(e => e.ReqDate);
					this.CloseConnection(connection);
					connection.Dispose();
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public void updateRequestLog(CustomerRequest model)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					// string query = "update Customer_Req_Log set Status='" + model.Status + "', handled_by='" + model.HandledBy + "' where mphone='" + model.Mphone + "' and Request='" + model.Request + "' and status='" + model.Prev_status + "'";
					string query = "update " + dbUser + "Customer_Req_Log set Status='" + model.Status + "', handled_by='" + model.HandledBy + "' where mphone='" + model.Mphone + "' and Gid='" + model.Gid + "'";

					var result = this.ExecuteScaler(query);

					this.CloseConnection(connection);
				}
					
			}
			catch(Exception ex)
			{
				throw;
			}
			
        }
    }

}

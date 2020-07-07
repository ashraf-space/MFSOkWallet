using Dapper;
using MFS.ClientService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.ClientService.Repository
{
    public interface ICustomerReqLogRepository : IBaseRepository<CustomerReqLog>
    {
        void updateRequestLog(CustomerRequest model);
        void deleteRequestLog(CustomerRequest model);
		object GetAllOnProcessRequestByCustomer(string mphone);
	}

    public class CustomerReqLogRepository : BaseRepository<CustomerReqLog>, ICustomerReqLogRepository
    {
        public void deleteRequestLog(CustomerRequest model)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = "delete from Customer_Req_Log where mphone='" + model.Mphone + "' and Request='" + model.Request + "' and status='" + model.Prev_status + "' and Gid='" + model.Gid + "'";

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
				  from CUSTOMER_REQ_LOG t
				 where t.mphone = '" + mphone + "' and t.status = 'P'";

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

		public void updateRequestLog(CustomerRequest model)
        {
			try
			{
				using (var connection = this.GetConnection())
				{
					// string query = "update Customer_Req_Log set Status='" + model.Status + "', handled_by='" + model.HandledBy + "' where mphone='" + model.Mphone + "' and Request='" + model.Request + "' and status='" + model.Prev_status + "'";
					string query = "update Customer_Req_Log set Status='" + model.Status + "', handled_by='" + model.HandledBy + "' where mphone='" + model.Mphone + "' and Gid='" + model.Gid + "'";

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

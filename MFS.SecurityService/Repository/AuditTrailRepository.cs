using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IAuditTrailRepository : IBaseRepository<AuditTrail>
    {
        object InsertIntoAuditTrail(AuditTrail model);
        object InsertIntoAuditTrailDetail(AuditTrailDetail auditTrailDetail);
		object GetUserListDdl();
		object GetAuditTrails(DateRangeModel date, string user, string action, string menu);
		object GetTrailDtlById(string id);
	}

    public class AuditTrailRepository : BaseRepository<AuditTrail>, IAuditTrailRepository
    {
		public object GetAuditTrails(DateRangeModel date, string user, string action, string menu)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("FROM_DATE", OracleDbType.Date, ParameterDirection.Input, date.FromDate);
					dyParam.Add("UPTO_DATE", OracleDbType.Date, ParameterDirection.Input, date.ToDate);
					dyParam.Add("USERNAME", OracleDbType.Varchar2, ParameterDirection.Input, user.Trim());
					dyParam.Add("ACTION", OracleDbType.Varchar2, ParameterDirection.Input, action.Trim());
					dyParam.Add("MENU", OracleDbType.Varchar2, ParameterDirection.Input, menu.Trim());
					dyParam.Add("TRAILS", OracleDbType.RefCursor, ParameterDirection.Output);

					var result = SqlMapper.Query<dynamic>(connection, "PR_GET_AUDITTRAILS", param: dyParam, commandType: CommandType.StoredProcedure).ToList();
					this.CloseConnection(connection);

					return result;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public object GetTrailDtlById(string id)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.which_field_name as ""fieldName"", t.which_value as ""preValue"", t.what_value as ""curValue"" from audit_trail_dtl t where t.audit_trail_id = '"+id+"'";
					var result = connection.Query<dynamic>(query).ToList();
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

		public object GetUserListDdl()
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					string query = @"select t.username as ""label"", t.username ""value"" from application_user t";
					var result = connection.Query<CustomDropDownModel>(query).ToList();
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

		public object InsertIntoAuditTrail(AuditTrail model)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_WHO", OracleDbType.Varchar2, ParameterDirection.Input, model.Who);
                    dyParam.Add("V_WHAT_ACTION_ID", OracleDbType.Int32, ParameterDirection.Input, model.WhatActionId);
                    dyParam.Add("V_WHICH_PARENT_MENU_ID", OracleDbType.Int32, ParameterDirection.Input, model.WhichParentMenuId);
                    dyParam.Add("V_WHICH_MENU", OracleDbType.Varchar2, ParameterDirection.Input, model.WhichMenu);
                    dyParam.Add("V_WHICH_ID", OracleDbType.Varchar2, ParameterDirection.Input, model.WhichId);
                    dyParam.Add("V_RESPONSE", OracleDbType.Varchar2, ParameterDirection.Input, model.Response);
                    dyParam.Add("V_AUDIT_TRAIL_ID", OracleDbType.Varchar2, ParameterDirection.Output, null, 32767);
                    dyParam.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input, model.Particular);

                    SqlMapper.Query(conn, "PROC_AUDIT_TRAIL_V2", param: dyParam, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    var auditTrailId = dyParam.oracleParameters[6].Value.ToString();
                    //var auditTrailId = dyParam.Get<OracleString>("V_AUDIT_TRAIL_ID").ToString();

                    return auditTrailId;
                }               
            }
            catch (Exception ex)
            {
				throw;
            }
        }

        public object InsertIntoAuditTrailDetail(AuditTrailDetail auditTrailDetail)
        {
            try
            {
                using (var conn = this.GetConnection())
                {
                    var dyParam = new OracleDynamicParameters();
                    dyParam.Add("V_AUDIT_TRAIL_ID", OracleDbType.Varchar2, ParameterDirection.Input, auditTrailDetail.AuditTrailId);
                    dyParam.Add("V_WHICH_FIELD_NAME", OracleDbType.Varchar2, ParameterDirection.Input, auditTrailDetail.WhichFeildName);
                    dyParam.Add("V_WHICH_VALUE", OracleDbType.Varchar2, ParameterDirection.Input, auditTrailDetail.WhichValue);
                    dyParam.Add("V_WHAT_VALUE", OracleDbType.Varchar2, ParameterDirection.Input, auditTrailDetail.WhatValue);
                    dyParam.Add("V_PARTICULAR", OracleDbType.Varchar2, ParameterDirection.Input, auditTrailDetail.Particular);

                    var result = SqlMapper.Query(conn, "PROC_AUDIT_TRAIL_DTL", param: dyParam, commandType: CommandType.StoredProcedure);
                    this.CloseConnection(conn);

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

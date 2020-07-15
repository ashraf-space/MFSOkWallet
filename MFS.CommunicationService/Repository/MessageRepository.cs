using Dapper;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace MFS.CommunicationService.Repository
{
	public class MessageRepository : ConnectionManager
	{
		//private readonly string dbUser;
		//public MessageRepository(MainDbUser objMainDbUser)
		//{
		//	dbUser = objMainDbUser.DbUser;
		//}
		private readonly MainDbUser mainDbUser = new MainDbUser();
		public MessageRepository()
		{			
		}
		public dynamic SendSms(MessageModel model)
		{
			try
			{
				using (var connection = this.GetConnection())
				{
					var dyParam = new OracleDynamicParameters();
					dyParam.Add("V_MPHONE", OracleDbType.Varchar2, ParameterDirection.Input, model.Mphone);
					dyParam.Add("V_SMS", OracleDbType.Varchar2, ParameterDirection.Input, model.MessageBody);
					dyParam.Add("V_MSGID", OracleDbType.Varchar2, ParameterDirection.Input, model.MessageId);
					dyParam.Add("V_MSGSTRING", OracleDbType.Varchar2, ParameterDirection.Input, model.MessageString);
					dyParam.Add("V_FORCE", OracleDbType.Varchar2, ParameterDirection.Input, model.Force);

					var result = SqlMapper.Query(connection, mainDbUser.DbUser+"PROC_SEND_MESSAGE", param: dyParam, commandType: CommandType.StoredProcedure);
					this.CloseConnection(connection);

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

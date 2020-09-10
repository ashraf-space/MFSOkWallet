using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MFS.TransactionService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;

namespace MFS.TransactionService.Repository
{
    public interface IToolsRepository : IBaseRepository<MtCbsinfo>
    {
        object GetMappedAccountInfoByMphone(string mphone);
        object GetNameByMphone(string mblNo);
        object GetCbsCustomerInfo(string accNo);
        object GetPendingCbsAccounts(string branchCode);
        object CheckAccountValidityByCount(string mblNo);
        int InactiveCbsAccountByAccountNo(string inactiveCbsAccountNo, string makeBy, string ubranch);
        object GetMappedAccountByMblNo(string mblNo);
        object CheckAccNoIsMappedByMblNo(string mblAcc, string accno);
        object CheckStatusByAcc(string mtcbsinfoAccno);
        int ActiveCbsAccountByAccountNo(string inactiveCbsAccountNo, string makeBy, string ubranch);
        int CheckEligibilityMappingByMphone(string mphone);
        object CheckPendingAccountByMphone(string mblAcc);
        object CheckActivatdAccountByMphone(string mblNo);
        object CheckCbsValidClass(string @class);
		void ChekCbsAccuntByAccNo(MtCbsinfo mtcbsinfo);
		object GetMappedAccountByAccNo(string accNo);
	}
    public class ToolsRepository : BaseRepository<MtCbsinfo>, IToolsRepository
    {
        MainDbUser mainDbUser = new MainDbUser();

        public object GetMappedAccountInfoByMphone(string mphone)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mphone);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_GET_MAPPEDCBS_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure);

                    _connection.Close();

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object GetNameByMphone(string mblNo)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mblNo);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_GET_NAME_BY_MPHONE", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    _connection.Close();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object GetCbsCustomerInfo(string accNo)
        {
            try
            {

                //string URL = "http://10.20.32.118/CBS/";
                //string urlParameters = "?proc=CBSINFO&ACCNO=" + accNo.ToString();
                //string URL = "http://10.20.32.158/CbsDemoAPi/api/DemoCbs?accno=" + accNo.ToString();
                //string URL = "http://10.156.4.253/CBS/?proc=CBSINFO&ACCNO=" + accNo.ToString();
				string URL = "http://10.156.4.16/CBS/?proc=CBSINFO&ACCNO=" + accNo.ToString();
				//string urlParameters = "proc=CBSINFO?&accno=" + accNo.ToString();
				object dataObjects = null;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/html"));

                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataObjects = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    client.Dispose();
                    return null;
                }

                string[] values = dataObjects.ToString().Split(',');
                if (values[0] == "1")
                {
                    MtCbsinfo aMtCbsinfo = new MtCbsinfo
                    {
                        Custid = values[1],
                        Name = values[2],
                        Accno = accNo,
                        Branch = values[3],
                        Class = values[4],
                        Accstat = values[5],
                        Frozen = values[6],
                        Dorm = values[7],
                        Mobnum = "0" + values[8],
                        Nationid = values[9]
                    };
                    client.Dispose();
                    return aMtCbsinfo;
                }

                else
                {
                    client.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object GetPendingCbsAccounts(string branchCode)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("BRANCH_CODE", OracleDbType.Varchar2, ParameterDirection.Input, branchCode);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_GET_CBS_ACC_MAPPING", param: parameter,
                        commandType: CommandType.StoredProcedure).ToList();
                    _connection.Close();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object CheckAccountValidityByCount(string mblNo)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mblNo);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_GET_CBS_ACC_COUNT", param: parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    _connection.Close();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int InactiveCbsAccountByAccountNo(string inactiveCbsAccountNo, string makeBy, string ubranch)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("ACC_NO", OracleDbType.Varchar2, ParameterDirection.Input, inactiveCbsAccountNo);
					parameter.Add("V_MAKE_BY", OracleDbType.Varchar2, ParameterDirection.Input, makeBy);
					parameter.Add("V_UBRANCH", OracleDbType.Varchar2, ParameterDirection.Input, ubranch);
					SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_INACTIVE_CBS_ACC_BY_ACCNO", param: parameter,
                        commandType: CommandType.StoredProcedure);

                    _connection.Close();

                    return 1;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int ActiveCbsAccountByAccountNo(string inactiveCbsAccountNo, string makeBy, string ubranch)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("ACC_NO", OracleDbType.Varchar2, ParameterDirection.Input, inactiveCbsAccountNo);
					parameter.Add("V_MAKE_BY", OracleDbType.Varchar2, ParameterDirection.Input, makeBy);
					parameter.Add("V_UBRANCH", OracleDbType.Varchar2, ParameterDirection.Input, ubranch);
					SqlMapper.Query<dynamic>(_connection, mainDbUser.DbUser + "SP_ACTIVE_CBS_ACC_BY_ACCNO", param: parameter,
                        commandType: CommandType.StoredProcedure);

                    _connection.Close();

                    return 1;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object GetMappedAccountByMblNo(string mblNo)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mblNo);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query<MtCbsinfo>(_connection, mainDbUser.DbUser + "SP_GET_MAP_ACC_BY_MBLNO", param: parameter,
                        commandType: CommandType.StoredProcedure).ToList();

                    _connection.Close();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
		public object GetMappedAccountByAccNo(string AccNo)
		{
			try
			{
				using (var _connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("V_ACC_NO", OracleDbType.Varchar2, ParameterDirection.Input, AccNo);
					parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
					var result = SqlMapper.Query<MtCbsinfo>(_connection, mainDbUser.DbUser + "SP_GET_MAP_ACC_BY_ACCNO", param: parameter,
						commandType: CommandType.StoredProcedure).FirstOrDefault();

					_connection.Close();

					return result;
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public object CheckAccNoIsMappedByMblNo(string mblAcc, string accno)
        {
            try
            {

                using (var _connection = this.GetConnection())
                {
                    string query = "SELECT COUNT(*) FROM "+ mainDbUser.DbUser + "MT_CBSINFO T WHERE T.ACCNO = '" + accno + "'";
                    var result = _connection.QueryFirstOrDefault<int>(query);

                    this.CloseConnection(_connection);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object CheckStatusByAcc(string mtcbsinfoAccno)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    var parameter = new OracleDynamicParameters();
                    parameter.Add("MOBLIE_NO", OracleDbType.Varchar2, ParameterDirection.Input, mtcbsinfoAccno);
                    parameter.Add("CUR_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                    var result = SqlMapper.Query(_connection, mainDbUser.DbUser + "SP_COUNT_MAP_ACC_BY_MBLNO", param: parameter,
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    _connection.Close();
                    return result;
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int CheckEligibilityMappingByMphone(string mphone)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    string query = "select count(*) from "+ mainDbUser.DbUser + "mt_cbsinfo t where t.mphone = '" + mphone + "' and t.status = 'A'";
                    var result = _connection.QueryFirstOrDefault<int>(query);

                    _connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public object CheckPendingAccountByMphone(string mblAcc)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    string query = "select count(*) from "+ mainDbUser.DbUser + "mt_cbsinfo t where   t.check_status = 'P' and t.mphone = '" + mblAcc + "'";
                    var result = _connection.QueryFirstOrDefault<int>(query);

                    _connection.Close();
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object CheckActivatdAccountByMphone(string mblNo)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    string query = "select count(*) from "+ mainDbUser.DbUser + "mt_cbsinfo t where t.mphone = '" + mblNo + "' and t.status = 'A'";
                    var result = _connection.QueryFirstOrDefault<int>(query);
                    _connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object CheckCbsValidClass(string className)
        {
            try
            {
                using (var _connection = this.GetConnection())
                {
                    string query = "select count(*) from "+ mainDbUser.DbUser + "mt_cbsclass t where t.class = '" + className.ToUpper() + "' and t.status = 'A'";
                    var result = _connection.QueryFirstOrDefault<int>(query);
                    _connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

		public void ChekCbsAccuntByAccNo(MtCbsinfo mtcbsinfo)
		{
			try
			{
				using (var _connection = this.GetConnection())
				{
					var parameter = new OracleDynamicParameters();
					parameter.Add("ACC_NO", OracleDbType.Varchar2, ParameterDirection.Input, mtcbsinfo.Accno);
					parameter.Add("V_CHECK_BY", OracleDbType.Varchar2, ParameterDirection.Input, mtcbsinfo.CheckBy);
					parameter.Add("V_MAKE_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, mtcbsinfo.MakeStatus);
					parameter.Add("V_CHECK_STATUS", OracleDbType.Varchar2, ParameterDirection.Input, mtcbsinfo.CheckStatus);										
					
					var result = SqlMapper.Query(_connection, mainDbUser.DbUser + "SP_CHECK_CBS_ACC_BY_ACCNO", param: parameter,
						commandType: CommandType.StoredProcedure).FirstOrDefault();

					_connection.Close();					
				}

			}
			catch (Exception e)
			{
				throw;
			}
		}
	}
}

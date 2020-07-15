using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IFeatureRepository : IBaseRepository<Feature>
    {
        dynamic GetAuthFeatureList(int id);
    }

    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {

        //private static string dbuser;
        //public FeatureRepository(MainDbUser objMainDbUser)
        //{
        //    dbuser = objMainDbUser.DbUser;
        //}
        MainDbUser mainDbUser = new MainDbUser();
        public dynamic GetAuthFeatureList(int id)
        {
            using (var conn = this.GetConnection())
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("USER_ID", OracleDbType.Int32, ParameterDirection.Input, id);
                dyParam.Add("FEATURE_LIST", OracleDbType.RefCursor, ParameterDirection.Output);

                var result = SqlMapper.Query(conn, mainDbUser.DbUser + "PR_MFS_GETUSERFEATURELIST", param: dyParam, commandType: CommandType.StoredProcedure);
                this.CloseConnection(conn);

                return result;
            }

        }

    }
}

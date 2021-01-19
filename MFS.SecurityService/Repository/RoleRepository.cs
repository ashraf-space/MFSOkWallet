using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        object GetDropdownListByRoleName(string roleName);
    }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {

        MainDbUser mainDbUser = new MainDbUser();
        public object GetDropdownListByRoleName(string roleName)
        {

            try
            {
                string query = null;
                TextCaseConversion convert = new TextCaseConversion();
                if ((roleName == "Admin") || (roleName == "System Admin") || (roleName == "Super Admin"))
                {
                    query = "Select Name as Label, Id as Value from " + mainDbUser.DbUser + "role";
                }
                else
                {
                    query = "Select Name as Label, Id as Value from " + mainDbUser.DbUser + "role where Name in ('Branch Teller','Branch KYC Maker','Branch KYC Checker')";
                }


                using (var connection = this.GetConnection())
                {
                    var list = connection.Query<DropdownListModel>(query);

                    this.CloseConnection(connection);
                    return list;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

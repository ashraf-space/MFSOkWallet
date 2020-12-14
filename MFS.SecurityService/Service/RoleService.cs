using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IRoleService : IBaseService<Role>
    {
        object GetDropdownListByRoleName(string roleName);
    }

    public class RoleService : BaseService<Role>, IRoleService
    {

        public IRoleRepository repo;
        public RoleService(IRoleRepository _repo)
        {
            repo = _repo;
        }

        public object GetDropdownListByRoleName(string roleName)
        {
            return repo.GetDropdownListByRoleName(roleName);
        }


    }
}

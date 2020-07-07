using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IUserRolesService : IBaseService<UserRoles>
    {
        
    }

    public class UserRolesService : BaseService<UserRoles>, IUserRolesService
    {
        public IUserRolesRepository repo;
        public UserRolesService(IUserRolesRepository _repo)
        {
            repo = _repo;
        }
        
    }
}

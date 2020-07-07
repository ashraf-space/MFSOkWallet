using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IUserRolesRepository : IBaseRepository<UserRoles>
    {

    }

    public class UserRolesRepository : BaseRepository<UserRoles>, IUserRolesRepository
    {

    }
}

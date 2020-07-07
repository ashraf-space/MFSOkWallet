using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {

    }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {

    }
}

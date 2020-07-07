using MFS.SecurityService.Models;
using MFS.SecurityService.Models.Utility;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IPermissionService : IBaseService<Permission>
    {
        IEnumerable<PermissionViewModel> GetPermissionWorklist(int roleId);
    }

    public class PermissionService : BaseService<Permission>, IPermissionService
    {
        public IPermissionRepository repo;
        public PermissionService(IPermissionRepository _repo)
        {
            repo = _repo;
        }

        public IEnumerable<PermissionViewModel> GetPermissionWorklist(int roleId)
        {
            return repo.GetPermissionWorklist(roleId);
        }
    }
}

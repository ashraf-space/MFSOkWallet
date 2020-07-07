using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IRestrictedDeviceService : IBaseService<RestrictedDevice>
    {

    }

    public class RestrictedDeviceService : BaseService<RestrictedDevice>, IRestrictedDeviceService
    {
        public IRestrictedDeviceRepository repo;
        public RestrictedDeviceService(IRestrictedDeviceRepository _repo)
        {
            repo = _repo;
        }

    }
    
}

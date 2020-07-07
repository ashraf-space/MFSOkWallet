using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IFeatureService : IBaseService<Feature>
    {
        dynamic GetAuthFeatureList(int id);
    }

    public class FeatureService : BaseService<Feature>, IFeatureService
    {
        public IFeatureRepository repo;
        public FeatureService(IFeatureRepository _repo)
        {
            repo = _repo;
        }

        public dynamic GetAuthFeatureList(int id)
        {
            return repo.GetAuthFeatureList(id);
        }
    }
}

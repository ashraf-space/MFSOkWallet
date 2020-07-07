using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IFeatureCategoryService : IBaseService<FeatureCategory>
    {
        
    }

    public class FeatureCategoryService : BaseService<FeatureCategory>, IFeatureCategoryService
    {
        public IFeatureCategoryRepository  featureCategoryRepo;
        public FeatureCategoryService(IFeatureCategoryRepository _featureCategoryRepo)
        {
            featureCategoryRepo = _featureCategoryRepo;
        }
    }
}

using Dapper;
using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IFeatureCategoryRepository : IBaseRepository<FeatureCategory>
    {
        
    }

    public class FeatureCategoryRepository : BaseRepository<FeatureCategory>, IFeatureCategoryRepository
    {
        
    }
}

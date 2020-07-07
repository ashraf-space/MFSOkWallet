using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface ITestRepository: IBaseRepository<Test>
    {
       
    }

    public class TestRepository: BaseRepository<Test>, ITestRepository 
    {
        
    }
}

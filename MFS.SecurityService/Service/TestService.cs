using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface ITestService : IBaseService<Test>
    {
        
    }

    public class TestService : BaseService<Test>, ITestService
    {
        public ITestRepository testRepo;
        public TestService(ITestRepository _testRepo)
        {
            testRepo = _testRepo;
        }        
    }
}

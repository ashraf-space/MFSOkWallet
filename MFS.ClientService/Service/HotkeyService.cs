using MFS.ClientService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IHotkeyService : IBaseService<Hotkey>
    {
        
    }

    public class HotkeyService : BaseService<Hotkey>, IHotkeyService
    {
        public IHotkeyRepository  repo;
        public HotkeyService(IHotkeyRepository _repo)
        {
            repo = _repo;
        }
    }
}

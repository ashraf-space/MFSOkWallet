using Dapper;
using MFS.ClientService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IHotkeyRepository : IBaseRepository<Hotkey>
    {
        
    }

    public class HotkeyRepository : BaseRepository<Hotkey>, IHotkeyRepository
    {
        
    }
}

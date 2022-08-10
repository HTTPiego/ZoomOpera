using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IAccount
    {
        string Name { get; set; }
        string Password { get; set; }
        string Role { get; init; }
    }
}

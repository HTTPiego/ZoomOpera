using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomOpera.DTOs
{
    public class JwtDTO
    {
        public string Token { get; set; }
        public JwtDTO(string token)
        {
            Token = token;
        }
    }
}

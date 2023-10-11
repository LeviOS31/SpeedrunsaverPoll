using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class UserAuth
    {
        public int id { get; set; }
        public string username { get; set; }
        public bool admin { get; set; }
    }
}

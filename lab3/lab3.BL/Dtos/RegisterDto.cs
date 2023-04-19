using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.BL.Dtos
{
    public  record RegisterDto (string UserName ,
        string Email,
        string Password,
        string Address);
    
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.DAL.Data;

public class Employee : IdentityUser
{
    public string address { get; set; } = string.Empty;
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.DAL.Data;

public class CompanyContext : IdentityDbContext<Employee>
{
    public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Employee>().ToTable("Employees");
        builder.Entity<IdentityUserClaim<string>>().ToTable("EmpClaims");
    }
}

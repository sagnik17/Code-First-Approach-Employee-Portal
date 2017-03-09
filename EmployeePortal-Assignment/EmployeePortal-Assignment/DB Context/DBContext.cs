using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EmployeePortal_Assignment.Models;

namespace EmployeePortal_Assignment.DB_Context
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBContext")
        {
            Database.SetInitializer(new SampleData());

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<PaySlip> Payslips { get; set; }
    }
}
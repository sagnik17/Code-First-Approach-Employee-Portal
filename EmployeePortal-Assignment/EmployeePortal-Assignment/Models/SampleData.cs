using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EmployeePortal_Assignment.DB_Context;

namespace EmployeePortal_Assignment.Models
{
    public class SampleData : CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext context)
        {

            try
            {
                List<Departments> d = new List<Departments>()
                {
                     new Departments {
                         Name = "HR", Location = "USA",
                     Employees = new List<Employee>() {
                     new Employee()
                     {
                         FirstName = "Mark",
                        LastName = "Hastings",
                        UserName = "mark",
                        Gender = "Male",
                        Password = "mark123",
                        ConfirmPassword = "mark123",
                        DOJ = Convert.ToDateTime("02/02/2017"),
                        EmailID = "mark@hotmail.com",
                     } } },
                     new Departments {
                         Name = "Finance", Location = "Australia",
                     Employees = new List<Employee>() {
                     new Employee()
                     {
                         FirstName = "Sonam",
                        LastName = "Gupta",
                        UserName = "Sonam",
                        Gender = "Female",
                        Password = "Sonam",
                        ConfirmPassword = "Sonam",
                        DOJ = Convert.ToDateTime("01/01/2017"),
                        EmailID = "Sonam@yahoo.com",
                     } } },
                     new Departments {
                         Name = "Marketing", Location = "India",
                     Employees = new List<Employee>() {
                     new Employee()
                     {
                         FirstName = "Fahim",
                        LastName = "Bhatt",
                        UserName = "Fahim",
                        Gender = "Male",
                        Password = "fahim",
                        ConfirmPassword = "fahim",
                        DOJ = Convert.ToDateTime("01/01/2017"),
                        EmailID = "Fahim@yahoo.com",
                     } } },
                     new Departments {
                         Name = "Research & Development", Location = "Japan",
                     Employees = new List<Employee>() {
                     new Employee()
                     {
                         FirstName = "Kiran",
                        LastName = "Sahu",
                        UserName = "Kiran",
                        Gender = "Male",
                        Password = "Kiran",
                        ConfirmPassword = "Kiran",
                        DOJ = Convert.ToDateTime("01/01/2017"),
                        EmailID = "Kiran@gmail.com",
                     } } }
                };

                foreach (Departments std in d)
                {
                    context.Departments.Add(std);

                }



                context.SaveChanges();
                base.Seed(context);
            }
            catch (Exception e)
            {
                throw e;
            }


        }
    }
}
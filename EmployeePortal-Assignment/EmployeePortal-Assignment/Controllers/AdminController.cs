using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeePortal_Assignment.Models;
using EmployeePortal_Assignment.DB_Context;

namespace EmployeePortal_Assignment.Controllers
{
    public class AdminController : Controller
    {
        DBContext _databaseObj;
        public AdminController()
        {
            _databaseObj = new DBContext();
        }
        // GET: Admin
        public ActionResult Admin_Page()
        {
            if (Session["Admin_LoginCredentials"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserLogin", "Account");
            }
        }

        public ActionResult ShowAllEmpData()
        {
            var model = new List<Employee>();
            try
            {
                    var value = (from e in _databaseObj.Employees join d in _databaseObj.Departments on e.DeptRefId equals d.DId
                                 select new { e.EmpId, e.FirstName, e.LastName, e.Gender,e.EmailID,e.Password,e.UserName,e.DOJ,d.Name,d.Location });
                    foreach (var employee in value)
                    {
                        var empModel = new Employee();
                        empModel.Department = new Departments();
                        empModel.Department.Name = employee.Name;
                        empModel.Department.Location = employee.Location;
                        empModel.EmpId = employee.EmpId;
                        empModel.FirstName = employee.FirstName;
                        empModel.LastName = employee.LastName;
                        empModel.Gender = employee.Gender;
                        empModel.EmailID = employee.EmailID;
                        empModel.UserName = employee.UserName;
                        empModel.Password = employee.Password;
                        empModel.DOJ = employee.DOJ;
                        model.Add(empModel);
                    }
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }


        public ActionResult ShowAllDept()
        {
            var model = new List<Departments>();
            try
            {
                using (var context = new DBContext())
                {
                    var value = context.Departments.ToList();
                    foreach (var dept in value)
                    {
                        var deptModel = new Departments();
                        deptModel.DId = dept.DId;
                        deptModel.Name = dept.Name;
                        deptModel.Location = dept.Location;
                        model.Add(deptModel);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }

    }
}
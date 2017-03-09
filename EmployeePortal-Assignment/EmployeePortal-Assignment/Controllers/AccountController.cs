using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EmployeePortal_Assignment.Models;
using EmployeePortal_Assignment.DB_Context;
using System.Globalization;

namespace EmployeePortal_Assignment.Controllers
{
    public class AccountController : Controller
    {
        DBContext _databaseObj;
        public AccountController()
        {
            _databaseObj = new DBContext();
        }

        #region User Registration
        // GET: Account
        [Route("Account/Register")]
        public ActionResult User_Register()
        {
            List<Departments> dplist = _databaseObj.Departments.ToList();
            SelectList list = new SelectList(dplist, "DId", "Name");
            ViewBag.DList = list;
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
        public ActionResult User_Register(Employee _emp_Object)
        {
            var deptid = Convert.ToInt32(Request.Form["DeptId"]);
            var usrdata = (from data in _databaseObj.Employees
                              where data.UserName == _emp_Object.UserName || data.EmailID == _emp_Object.EmailID
                              select new { data.UserName }).FirstOrDefault();
            
            if(usrdata != null)
            {
                ViewBag.msg = "Username/Email ID already used !!!";
                List<Departments> dplist = _databaseObj.Departments.ToList();
                SelectList list = new SelectList(dplist, "DId", "Name");
                ViewBag.DList = list;
                return View();
            }
            else
            {
                Employee _empData = new Employee()
                {
                    FirstName = _emp_Object.FirstName,
                    LastName = _emp_Object.LastName,
                    UserName = _emp_Object.UserName,
                    Gender = _emp_Object.Gender,
                    EmailID = _emp_Object.EmailID,
                    DOJ = RandomDay(),
                    Password = _emp_Object.Password,
                    ConfirmPassword = _emp_Object.ConfirmPassword,
                    DeptRefId = deptid
                };
                _databaseObj.Employees.Add(_empData);
                _databaseObj.SaveChanges();
                TempData["status_msg"] = "Registration Successful !!!";
                return RedirectToAction("Status_Page");
            }
            
        }
        public ActionResult Status_Page()
        {
            ViewBag.status_msg = TempData["status_msg"];
            return View();
        }
        #endregion

        #region User Login

        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("UserLogin")]
        public ActionResult UserLogin(Employee _empObj)
        {
            var LoginCredentials = _databaseObj.Employees.FirstOrDefault(emp => emp.UserName == _empObj.UserName && emp.Password == _empObj.Password);

            if(_empObj.UserName.Equals("admin") && _empObj.Password.Equals("admin"))
            {
                Session["LoginCredentials"] = _empObj;
                return RedirectToAction("Admin_Page","Admin");
            }
            else if(LoginCredentials != null)
            {
                Session["LoginCredentials"] = LoginCredentials;
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.error = "Invalid Login Credentials";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("UserLogin");
        }
        public ActionResult Welcome()
        {
            if(Session["LoginCredentials"] == null)
            {
                return RedirectToAction("UserLogin");
            }
            return View();
        }
        #endregion

        #region User Details

        public ActionResult UserDetails()
        {
            if(Session["LoginCredentials"] != null)
            {
                var details = Session["LoginCredentials"] as Employee;
                details.Department = _databaseObj.Departments.FirstOrDefault(dbo => dbo.DId == details.DeptRefId);
                ViewBag.EditStatus = TempData["EditStatus"];
                return View(details);
            }
            else
            {
                return RedirectToAction("UserLogin");
            }
        }
        #endregion

        #region Edit User Details
        public ActionResult EditDetails()
        {
            if (Session["LoginCredentials"] != null)
            {
                var details = Session["LoginCredentials"] as Employee;
                details.Department = _databaseObj.Departments.FirstOrDefault(dbo => dbo.DId == details.DeptRefId);
                
                return View(details);
            }
            else
            {
                return RedirectToAction("UserLogin");
            }
        }
        [HttpPost]
        public ActionResult EditDetails(Employee _empObj)
        {
            Employee emp = (from e in _databaseObj.Employees
                    where e.EmpId == _empObj.EmpId
                    select e).First();
            
                    emp.FirstName = _empObj.FirstName;
                    emp.LastName = _empObj.LastName;
                    emp.UserName = _empObj.UserName;
                    emp.Gender = _empObj.Gender;
                    emp.EmailID = _empObj.EmailID;
                Session["LoginCredentials"] = emp;
                _databaseObj.SaveChanges();
                TempData["EditStatus"] = "Your Profile Updated Successfully !!!";
                return RedirectToAction("UserDetails");
        }

        #endregion

        #region Update Password
        [Route("Account/UpdatePassword")]
        public ActionResult UpdatePassword()
        {
            if (Session["LoginCredentials"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserLogin");
            }

        }
        [HttpPost]
        public ActionResult UpdatePassword(Employee _empObj)
        {
                var empdetails = Session["LoginCredentials"] as Employee;
                Employee emp = (from e in _databaseObj.Employees
                                where e.EmpId == empdetails.EmpId
                                select e).First();
                emp.Password = _empObj.Password;
                emp.ConfirmPassword = _empObj.ConfirmPassword;
                Session["LoginCredentials"] = emp;
                _databaseObj.SaveChanges();
                TempData["EditStatus"] = "Your Password Updated Successfully !!!";
                return RedirectToAction("UserDetails");
                
        }
        #endregion

        #region Search Users
        public ActionResult SearchDepartment()
        {
            var LoginUserData = Session["LoginCredentials"] as Employee;
            var model = new List<Employee>();
            var _userList = (from e in _databaseObj.Employees
                             join d in _databaseObj.Departments on e.DeptRefId equals d.DId
                             where e.DeptRefId == LoginUserData.DeptRefId
                             select new {e.FirstName, e.LastName,e.Gender, e.EmailID, e.DOJ, d.Name, d.Location });

            foreach (var userObj in _userList)
            {
                var empModel = new Employee();
                empModel.Department = new Departments();
                empModel.Department.Name = userObj.Name;
                empModel.Department.Location = userObj.Location;
                empModel.FirstName = userObj.FirstName;
                empModel.LastName = userObj.LastName;
                empModel.Gender = userObj.Gender;
                empModel.EmailID = userObj.EmailID;
                empModel.DOJ = userObj.DOJ;
                model.Add(empModel);
            }
            return View(model);
        }

        public ActionResult SearchOrganization()
        {
            var model = new List<Employee>();
            var _userList = (from e in _databaseObj.Employees
                             join d in _databaseObj.Departments on e.DeptRefId equals d.DId
                             select new { e.FirstName, e.LastName, e.Gender, e.EmailID, e.DOJ, d.Name, d.Location });

            foreach (var userObj in _userList)
            {
                var empModel = new Employee();
                empModel.Department = new Departments();
                empModel.Department.Name = userObj.Name;
                empModel.Department.Location = userObj.Location;
                empModel.FirstName = userObj.FirstName;
                empModel.LastName = userObj.LastName;
                empModel.Gender = userObj.Gender;
                empModel.EmailID = userObj.EmailID;
                empModel.DOJ = userObj.DOJ;
                model.Add(empModel);
            }
            return View(model);
        }
        #endregion

        #region Payslip Details
        private void InsertPayslipDetails(Employee empData,DateTime date)
        {
            var obj = Session["LoginCredentials"] as Employee;
            var payslipDetails = (from emp in _databaseObj.Employees
                                  join slip in _databaseObj.Payslips
                                  on emp.EmpId equals slip.empRefID
                                  where slip.DateofSalaryCredit.Month == date.Month && slip.DateofSalaryCredit.Year == date.Year && emp.EmpId == obj.EmpId
                                  select new
                                  { emp.FirstName }).FirstOrDefault();

            if(payslipDetails == null)
            {
                Random rnum = new Random();
                int PFDeduction_rn = rnum.Next(300, 1000);
                PaySlip _payslipObj = new PaySlip()
                {
                    BasicSalary = 15000,
                    HouseRentAllowance = 6000,
                    DateofSalaryCredit = date,
                    Bonus = 13500,
                    PFNumber = "HJ/0JK53" + rnum.Next(300) + "/GH023" + rnum.Next(500),
                    PFDeduction = PFDeduction_rn,
                    TravelAllowance = 2500,
                    SpecialAllowance = 6000,
                    TotalEarning = 41200 - PFDeduction_rn,
                    empRefID = empData.EmpId
                };
                _databaseObj.Payslips.Add(_payslipObj);
                _databaseObj.SaveChanges();
            }
           
        }

        [Route("Account/DisplayPaySlip")]
        public ActionResult DisplayPaySlip()
        {
            if (Session["LoginCredentials"] != null)
            {
                DateTime date;
                var empData = Session["LoginCredentials"] as Employee;
                int diff = DateTime.Now.Month - empData.DOJ.Month;
                if (diff > 0)
                {
                    for (int i = empData.DOJ.Month; i < DateTime.Now.Month; i++)
                    {
                        if(i == 2)
                        {
                            date = DateTime.ParseExact("28/0" + i + "/2017", "dd/MM/yyyy", null);
                        }
                        else if(i==1 || i==3 || i==5 || i==7 || i==8 || i==10 || i==12)
                        {
                            date = DateTime.ParseExact("31/0" + i + "/2017", "dd/MM/yyyy", null);
                        }
                        else
                        {
                            date = DateTime.ParseExact("30/0" + i + "/2017", "dd/MM/yyyy", null);
                        }
                       
                        InsertPayslipDetails(empData, date);
                    }
                }
                FillDropDown();
                return View();
            }
            else
            {
                return RedirectToAction("UserLogin");
            }
        }
        [HttpPost]
        [Route("Account/DisplayPaySlip")]
        public ActionResult DisplayPaySlip(int Month, int Year)
        {
            if (Session["LoginCredentials"] != null)
            {
                var obj = Session["LoginCredentials"] as Employee;
                var payslipDetails = (from emp in _databaseObj.Employees
                                      join slip in _databaseObj.Payslips
                                      on emp.EmpId equals slip.empRefID
                                      where slip.DateofSalaryCredit.Month == Month && slip.DateofSalaryCredit.Year == Year && emp.EmpId == obj.EmpId
                                      select new
                                      {
                                          emp.FirstName,
                                          emp.LastName,
                                          emp.DOJ,
                                          slip.BasicSalary,
                                          slip.DateofSalaryCredit,
                                          slip.Bonus,
                                          slip.TotalEarning,
                                          slip.SpecialAllowance,
                                          slip.PFNumber,
                                          slip.PFDeduction,
                                          slip.HouseRentAllowance,
                                          slip.TravelAllowance
                                      });
               
                PaySlip _payObj = new PaySlip();
                _payObj.Employees = new Employee();
                foreach (var temp in payslipDetails)
                {
                    _payObj.Employees.FirstName = temp.FirstName;
                    _payObj.Employees.LastName = temp.LastName;
                    _payObj.Employees.DOJ = temp.DOJ;
                    _payObj.BasicSalary = temp.BasicSalary;
                    _payObj.SpecialAllowance = temp.SpecialAllowance;
                    _payObj.TravelAllowance = temp.TravelAllowance;
                    _payObj.HouseRentAllowance = temp.HouseRentAllowance;
                    _payObj.PFDeduction = temp.PFDeduction;
                    _payObj.PFNumber = temp.PFNumber;
                    _payObj.TotalEarning = temp.TotalEarning;
                    _payObj.DateofSalaryCredit = temp.DateofSalaryCredit;
                    _payObj.Bonus = temp.Bonus;
                }
                FillDropDown();
                if(_payObj.BasicSalary != 0)
                {
                    return View(_payObj);
                }
                else
                {
                    ViewBag.payslipMsg = "Pay SLip Not Generated Till Yet";
                    return View(_payObj);
                }
               
            }
            else
            {
                return RedirectToAction("UserLogin");
            }
        }


        #endregion

        #region Other Methods
        private Random gen = new Random();
        DateTime RandomDay()
        {
            DateTime start = new DateTime(2017, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private void FillDropDown()
        {
            var emp = Session["LoginCredentials"] as Employee;
            var listofMonths = MonthsRange(emp.DOJ, DateTime.Now);
            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

            ViewBag.months = listofMonths.GroupBy(o => o.Item1).Select(s => s.First()).Select(dbo => new SelectListItem { Text = dateTimeFormat.GetMonthName(dbo.Item1), Value = dbo.Item1.ToString() });
            ViewBag.Years = listofMonths.GroupBy(o => o.Item2).Select(s => s.First()).Select(dbo => new SelectListItem { Text = dbo.Item2.ToString(), Value = dbo.Item2.ToString() });
        }
        private static IEnumerable<Tuple<int, int>> MonthsRange(DateTime startDate,DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    iterator.Month,
                    iterator.Year);
                iterator = iterator.AddMonths(1);
            }
        }
        
        #endregion
    }
}
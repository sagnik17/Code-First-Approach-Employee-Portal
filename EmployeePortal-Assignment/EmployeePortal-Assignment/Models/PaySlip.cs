using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeePortal_Assignment.Models
{
    public class PaySlip
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaySlipId { get; set; }
        public double BasicSalary { get; set; }
        public double HouseRentAllowance { get; set; }
        public double TravelAllowance { get; set; }
        public double SpecialAllowance { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateofSalaryCredit { get; set; }
        public double Bonus { get; set; }
        public string PFNumber { get; set; }
        public double PFDeduction { get; set; }
        public double TotalEarning { get; set; }
        [Display(Name = "PaymentDetails")]
        public int empRefID { get; set;}

        [ForeignKey("empRefID")]
        public Employee Employees { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeePortal_Assignment.Models
{
    public class Departments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DId { get; set; }

        //[DisplayName("Department Name")]
        public string Name { get; set; }
        public string Location { get; set; }


        // Navigation Property
        public List<Employee> Employees { get; set; }
    }
}

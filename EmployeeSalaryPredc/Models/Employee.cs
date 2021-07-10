//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeSalaryPredc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Addresses = new HashSet<Address>();
            this.Vechiles = new HashSet<Vechile>();
            this.Salaries = new HashSet<Salary>();
        }
    
        public int EmpId { get; set; }
        [Required(ErrorMessage = "Please enter Employee Name")]
        [DataType(DataType.Text)]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Please enter Date of birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [Required(ErrorMessage = "Please enter Hire Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> HireDate { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vechile> Vechiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salaries { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeSalaryPredc.Models;

namespace EmployeeSalaryPredc.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        PredictionEntities PE = new PredictionEntities();
        // GET: Employee
        [Authorize(Roles = "Admin, User, HR, Finance, TeamLead")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User, HR, Finance, TeamLead, Manager")]
        public ActionResult GetEmp()
        {
            var Emp = (from i in PE.Employees
                       select new
                       {
                           i.EmployeeName,
                           i.BirthDate,
                           i.HireDate,
                           i.Gender,
                           i.MaritalStatus,
                           i.Email,
                           i.PhoneNumber,
                           i.EmpId
                       }).ToList();
            return Json(new { data = Emp }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
                return View(new Employee());
            else
            {
                var Emp = PE.Employees.Where(i => i.EmpId == id).FirstOrDefault();
                return View(Emp);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        public ActionResult AddOrEdit(Employee Emp)
        {
            if(Emp.EmpId == 0)
            {
                PE.Employees.Add(Emp);
                PE.SaveChanges();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PE.Entry(Emp).State = EntityState.Modified;
                PE.SaveChanges();
                return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult RemoveEmp(int id = 0)
        {
            var removeEmp = PE.Employees.Where(i => i.EmpId == id).FirstOrDefault();
            PE.Employees.Remove(removeEmp);
            PE.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
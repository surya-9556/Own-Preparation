using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeSalaryPredc.Models;

namespace EmployeeSalaryPredc.Controllers
{
    public class SalaryController : Controller
    {
        private PredictionEntities PE = new PredictionEntities();
        // GET: Salary
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetEmpSal()
        {
            var salPre = (from i in PE.Salaries
                          select new
                          {
                              Name = i.Employee.EmployeeName,
                              Basic = i.BasicSalary,
                              DA = i.DA,
                              HRA = i.HRA,
                              Travel = i.Travel,
                              salId = i.SalaryId,
                              Total = (i.BasicSalary + i.DA + i.HRA + i.Travel) * 12
                          }).ToList();
            return Json(new { data = salPre }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
            {
                ViewBag.Salary = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                return View(new Salary());
            }
            else
            {
                var remove = PE.Salaries.Where(i => i.SalaryId == id).FirstOrDefault();
                ViewBag.Salary = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                return View(remove);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Salary sal)
        {
            if(sal.SalaryId == 0)
            {
                PE.Salaries.Add(sal);
                PE.SaveChanges();
                ViewBag.Salary = new SelectList(PE.Employees, "EmpId", "EmployeeName", sal.EmployeeId);
                return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PE.Entry(sal).State = EntityState.Modified;
                PE.SaveChanges();
                ViewBag.Salary = new SelectList(PE.Employees, "EmpId", "EmployeeName", sal.EmployeeId);
                return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemSal(int id = 0)
        {
            var remove = PE.Salaries.Where(i => i.SalaryId == id).FirstOrDefault();
            PE.Salaries.Remove(remove);
            PE.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
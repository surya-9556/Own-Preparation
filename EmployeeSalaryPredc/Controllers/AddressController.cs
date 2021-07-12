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
    public class AddressController : Controller
    {
        private PredictionEntities PE = new PredictionEntities();
        [Authorize(Roles = "Admin,HR")]
        // GET: Address
        public ActionResult ViewEmpAddress()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult GetEmpAddr()
        {
            var addr = (from i in PE.Addresses
                        select new
                        {
                            Name = i.Employee.EmployeeName,
                            Address = i.Address1,
                            City = i.City,
                            State = i.State,
                            PinCode = i.PinCode,
                            ADDID = i.AddressId,
                        }).ToList();
            return Json(new { data = addr }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                return View();
            }
            else
            {
                var edit = PE.Addresses.Where(i => i.AddressId == id).FirstOrDefault();
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName",edit.EmployeeId);
                return View(edit);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult AddOrEdit(Address add)
        {
            using (PredictionEntities PE = new PredictionEntities())
            {
                if (add.AddressId == 0)
                {
                    PE.Addresses.Add(add);
                    PE.SaveChanges();
                    ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName", add.EmployeeId);
                    return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    PE.Entry(add).State = EntityState.Modified;
                    PE.SaveChanges();
                    ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName", add.EmployeeId);
                    return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult RemAddr(int id = 0)
        {
            var remove = PE.Addresses.Where(i => i.AddressId == id).FirstOrDefault();
            PE.Addresses.Remove(remove);
            PE.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
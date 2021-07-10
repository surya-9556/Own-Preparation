using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeSalaryPredc.Models;

namespace EmployeeSalaryPredc.Controllers
{
    public class AddressController : Controller
    {
        private PredictionEntities PE = new PredictionEntities();
        // GET: Address
        public ActionResult ViewEmpAddress()
        {
            return View();
        }

        [HttpPost]
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
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Addr = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                return View(new Address());
            }
            else
            {
                var edit = PE.Addresses.Where(i => i.AddressId == id).FirstOrDefault();
                ViewBag.Addr = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                return View(edit);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Address addr)
        {
            if (addr.AddressId == 0)
            {
                PE.Addresses.Add(addr);
                PE.SaveChanges();
                ViewBag.Addr = new SelectList(PE.Employees, "EmpId", "EmployeeName", addr.EmployeeId);
                return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PE.Entry(addr).State = EntityState.Modified;
                PE.SaveChanges();
                ViewBag.Addr = new SelectList(PE.Employees, "EmpId", "EmployeeName", addr.EmployeeId);
                return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemAddr(int id = 0)
        {
            var remove = PE.Addresses.Where(i => i.AddressId == id).FirstOrDefault();
            PE.Addresses.Remove(remove);
            PE.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
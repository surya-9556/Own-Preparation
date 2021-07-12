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
    public class VechileController : Controller
    {
        private PredictionEntities PE = new PredictionEntities();
        [Authorize(Roles = "Admin,HR")]
        // GET: Vechile
        public ActionResult ViewVech()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult GetVech()
        {
            var vech = (from i in PE.Vechiles
                        select new
                        {
                            EmpName = i.Employee.EmployeeName,
                            DrivName = i.Driver.DriverName,
                            VechName = i.VechileName,
                            Number = i.VechileNumber,
                            Route = i.VechileRoute,
                            Capacity = i.Capacity,
                            VechId = i.VechileId
                        }).ToList();

            return Json(new { data = vech }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
            {
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                ViewBag.DriverId = new SelectList(PE.Drivers, "DriverId", "DriverName");
                ViewBag.AddressId = new SelectList(PE.Addresses, "AddressId", "City");
                return View(new Vechile());
            }
            else
            {
                var vech = PE.Vechiles.Where(i => i.VechileId == id).FirstOrDefault();
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                ViewBag.DriverId = new SelectList(PE.Drivers, "DriverId", "DriverName");
                ViewBag.AddressId = new SelectList(PE.Addresses, "AddressId", "City");
                return View(vech);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult AddOrEdit(Vechile vech)
        {
            if(vech.VechileId == 0)
            {
                //var name = PE.Vechiles.Where(i => i.VechileName == vech.VechileName).FirstOrDefault();
                PE.Vechiles.Add(vech);
                PE.SaveChanges();
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                ViewBag.DriverId = new SelectList(PE.Drivers, "DriverId", "DriverName");
                ViewBag.AddressId = new SelectList(PE.Addresses, "AddressId", "City");
                return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PE.Entry(vech).State = EntityState.Modified;
                PE.SaveChanges();
                ViewBag.EmployeeId = new SelectList(PE.Employees, "EmpId", "EmployeeName");
                ViewBag.DriverId = new SelectList(PE.Drivers, "DriverId", "DriverName");
                ViewBag.AddressId = new SelectList(PE.Addresses, "AddressId", "City");
                return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult RemoveVech(int id = 0)
        {
            var vech = PE.Vechiles.Where(i => i.VechileId == id).FirstOrDefault();
            PE.Vechiles.Remove(vech);
            PE.SaveChanges();
            return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}
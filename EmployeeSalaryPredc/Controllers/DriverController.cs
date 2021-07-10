using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeSalaryPredc.Models;

namespace EmployeeSalaryPredc.Controllers
{
    public class DriverController : Controller
    {
        
        // GET: Driver
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDriver()
        {
            try
            {
                using (PredictionEntities PE = new PredictionEntities())
                {
                    var driver = (from i in PE.Drivers
                                  select new
                                  {
                                      Name = i.DriverName,
                                      PhoneNo = i.PhoneNumber,
                                      DriverId = i.DriverId
                                  }).ToList();
                    return Json(new { data = driver }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
                return View(new Driver());
            else
                using (PredictionEntities PE = new PredictionEntities())
                {
                    var driv = PE.Drivers.Where(i => i.DriverId == id).FirstOrDefault();
                    return View(driv);
                }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Driver div)
        {
            using (PredictionEntities PE = new PredictionEntities())
            {
                if(div.DriverId == 0)
                {
                    PE.Drivers.Add(div);
                    PE.SaveChanges();
                    return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    PE.Entry(div).State = EntityState.Modified;
                    PE.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" },JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult RemoveRecord(int id = 0)
        {
            using (PredictionEntities PE = new PredictionEntities())
            {
                var remove = PE.Drivers.Where(i => i.DriverId == id).FirstOrDefault();
                PE.Drivers.Remove(remove);
                PE.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
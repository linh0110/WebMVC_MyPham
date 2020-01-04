using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinhcosMVC.Controllers
{
    public class ClientController : Controller
    {
        //
        // GET: /Client/

        //public ActionResult Home()
        //{
        //    return View();
        //}
        public ActionResult Home1()
        {
            return View();
        }

        public ActionResult Contact()
        {
            //var gt = from t in data.TheLoaiTinTucs select t;
            //return View(gt);
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinhcosMVC.Models;

namespace LinhcosMVC.Controllers
{
    public class TinTucController : Controller
    {
        //
        // GET: /TinTuc/

        // Tạo đối tượng chứa cơ sở dữ liệu tin tức
        LinhcosSQLDataContext data = new LinhcosSQLDataContext();

        // lấy tin mới nhất
        private List<TinTuc> Laytinmoi(int count)
        {
            return data.TinTucs.OrderByDescending(a => a.NgayDang).Take(count).ToList();
        }


        private List<TinTuc> AllTinTuc (int count)
        {
            return data.TinTucs.Take(count).ToList();
        }

        public ActionResult IndexTinTuc()
        {
            //ViewBag.NoiDung = "Chào mừng đến với trang tin tức";

            var alltin = AllTinTuc(3);
            return View(alltin);
            //var alltintuc = from tt in data.TinTucs select tt;
            //return View(alltintuc);

        }

        public ActionResult TinTucMoiNhat()
        {
            //// lấy 3 tin mới nhất
            ////var tinmoi = Laytinmoi(5);
            //return View(tinmoi);
            var tinmoi = Laytinmoi(3);
            return PartialView(tinmoi);
        }

        public ActionResult ChiTietTT(int matin)
        {
            var chitiet = data.TinTucs.Where(m => m.MaTin == matin).First();
            return View(chitiet);
        }
    }
}

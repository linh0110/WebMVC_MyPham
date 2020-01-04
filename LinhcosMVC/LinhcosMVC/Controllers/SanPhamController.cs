using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinhcosMVC.Models;

namespace LinhcosMVC.Controllers
{
    public class SanPhamController : Controller
    {

        LinhcosSQLDataContext data = new LinhcosSQLDataContext();

        private List<SanPham> spham(int count)
        {
            return data.SanPhams.Take(count).ToList();
        }

        public ActionResult Index()
        {
            var SanPhamMoi = spham(16);
            return View(SanPhamMoi);
        }

        public ActionResult SPTheoLoai(string maloai)
        {
            var sptheoDM = data.SanPhams.Where(a => a.MaLoaiSP == maloai).Select(a=>a);
            return View(sptheoDM);
        }

        public ActionResult Details(int masp)
        {
            var chitiet = data.SanPhams.Where(m => m.MaSP == masp).First();
            return View(chitiet);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinhcosMVC.Models;

namespace LinhcosMVC.Controllers
{
    public class GioHangController : Controller
    {

        LinhcosSQLDataContext data = new LinhcosSQLDataContext();
        
        public List<Giohang> Laygiohang()
        {

            List<Giohang> lstGioHang = Session["GioHang"] as List<Giohang>;
              if (lstGioHang == null)
              {
                  lstGioHang = new List<Giohang>();
                  Session["GioHang"] = lstGioHang;
              }
            return lstGioHang;
        }

        public ActionResult Dathang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("dangnhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SanPham");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult Dathang(FormCollection f)
        {
            DonDatHang DDH = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Giohang> gh = Laygiohang();
            DDH.MaKH = kh.MaKH;
            DDH.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("0:MM/dd/yyyy", f["NgayGiao"]);
            DDH.Ngaygiao = DateTime.Parse(ngaygiao);
            DDH.Tinhtranggiaohang = false;
            DDH.Dathanhtoan = false;
            data.DonDatHangs.InsertOnSubmit(DDH);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDH = DDH.MaDH;
                ctdh.MaSP = item.sMaSP;
                ctdh.Soluong = item.iSoLuong;
                ctdh.Dongia = (int)item.dGia;
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }

        // Them vao gio hang
        public ActionResult ThemGioHang(int sMaSP,string strURL)
        {
            // Lay ra Session gio hang
            List<Giohang> lstGioHang = Laygiohang();
            // Kiem tra sach nay ton tai trong Session["Giohang"] chua?
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.sMaSP == sMaSP);
            if (sanpham == null)
            {
                sanpham = new Giohang(sMaSP);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        // Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang!= null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        //Tinh Tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        
        //Xay dung trang gio hang
        public ActionResult GioHang()
        {
            List<Giohang> lstGioHang = Laygiohang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index","SanPham");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult Xoagiohang(string iMaSP)
        {
            int lol = int.Parse(iMaSP);
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.sMaSP == lol);
            if (sanpham != null)
            {
                lstGiohang.Remove(sanpham); 
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            return RedirectToAction("GioHang");
        }
    }
}

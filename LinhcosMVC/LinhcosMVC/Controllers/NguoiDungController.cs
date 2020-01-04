using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using LinhcosMVC.Models;


namespace LinhcosMVC.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /NguoiDung/

        LinhcosSQLDataContext data = new LinhcosSQLDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, KhachHang kh)
        {
            var Hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["matkhau"];
            var matkhaunhaplai = collection["matkhaunhaplai"];
            var diachi = collection["diachi"];
            var gioitinh=collection["gioitinh"];
            var email = collection["email"];
            var dienthoai = collection["dienthoai"];
            if (String.IsNullOrEmpty(Hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"]="Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (matkhaunhaplai.ToString()!=matkhau.ToString())
            {
                ViewData["Loi4"] = "Nhập lại mật khẩu không đúng";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Điện thoại không được để trống";
            }
            else
            {
                kh.HoTen = Hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.DiaChi = diachi;
                kh.GioiTinh = gioitinh;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = Convert.ToDateTime(collection["ngaysinh"]);
                kh.Email = email;
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("dangnhap");
            }
            return this.Register();
        }

        public ActionResult dangnhap(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gan gia tri cho doi tuong duoc tao moi(ad)
                KhachHang ad = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (ad != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = ad;
                    return RedirectToAction("Index","SanPham");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }
    }
}

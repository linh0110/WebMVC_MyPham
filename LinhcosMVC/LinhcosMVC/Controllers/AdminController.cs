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
    public class AdminController : Controller
    {
        LinhcosSQLDataContext data = new LinhcosSQLDataContext();
        //
        // GET: /Admin/


        public ActionResult SanPham(int ?page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //return View(data.SanPhams.ToList());
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(data.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // gán các giá trị người dùng nhập liệu cho các biến
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
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("SanPham", "Admin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
        public ActionResult Login ()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        // Thêm sản phẩm mới
        [HttpGet]
        public ActionResult ThemMoiSP()
        {
            ViewBag.MaLoaiSP = new SelectList(data.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiSP(SanPham sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoaiSP = new SelectList(data.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            // kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            // Thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //
                    var fileName = Path.GetFileName(fileupload.FileName);
                    // Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Asset/Pic/HinhSP"), fileName);
                    fileupload.SaveAs(path);
                    sp.Hinh = fileName;
                    // luu vao CSDL
                    data.SanPhams.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
        // Hiển thị sản phẩm
        public ActionResult ChiTietSP(int id)
        {
            // lấy ra sản phẩm theo mã
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        public ActionResult XoaSP(int id)
        {
            // Lấy ra đối tượng cần xóa theo mã
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        //Xóa Sản Phẩm
        [HttpGet,ActionName("XoaSP")]
        public ActionResult XacNhanXoaSP(int id)
        {
            //Lấy ra đối tượng cần xóa theo mã
            SanPham sp = data.SanPhams.SingleOrDefault(x => x.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SanPhams.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("SanPham");
            //return View(sp);
        }

        // Sửa sản phẩm
        [HttpGet]
        public ActionResult SuaSP(int id)
        {
            //Lấy ra đối tượng cần sửa theo mã
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoaiSP = new SelectList(data.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(SanPham sp,HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLoaiSP = new SelectList(data.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("SanPham");
        }
    }
}

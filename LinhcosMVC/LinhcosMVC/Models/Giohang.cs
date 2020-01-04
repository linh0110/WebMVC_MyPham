using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinhcosMVC.Models;

namespace LinhcosMVC.Models
{
    public class Giohang
    {
        LinhcosSQLDataContext data = new LinhcosSQLDataContext();
        public int sMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sHinh { set; get; }
        public Double dGia { set; get; }
        public int iSoLuong { set; get; }

        public Double dThanhTien
        {
            get { return iSoLuong * dGia; }
        }

        // Khoi tao gio hang theo ma sach duoc truyen vao voi SoLuong mac dinh la 1

        public Giohang (int MaSP)
        {
            sMaSP = MaSP;
            SanPham sp = data.SanPhams.Single(n => n.MaSP == sMaSP);
            sTenSP = sp.TenSP;
            sHinh = sp.Hinh;
            dGia = double.Parse(sp.Gia.ToString());
            iSoLuong = 1;
        }
    }
}
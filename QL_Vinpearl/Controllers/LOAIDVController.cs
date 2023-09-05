using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_Vinpearl.Models;

namespace QL_Vinpearl.Controllers
{
	public class LOAIDVController : Controller
	{
		private QL_VinpearlEntities db = new QL_VinpearlEntities();

		//Danh sách dịch vụ theo loại dịch vụ
		public ActionResult DanhSachTheoLoaiDV(string loaiDV)
		{
			var dichVu = db.DICHVU.Where(dv => dv.LOAIDV.tenLoai == loaiDV);
			return View(dichVu.ToList());
		}

		// GET: LOAIDV/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			LOAIDV lOAIDV = db.LOAIDV.Find(id);
			if (lOAIDV == null)
			{
				return HttpNotFound();
			}
			return View(lOAIDV);
		}

		// GET: LOAIDV/Create


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

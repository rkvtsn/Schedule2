using System.Web;
using Mvc_Schedule.Models;
using Mvc_Schedule.Models.DataModels.Entities;
using System.Web.Mvc;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Controllers
{
	public class ScheduleController : Controller
	{
		private readonly DomainContext _db = new DomainContext();

		public ScheduleController()
		{
			ViewBag.Title = "Редактор расписания";
		}


		//
		// GET: /Schedule/

		[HttpGet]
		public ActionResult Index(int id = -1, int week = 1)
		{
			var model = _db.Schedule.ListForIndex(id, 1 == week);
			if (model.Group == null)
				return RedirectToRoute(new { controller = "Default", action = "Error", id = 404 });

			ViewBag.Title = model.Group.Name;
			return View(model);
		}

		#region oldStyle
		////
		//// GET: /Schedule/Create

		//[Authorize]
		//[HttpGet]
		//public ActionResult Create(int id = -1, int week = 1)
		//{
		//    var group = _db.Groups.Get(id);
		//    if (group == null)
		//        return RedirectToRoute(new { controller = "Default", action = "Error", id = 404 });

		//    var model = _db.Schedule.ListForCreate(id, 1 == week);

		//    return View(model);
		//}

		////
		//// POST: /Schedule/Create

		//[Authorize]
		//[HttpPost]
		//public ActionResult Create(ScheduleTableCreate scheduletable)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _db.Schedule.ListAdd(scheduletable);
		//        _db.SaveChanges();
		//        return RedirectToAction("Index", "Facult");
		//    }

		//    scheduletable.Lessons = _db.Lessons.List();
		//    scheduletable.Weekdays = _db.Weekdays.List();
		//    scheduletable.Subjects = _db.Subjects.List();

		//    return View(scheduletable);
		//}
		#endregion

		//
		// GET: /Schedule/Create

		[Authorize]
		[HttpGet]
		public ActionResult Create(int id = -1, int week = 1)
		{
			var model = _db.Schedule.ListForCreate(id, 1 == week);
			if (model == null)
				return RedirectToRoute(new { controller = "Default", action = "Error", id = 404 });

			return View(model);
		}

		[HttpPost]
		public JsonResult GetSubjects(string id)
		{
			var model = _db.Subjects.List(id);

			return Json(model);
		}


		//
		// POST: /Schedule/Create

		//[Authorize]
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create(ScheduleTableCreate scheduletable)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _db.Schedule.ListAdd(scheduletable);
		//        _db.SaveChanges();
		//        return RedirectToAction("Index", "Facult");
		//    }

		//    scheduletable.Lessons = _db.Lessons.List();
		//    scheduletable.Weekdays = _db.Weekdays.List();

		//    return View(scheduletable);
		//}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(FormCollection scheduleRows)
		{
			bool isValid = false;
			var scheduletable = _db.Schedule.FormToTable(scheduleRows, out isValid);

			if (isValid)
			{
				_db.Schedule.ListAdd(scheduletable);
				_db.SaveChanges();
				return RedirectToAction("Index", "Facult");
			}

			ViewBag.Error = "Ошибка ввода, заполните все поля";
			scheduletable.Lessons = _db.Lessons.List();
			scheduletable.Weekdays = _db.Weekdays.List();

			return View(scheduletable);
			//return View(new ScheduleTableCreate()); // :Debug Mode
		}


		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}
using System.Web.Mvc;
using Mvc_Schedule.Models;
using Mvc_Schedule.Models.DataModels.Entities;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Controllers
{
	[Authorize]
	public class SubjectsController : Controller
	{
		private readonly DomainContext _db = new DomainContext();

		public SubjectsController()
		{
			ViewBag.Title = "Редактор дисциплин";
		}

		//
		// GET: /Subjects/

		public ViewResult Index(int id = 1)
		{
			var model = new SubjectIndex { ListSubjects = _db.Subjects.List() };

			return View(model);
		}

		//
		// POST: /Subjects/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(SubjectIndex subject)
		{
			if (ModelState.IsValid)
			{
				_db.Subjects.Add(subject.NewSubject);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			subject.ListSubjects = _db.Subjects.List();
			return View(subject);
		}

		//
		// GET: /Subjects/Edit/5

		public ActionResult Edit(int id)
		{
			Subject subject = _db.Subjects.Get(id);
			return View(subject);
		}

		//
		// POST: /Subjects/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Subject subject)
		{
			if (ModelState.IsValid)
			{
				_db.Subjects.Edit(subject);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(subject);
		}

		//
		// GET: /Subjects/Delete/5

		public ActionResult Delete(int id)
		{
			Subject subject = _db.Subjects.Get(id);
			return View(subject);
		}

		//
		// POST: /Subjects/Delete/5

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			_db.Subjects.Delete(id);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}
using System.Web.Mvc;
using Mvc_Schedule.Models;
using Mvc_Schedule.Models.DataModels.Entities;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Controllers
{ 
	[Authorize]
    public class LessonsController : Controller
    {
        private readonly DomainContext _db = new DomainContext();

    	public LessonsController()
    	{
    		ViewBag.Title = "Редактор звонков";
    	}

        //
        // GET: /Lessons/

        public ViewResult Index()
        {
            return View(_db.Lessons.List());
        }

        //
        // GET: /Lessons/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Lessons/Create

        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Create(LessonsCreate lesson)
        {
            if (ModelState.IsValid)
            {
                _db.Lessons.Add(lesson);
                _db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(lesson);
        }
        
        //
        // GET: /Lessons/Edit/5
 
        public ActionResult Edit(int id)
        {
			var lesson = _db.Lessons.Get(id);
            return View(lesson);
        }

        //
        // POST: /Lessons/Edit/5

        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _db.Lessons.Edit(lesson);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lesson);
        }

        //
        // GET: /Lessons/Delete/5
 
        public ActionResult Delete(int id)
        {
            Lesson lesson = _db.Lessons.Get(id);
            return View(lesson);
        }

        //
        // POST: /Lessons/Delete/5

        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _db.Lessons.Delete(id);
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
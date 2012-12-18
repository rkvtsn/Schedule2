using System.Web.Mvc;
using Mvc_Schedule.Models;
using Mvc_Schedule.Models.DataModels.Entities;
using Mvc_Schedule.Models.DataModels.ModelViews;


namespace Mvc_Schedule.Controllers
{ 
	[Authorize]
    public class FacultController : Controller
    {
        private readonly DomainContext _db = new DomainContext();

    	public FacultController()
    	{
			ViewBag.Title = "Редактор факультетов";
    	}

        //
        // GET: /Facult/

        public ViewResult Index()
        {
			var model = _db.Facults.List();
			return View(model);
        }

        //
        // GET: /Facult/Create

		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Facult/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Create(FacultCreate facult)
        {
            if (ModelState.IsValid)
            {
				_db.Facults.Add(facult);
				_db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facult);
        }
        
        //
        // GET: /Facult/Edit/5
 
        public ActionResult Edit(int id)
        {
            var facult = _db.Facults.Get(id);
			if (facult == null)
				return RedirectToRoute(new { controller = "Default", action = "Error", id = 404 });
            return View(facult);
        }

        //
        // POST: /Facult/Edit/5

        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(Facult facult)
        {
            if (ModelState.IsValid)
            {
				_db.Facults.Edit(facult);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facult);
        }

        //
        // GET: /Facult/Delete/5
 
        public ActionResult Delete(int id)
        {
            var facult = _db.Facults.Get(id);
			if (facult == null)
				return RedirectToRoute(new { controller = "Default", action = "Error", id = 404 });
            return View(facult);
        }

        //
        // POST: /Facult/Delete/5

        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            _db.Facults.Delete(id);
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
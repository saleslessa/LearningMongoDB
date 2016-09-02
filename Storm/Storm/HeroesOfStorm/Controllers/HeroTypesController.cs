using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.Interfaces;
using System;
using System.Text;
using System.Web.Mvc;

namespace HeroesOfStorm.Controllers
{
    public class HeroTypesController : Controller
    {
        private readonly IHeroTypeAppService _heroTypeAppService;

        public HeroTypesController(IHeroTypeAppService heroTypeAppService)
        {
            _heroTypeAppService = heroTypeAppService;
        }

        // GET: HeroTypes
        public ActionResult Index()
        {
            return View(_heroTypeAppService.ListAll());
        }

        // GET: HeroTypes/Details/5
        public ActionResult Details(string id)
        {
            return View(_heroTypeAppService.GetById(ObjectId.Parse(id)));
        }

        // GET: HeroTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HeroTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Storm.Business.ViewModels.HeroTypeViewModel model, FormCollection f)
        {
            try
            {
                model.HeroTypeId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("HeroTypeId").RawValue));
                var validation = _heroTypeAppService.Add(model);
                if (validation.IsValid)
                    ViewBag.Success = validation.Message;
                else
                    foreach (var error in validation.Erros)
                        ModelState.AddModelError(string.Empty, error.Message);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: HeroTypes/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_heroTypeAppService.GetById(ObjectId.Parse(id)));
        }

        // POST: HeroTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Storm.Business.ViewModels.HeroTypeViewModel model, FormCollection f)
        {
            try
            {
                model.HeroTypeId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("HeroTypeId").RawValue));
                var validation = _heroTypeAppService.Update(model);

                if (validation.IsValid)
                    ViewBag.Success = validation.Message;
                else
                    foreach (var error in validation.Erros)
                        ModelState.AddModelError(string.Empty, error.Message);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        private string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
                builder.Append(value);

            return builder.ToString();
        }

        // GET: HeroTypes/Delete/5
        public ActionResult Delete(string id)
        {
            return View(_heroTypeAppService.GetById(ObjectId.Parse(id)));
        }

        // POST: HeroTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Storm.Business.ViewModels.HeroTypeViewModel model, FormCollection f)
        {
            try
            {
                model.HeroTypeId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("HeroTypeId").RawValue));
                _heroTypeAppService.Remove(model.HeroTypeId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}

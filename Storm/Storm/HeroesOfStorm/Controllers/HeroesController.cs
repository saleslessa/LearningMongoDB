using MongoDB.Bson;
using Storm.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace HeroesOfStorm.Controllers
{
    public class HeroesController : Controller
    {
        private readonly IHeroAppService _heroAppService;
        private readonly IHeroTypeAppService _heroTypeAppService;

        public HeroesController(IHeroAppService heroAppService, IHeroTypeAppService heroTypeAppService)
        {
            _heroAppService = heroAppService;
            _heroTypeAppService = heroTypeAppService;
        }

        // GET: Heroes
        public ActionResult Index()
        {
            return View(_heroAppService.ListAll());
        }

        // GET: Heroes/Details/5
        public ActionResult Details(string id)
        {
            return View(_heroAppService.GetById(ObjectId.Parse(id)));
        }

        private void LoadHeroTypes(string SelectedType = "")
        {
            var list = _heroTypeAppService.ListAll();
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem());

            foreach (var item in list)
                selectList.Add(new SelectListItem { Value = item.HeroTypeId.ToString(), Text = item.HeroTypeName, Selected = SelectedType == item.HeroTypeId.ToString() });

            ViewBag.HeroType = selectList;

        }

        // GET: Heroes/Create
        public ActionResult Create()
        {
            LoadHeroTypes();
            return View();
        }

        // POST: Heroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Storm.Business.ViewModels.HeroViewModel model, FormCollection f)
        {

            try
            {
                string HeroTypeId = ConvertStringArrayToString((string[])f.GetValue("HeroType").RawValue);
                if (HeroTypeId == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Hero Type not selected. Please chose one");
                    return View(model);
                }
                LoadHeroTypes(HeroTypeId);

                model.HeroType = _heroTypeAppService.GetById(ObjectId.Parse(HeroTypeId));
                var validation = _heroAppService.Add(model);
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

        // GET: Heroes/Edit/5
        public ActionResult Edit(string id)
        {
            var model = _heroAppService.GetById(ObjectId.Parse(id));
            LoadHeroTypes(model.HeroType.HeroTypeId.ToString());
            return View(model);
        }

        // POST: Heroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Storm.Business.ViewModels.HeroViewModel model, FormCollection f)
        {
            try
            {
                string HeroTypeId = ConvertStringArrayToString((string[])f.GetValue("HeroType").RawValue);
                if (HeroTypeId == string.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Hero Type not selected. Please chose one");
                    return View(model);
                }
                model.HeroId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("HeroId").RawValue));

                LoadHeroTypes(HeroTypeId);

                model.HeroType = _heroTypeAppService.GetById(ObjectId.Parse(HeroTypeId));
                var validation = _heroAppService.Update(model);
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

        // GET: Heroes/Delete/5
        public ActionResult Delete(string id)
        {
            return View(_heroAppService.GetById(ObjectId.Parse(id)));
        }

        // POST: Heroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Storm.Business.ViewModels.HeroViewModel model, FormCollection f)
        {
            try
            {
                model.HeroId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("HeroId").RawValue));
                _heroAppService.Remove(model.HeroId);
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

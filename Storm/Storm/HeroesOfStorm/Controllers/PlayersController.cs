using DomainValidation.Validation;
using MongoDB.Bson;
using Storm.Business.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HeroesOfStorm.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerAppService _playerAppService;
        private readonly IHeroAppService _heroAppService;

        public PlayersController(IPlayerAppService playerAppService, IHeroAppService heroAppService)
        {
            _playerAppService = playerAppService;
            _heroAppService = heroAppService;
        }

        // GET: Players
        public ActionResult Index()
        {
            return View(_playerAppService.ListAll());
        }

        // GET: Heroes/Details/5
        public ActionResult Details(string id)
        {
            return View(_playerAppService.GetById(ObjectId.Parse(id)));
        }

        // GET: Heroes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Heroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Storm.Business.ViewModels.PlayerViewModel model)
        {
            try
            {
                var validation = _playerAppService.Add(model);
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
            return View(_playerAppService.GetById(ObjectId.Parse(id)));
        }

        // POST: HeroTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Storm.Business.ViewModels.PlayerViewModel model, FormCollection f)
        {
            try
            {
                model.PlayerId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("PlayerId").RawValue));
                var validation = _playerAppService.Update(model);

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

        // GET: HeroTypes/Delete/5
        public ActionResult Delete(string id)
        {
            return View(_playerAppService.GetById(ObjectId.Parse(id)));
        }

        // POST: HeroTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Storm.Business.ViewModels.PlayerViewModel model, FormCollection f)
        {
            try
            {
                model.PlayerId = ObjectId.Parse(ConvertStringArrayToString((string[])f.GetValue("PlayerId").RawValue));
                _playerAppService.Remove(model.PlayerId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        public ActionResult BuyHeroes(string idPlayer)
        {
            var player = _playerAppService.GetById(ObjectId.Parse(idPlayer));
            var heroes = _heroAppService.ListAll().Where(t => !player.PlayerHeroes.Contains(t) && t.HeroActive == true).ToList();
            ViewBag.idPlayer = idPlayer;
            return View(heroes);
        }

        [HttpPost]
        public JsonResult BuyHeroes(string idPlayer, string idHero)
        {
            try
            {
                var validation = _playerAppService.BuyHero(ObjectId.Parse(idPlayer), ObjectId.Parse(idHero));
                if (validation.IsValid)
                    return Json(new { error = "", message = validation.Message });

                return Json(new { error = "ValidationResultError", message = validation });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Exception", messsage = ex.Message });
            }
        }

        private string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
                builder.Append(value);

            return builder.ToString();
        }

    }
}
using MongoDB.Bson;
using Storm.Business.Interfaces;
using Storm.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Storm.Service.Controllers
{
    [RoutePrefix("api/Players")]
    public class PlayersController : ApiController
    {
        private readonly IPlayerAppService _playerAppService;

        public PlayersController(IPlayerAppService playerAppService)
        {
            _playerAppService = playerAppService;
        }

        // GET: api/Players
        public IEnumerable<PlayerViewModel> Get()
        {
            return _playerAppService.ListAll();
        }

        // GET: api/Players/5
        public PlayerViewModel Get(string id)
        {
            return _playerAppService.GetById(ObjectId.Parse(id));
        }

        // POST: api/Players
        public HttpResponseMessage Post([FromBody]PlayerViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Player"));

            try
            {
                var validation = _playerAppService.Add(model);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // PUT: api/Players/5
        public HttpResponseMessage Put(int id, [FromBody]PlayerViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Player"));

            try
            {
                var validation = _playerAppService.Update(model);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage Put(string id, [FromBody]Dictionary<string, string> value)
        {
            var model = _playerAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            try
            {
                var validation = _playerAppService.Update(model, value);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetAllHeroes")]
        [HttpGet]
        public IEnumerable<HeroViewModel> GetAllHeroes(string id)
        {
            return _playerAppService.GetAllHeroes(ObjectId.Parse(id));
        }

        [Route("GetActiveHeroes")]
        [HttpGet]
        public IEnumerable<HeroViewModel> GetActiveHeroes(string id)
        {
            return _playerAppService.GetActiveHeroes(ObjectId.Parse(id));
        }

        [Route("BuyHero")]
        [HttpPost]
        public HttpResponseMessage BuyHero(string idPlayer, string idHero)
        {
            try
            {
                var validation = _playerAppService.BuyHero(ObjectId.Parse(idPlayer), ObjectId.Parse(idHero));
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        // DELETE: api/Players/5
        public HttpStatusCode Delete(string id)
        {
            var model = _playerAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return HttpStatusCode.NotFound;

            try
            {
                _playerAppService.Remove(ObjectId.Parse(id));
                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}

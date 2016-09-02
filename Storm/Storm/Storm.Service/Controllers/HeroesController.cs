using MongoDB.Bson;
using Storm.Business.Interfaces;
using Storm.Business.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Storm.Service.Controllers
{
    public class HeroesController : ApiController
    {
        private readonly IHeroAppService _heroAppService;

        public HeroesController(IHeroAppService heroAppService)
        {
            _heroAppService = heroAppService;
        }

        // GET: api/Hero
        public IEnumerable<HeroViewModel> Get()
        {
            return _heroAppService.ListAll();
        }

        // GET: api/Hero/5
        public HeroViewModel Get(string id)
        {
            return _heroAppService.GetById(ObjectId.Parse(id));
        }

        public HttpResponseMessage Post([FromBody] HeroViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Hero"));

            try
            {
                var validation = _heroAppService.Add(model);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        // PUT: api/Hero/5
        public HttpResponseMessage Put([FromBody]HeroViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Hero"));

            try
            {
                var validation = _heroAppService.Update(model);
                if (!validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));

                return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // PUT: api/Hero/5
        public HttpResponseMessage Put(string id, [FromBody]Dictionary<string, string> value)
        {
            var model = _heroAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);


            try
            {
                var validation = _heroAppService.Update(model, value);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE: api/Hero/5
        public HttpStatusCode Delete(string id)
        {
            var model = _heroAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return HttpStatusCode.NotFound;

            try
            {
                _heroAppService.Remove(ObjectId.Parse(id));
                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }

        }
    }
}

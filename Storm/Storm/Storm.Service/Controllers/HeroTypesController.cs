using MongoDB.Bson;
using Storm.Business.Interfaces;
using Storm.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Storm.Service.Controllers
{
    public class HeroTypesController : ApiController
    {
        private readonly IHeroTypeAppService _heroTypeAppService;

        public HeroTypesController(IHeroTypeAppService heroTypeAppService)
        {
            _heroTypeAppService = heroTypeAppService;
        }

        // GET: api/HeroTypes
        public IEnumerable<HeroTypeViewModel> Get()
        {
            return _heroTypeAppService.ListAll();
        }

        // GET: api/HeroTypes/5
        public HeroTypeViewModel Get(string id)
        {
            return _heroTypeAppService.GetById(ObjectId.Parse(id));
        }

        // POST: api/HeroTypes
        public HttpResponseMessage Post([FromBody]HeroTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Hero Type"));

            try
            {
                var validation = _heroTypeAppService.Add(model);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }

        // PUT: api/HeroTypes/5
        public HttpResponseMessage Put(HeroTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError("Invalid Hero Type"));

            try
            {
                var validation = _heroTypeAppService.Update(model);
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
            var model = _heroTypeAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);


            try
            {
                var validation = _heroTypeAppService.Update(model, value);
                if (validation.IsValid)
                    return Request.CreateResponse(HttpStatusCode.Created, validation.ToJson());

                return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpError(validation.Erros.ToJson()));
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE: api/HeroTypes/5
        public HttpStatusCode Delete(string id)
        {
            var model = _heroTypeAppService.GetById(ObjectId.Parse(id));
            if (model == null)
                return HttpStatusCode.NotFound;

            try
            {
                _heroTypeAppService.Remove(ObjectId.Parse(id));
                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}

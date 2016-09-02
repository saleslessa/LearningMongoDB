using AutoMapper;
using DomainValidation.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Storm.Business.AppService;
using Storm.Business.AutoMapper;
using Storm.Business.Interfaces;
using Storm.Business.ViewModels;
using Storm.Domain.Entities;
using Storm.Domain.Interfaces.Repository;
using Storm.Domain.Interfaces.Service;
using Storm.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace Storm.UnitTests
{
    [TestClass]
    public class HeroAppServiceTest
    {
        public HeroAppServiceTest()
        {
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod]
        public void HeroAppService_Add_True()
        {
            var validationResult = new ValidationResult();
            var viewModel = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 23,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            };

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModel.HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();
            repoType.Stub(s => s.GetByName(viewModel.HeroType.HeroTypeName)).Return(new HeroType());

            var appService = new HeroAppService(new HeroService(repo, repoType));

            validationResult.Add(appService.Add(viewModel));

            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void HeroAppService_Add_False()
        {
            var validationResult = new ValidationResult();
            var viewModelInconsistent = new HeroViewModel()
            {
                HeroName = "tes#t",
                HeroPrice = 25.1
            };

            var viewModel = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 25,
                HeroType = new HeroTypeViewModel()
            };

            var modelInconsistent = Mapper.Map<HeroViewModel, Hero>(viewModelInconsistent);
            modelInconsistent.IsValid();

            var mockServiceInconsistent = MockRepository.GenerateStub<IHeroService>();
            var appServiceInconsistent = MockRepository.GenerateStub<IHeroAppService>();
            appServiceInconsistent.Stub(e => e.Add(viewModelInconsistent)).Return(modelInconsistent.ValidationResult);

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModel.HeroName)).Return(new Hero());

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var appService = new HeroAppService(new HeroService(repo, repoType));

            validationResult.Add(appService.Add(viewModel));
            validationResult.Add(new HeroAppService(mockServiceInconsistent).Add(viewModelInconsistent));

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "This name was already chosen. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid price. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid name. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid type. Please try again."));
        }

        [TestMethod]
        public void HeroAppService_Update_True()
        {
            var validationResult = new ValidationResult();
            var viewModel = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 25,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test"}
            };
            var viewModelSameObject = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 25,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            };
            var modelSameObject = Mapper.Map<HeroViewModel, Hero>(viewModelSameObject);

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModel.HeroName)).Return(null);

            var repoSameObject = MockRepository.GenerateStub<IHeroRepository>();
            repoSameObject.Stub(s => s.GetByName(viewModelSameObject.HeroName)).Return(modelSameObject);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();
            repoType.Stub(s => s.GetByName("test")).Return(new HeroType());

            var appService = new HeroAppService(new HeroService(repo, repoType));
            var appServiceSameObject = new HeroAppService(new HeroService(repoSameObject, repoType));

            validationResult.Add(appService.Update(viewModel));
            validationResult.Add(appServiceSameObject.Update(viewModelSameObject));

            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void HeroAppService_Update_False()
        {
            var validationResult = new ValidationResult();
            var viewModelInconsistent = new HeroViewModel()
            {
                HeroName = "tes2t",
                HeroPrice = 4
            };
            var viewModelOtherObjectWithSameName = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 25,
                HeroType = new HeroTypeViewModel()
            };
            var modelSameObject = Mapper.Map<HeroViewModel, Hero>(viewModelOtherObjectWithSameName);

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModelInconsistent.HeroName)).Return(null);

            var repoSameObject = MockRepository.GenerateStub<IHeroRepository>();
            repoSameObject.Stub(s => s.GetByName(viewModelOtherObjectWithSameName.HeroName)).Return(new Hero());

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var appService = new HeroAppService(new HeroService(repo, repoType));
            var appServiceSameObject = new HeroAppService(new HeroService(repoSameObject, repoType));

            validationResult.Add(appService.Update(viewModelInconsistent));
            validationResult.Add(appServiceSameObject.Update(viewModelOtherObjectWithSameName));

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "This name was already chosen. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid price. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid name. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Hero has invalid type. Please try again."));
        }

        [TestMethod]
        public void HeroAppService_UpdateKeyValue_True()
        {
            var validationResult = new ValidationResult();
            var viewModel = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 5,
                HeroType = new HeroTypeViewModel() { HeroTypeName = "test" }
            };
            var keyValue = new Dictionary<string, string>();
            keyValue.Add("HeroName", "asd");
            keyValue.Add("HeroDeleted", "true");

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModel.HeroName)).Return(null);

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();
            repoType.Stub(s => s.GetByName("test")).Return(new HeroType());

            var appService = new HeroAppService(new HeroService(repo, repoType));

            validationResult.Add(appService.Update(viewModel, keyValue));

            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void HeroAppService_UpdateKeyValue_False()
        {
            var validationResult = new ValidationResult();
            var viewModel = new HeroViewModel()
            {
                HeroName = "test",
                HeroPrice = 5,
                HeroType = new HeroTypeViewModel()
            };
            var keyValueInconsistent = new Dictionary<string, string>();
            keyValueInconsistent.Add("HeroName", "asd");
            keyValueInconsistent.Add("HeroDelete", "true");


            var keyValueSameName = new Dictionary<string, string>();
            keyValueSameName.Add("HeroName", "test");

            var keyValueInvalidValue = new Dictionary<string, string>();
            keyValueInvalidValue.Add("HeroDeleted", "A");

            var repo = MockRepository.GenerateStub<IHeroRepository>();
            repo.Stub(s => s.GetByName(viewModel.HeroName)).Return(new Hero());

            var repoType = MockRepository.GenerateStub<IHeroTypeRepository>();

            var appService = new HeroAppService(new HeroService(repo, repoType));

            validationResult.Add(appService.Update(viewModel, keyValueInconsistent));
            validationResult.Add(appService.Update(viewModel, keyValueSameName));
            validationResult.Add(appService.Update(viewModel, keyValueInvalidValue));

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "This name was already chosen. Please chose another."));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Attribute HeroDelete does not exist"));
            Assert.IsTrue(validationResult.Erros.Any(e => e.Message == "Invalid values for update"));
        }
    }
}

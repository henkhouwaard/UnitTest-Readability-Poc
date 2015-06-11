using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

using UnitTestReadabilityPoc.Interfaces;
using UnitTestReadabilityPoc.SystemsUnderTest;

namespace UnitTestReadabilityPoc
{
    [TestClass]
    public class TestWithAutoFixture
    {
        private IFixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            //The fixture is being created in the TestInitialize method, because
            //we need to make sure that we aren't working on autofixture setups
            //that were done in other tests.
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public void GetStringById_RepoNotSetup_SuffixesAutoFixtureString()
        {
            //In this case, we don't need to interact with the mock
            //As we can see, Autofixture takes care of everything for us
            //(creating the mock and injecting it in to the constructor of
            //the controller)
            var sut = _fixture.Create<Controller>();

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            actual.EndsWith(Controller.SUFFIX).Should().BeTrue();
        }

        [TestMethod]
        public void SaveString_Always_CallsSaveStringOnRepository()
        {
            //In this case, we have to interact with the mock because we 
            //want to test the interaction (verify) of the controller with the repository
            var repositoryMock = _fixture.Freeze<Mock<IRepository>>();
            var sut = _fixture.Create<Controller>();

            var stringToSave = _fixture.Create<string>();
            sut.SaveString(stringToSave);

            repositoryMock.Verify(repository => repository.SaveString(stringToSave), Times.Once);
        }

        [TestMethod]
        public void GetStringById_RepoIsSetup_SuffixesThatString()
        {
            //In this case we also want to interact with the mock because we
            //want to set up behavior on the mock
            var repositoryMock = _fixture.Freeze<Mock<IRepository>>();
            var stringToReturnFromRepo = _fixture.Create<string>();
            repositoryMock.Setup(repository => repository.GetStringById(It.IsAny<int>()))
                          .Returns(stringToReturnFromRepo);

            var sut = _fixture.Create<Controller>();

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            string expected = String.Concat(stringToReturnFromRepo, Controller.SUFFIX);
            actual.Should().Be(expected);
        }
    }
}
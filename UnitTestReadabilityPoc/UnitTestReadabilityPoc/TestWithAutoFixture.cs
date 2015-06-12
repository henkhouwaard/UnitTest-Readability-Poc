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
        private Mock<IRepository> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
            _repositoryMock = _fixture.Freeze<Mock<IRepository>>();
        }

        [TestMethod]
        public void GetStringById_RepoNotSetup_SuffixesAutoFixtureString()
        {
            var sut = _fixture.Create<Controller>();

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            actual.EndsWith(Controller.SUFFIX).Should().BeTrue();
        }

        [TestMethod]
        public void SaveString_Always_CallsSaveStringOnRepository()
        {
            var sut = _fixture.Create<Controller>();

            var stringToSave = _fixture.Create<string>();
            sut.SaveString(stringToSave);

            _repositoryMock.Verify(repository => repository.SaveString(stringToSave), Times.Once);
        }

        [TestMethod]
        public void GetStringById_RepoIsSetup_SuffixesThatString()
        {
            var stringToReturnFromRepo = _fixture.Create<string>();
            _repositoryMock.Setup(repository => repository.GetStringById(It.IsAny<int>()))
                          .Returns(stringToReturnFromRepo);

            var sut = _fixture.Create<Controller>();

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            string expected = String.Concat(stringToReturnFromRepo, Controller.SUFFIX);
            actual.Should().Be(expected);
        }
    }
}
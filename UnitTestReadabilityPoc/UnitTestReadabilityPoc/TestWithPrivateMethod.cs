using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Ploeh.AutoFixture;

using UnitTestReadabilityPoc.Interfaces;
using UnitTestReadabilityPoc.SystemsUnderTest;

namespace UnitTestReadabilityPoc
{
    [TestClass]
    public class TestWithPrivateMethod
    {
        private readonly IFixture _fixture = new Fixture();
        private Mock<IRepository> _repositoryMock;
        private Mock<IDateProvider> _dateProviderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IRepository>();
            _dateProviderMock = new Mock<IDateProvider>();
        }

        [TestMethod]
        public void GetStringById_RepoNotSetup_SuffixesAutoFixtureString()
        {
            var sut = CreateController();

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            actual.EndsWith(Controller.SUFFIX).Should().BeTrue();
        }

        [TestMethod]
        public void SaveString_Always_CallsSaveStringOnRepository()
        {
            var sut = CreateController();

            var stringToSave = _fixture.Create<string>();
            sut.SaveString(stringToSave);

            _repositoryMock.Verify(repository => repository.SaveString(stringToSave), Times.Once);
        }

        [TestMethod]
        public void GetStringById_RepoIsSetup_SuffixesThatString()
        {
            var sut = CreateController();

            var stringToReturnFromRepo = _fixture.Create<string>();
            _repositoryMock.Setup(repository => repository.GetStringById(It.IsAny<int>()))
                          .Returns(stringToReturnFromRepo);

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            string expected = String.Concat(stringToReturnFromRepo, Controller.SUFFIX);
            actual.Should().Be(expected);
        }

        private Controller CreateController()
        {
            return new Controller(_repositoryMock.Object, _dateProviderMock.Object);
        }
    }
}

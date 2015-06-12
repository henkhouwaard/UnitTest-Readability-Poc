using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Ploeh.AutoFixture;

using UnitTestReadabilityPoc.Interfaces;
using UnitTestReadabilityPoc.SystemsUnderTest;

namespace UnitTestReadabilityPoc
{
    [TestClass]
    public class TestWithNewingSut
    {
        private readonly IFixture _fixture = new Fixture();

        [TestMethod]
        public void GetStringById_RepoNotSetup_SuffixesAutoFixtureString()
        {
            var repositoryMock = new Mock<IRepository>();
            var dateProviderMock = new Mock<IDateProvider>();
            var sut = new Controller(repositoryMock.Object, dateProviderMock.Object);

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            actual.EndsWith(Controller.SUFFIX).Should().BeTrue();
        }

        [TestMethod]
        public void SaveString_Always_CallsSaveStringOnRepository()
        {
            var repositoryMock = new Mock<IRepository>();
            var dateProviderMock = new Mock<IDateProvider>();
            var sut = new Controller(repositoryMock.Object, dateProviderMock.Object);

            var stringToSave = _fixture.Create<string>();
            sut.SaveString(stringToSave);

            repositoryMock.Verify(repository => repository.SaveString(stringToSave), Times.Once);
        }

        [TestMethod]
        public void GetStringById_RepoIsSetup_SuffixesThatString()
        {
            var repositoryMock = new Mock<IRepository>();
            var dateProviderMock = new Mock<IDateProvider>();
            var sut = new Controller(repositoryMock.Object, dateProviderMock.Object);

            var stringToReturnFromRepo = _fixture.Create<string>();
            repositoryMock.Setup(repository => repository.GetStringById(It.IsAny<int>()))
                          .Returns(stringToReturnFromRepo);

            var idToGet = _fixture.Create<int>();
            string actual = sut.GetStringById(idToGet);

            string expected = String.Concat(stringToReturnFromRepo, Controller.SUFFIX);
            actual.Should().Be(expected);
        }
    }
}

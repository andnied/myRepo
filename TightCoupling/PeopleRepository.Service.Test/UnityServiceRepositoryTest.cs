using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PeopleViewer.SharedObjects;
using PersonRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleRepository.Service.Test
{
    [TestClass]
    public class UnityServiceRepositoryTest
    {
        IPersonRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            var ppl = new List<Person>
            {
                new Person { FirstName = "af", LastName = "al", Rating = 0, StartDate = DateTime.Now },
                new Person { FirstName = "bf", LastName = "bl", Rating = 1, StartDate = DateTime.UtcNow }
            };

            var mock = new Mock<IPersonRepository>();
            mock.Setup(r => r.GetPeople()).Returns(ppl);
            mock.Setup(r => r.AddPerson(It.IsAny<Person>())).Callback((Person p) => ppl.Add(p));

            var container = new UnityContainer();
            container.RegisterInstance<IPersonRepository>(mock.Object);
            _repo = container.Resolve<IPersonRepository>();
        }

        [TestMethod]
        public void GetPeople_OnCall_ReturnsAll()
        {
            var test = _repo.GetPeople();

            Assert.AreEqual(2, test.Count());
        }

        [TestMethod]
        public void AddPerson_OnCall_CountIncremented()
        {
            var startCount = _repo.GetPeople().Count();
            _repo.AddPerson(new Person { FirstName = "cf", LastName = "cl", Rating = 2, StartDate = DateTime.UtcNow });
            var thenCount = _repo.GetPeople().Count();

            Assert.AreEqual(thenCount, startCount + 1);
        }
    }
}

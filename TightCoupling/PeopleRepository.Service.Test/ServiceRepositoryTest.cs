using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonRepository.Service.MyService;
using PeopleViewer.SharedObjects;
using System.Collections.Generic;
using Moq;
using PersonRepository.Service;

namespace PeopleRepository.Service.Test
{
    [TestClass]
    public class ServiceRepositoryTest
    {
        IPersonService _personService;

        [TestInitialize]
        public void Setup()
        {
            var people = new List<Person>()
            {
                new Person { FirstName = "af", LastName = "al", Rating = 0, StartDate = DateTime.Now },
                new Person { FirstName = "bf", LastName = "bl", Rating = 1, StartDate = DateTime.UtcNow }
            };

            var mock = new Mock<IPersonService>();
            mock.Setup(s => s.GetPeople()).Returns(people);
            mock.Setup(s => s.GetPerson(It.IsAny<string>()))
                .Returns((string name) => people.FirstOrDefault(p => p.LastName == name));
            mock.Setup(s => s.AddPerson(It.IsAny<Person>()))
                .Callback((Person person) => people.Add(person));
            _personService = mock.Object;
        }

        [TestMethod]
        public void GetPeople_OnExecute_ReturnsAll()
        {
            var repo = new ServiceRepository() { ServiceProxy = _personService };
            var ppl = repo.GetPeople();

            Assert.IsNotNull(ppl);
            Assert.AreEqual(2, ppl.Count());
        }

        [TestMethod]
        public void GetPerson_OnExecuteWithValidValue_ReturnsPerson()
        {
            var repo = new ServiceRepository() { ServiceProxy = _personService };
            var ppl = repo.GetPeople();

            Assert.IsNotNull(ppl);
            Assert.IsTrue(ppl.Count() > 0, "People not populated");

            var person = repo.GetPerson("al");

            Assert.IsNotNull(person);
            Assert.IsTrue(person.LastName == "al");
        }

        [TestMethod]
        public void AddPerson_OnExecute_CountIncrements()
        {
            var repo = new ServiceRepository() { ServiceProxy = _personService };
            var count = repo.GetPeople().Count();
            var personToBeAded = new Person
            {
                FirstName = "cf",
                LastName = "cl",
                Rating = 2,
                StartDate = DateTime.Now
            };
            repo.AddPerson(personToBeAded);
            var countAfter = repo.GetPeople().Count();

            Assert.AreEqual(count + 1, countAfter);
        }
    }
}

using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonRepository.Interface;
using PeopleViewer.SharedObjects;
using System.Collections.Generic;
using Moq;

namespace PeopleViewer.Presentation.Tests
{
    [TestClass]
    public class PeopleViewerViewModelTest
    {
        IPersonRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            var people = new List<Person>()
            {
                new Person { FirstName = "af", LastName = "al", Rating = 0, StartDate = DateTime.Now },
                new Person { FirstName = "bf", LastName = "bl", Rating = 1, StartDate = DateTime.UtcNow }
            };

            var mock = new Mock<IPersonRepository>();
            mock.Setup(r => r.GetPeople()).Returns(people);
            _repo = mock.Object;
        }

        [TestMethod]
        public void People_OnRefresh_IsPopulated()
        {
            var peopleVM = new PeopleViewerViewModel(_repo);
            peopleVM.RefreshPeopleCommand.Execute(null);

            Assert.IsNotNull(peopleVM.People);
            Assert.AreEqual(2, peopleVM.People.Count());
        }

        [TestMethod]
        public void People_OnClear_IsEmpty()
        {
            var peopleVM = new PeopleViewerViewModel(_repo);
            peopleVM.RefreshPeopleCommand.Execute(null);
            Assert.IsNotNull(peopleVM.People);

            peopleVM.ClearPeopleCommand.Execute(null);
            Assert.AreEqual(0, peopleVM.People.Count());
        }
    }
}

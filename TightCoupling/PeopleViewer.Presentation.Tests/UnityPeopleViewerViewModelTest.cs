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

namespace PeopleViewer.Presentation.Tests
{
    [TestClass]
    public class UnityPeopleViewerViewModelTest
    {
        PeopleViewerViewModel _vm;

        [TestInitialize]
        public void Setup()
        {
            var ppl = new List<Person>
            {
                new Person { FirstName = "af", LastName = "al", Rating = 0, StartDate = DateTime.Now },
                new Person { FirstName = "bf", LastName = "bl", Rating = 1, StartDate = DateTime.UtcNow }
            };

            var moq = new Mock<IPersonRepository>();
            moq.Setup(r => r.GetPeople()).Returns(ppl);

            var container = new UnityContainer();
            container.RegisterInstance<IPersonRepository>(moq.Object);

            _vm = container.Resolve<PeopleViewerViewModel>();
        }

        [TestMethod]
        public void UnityPeople_OnRefresh_IsPopulated()
        {
            _vm.RefreshPeopleCommand.Execute(null);

            Assert.IsNotNull(_vm.People);
            Assert.AreEqual(2, _vm.People.Count());
        }

        [TestMethod]
        public void UnityPeople_OnClear_IsEmpty()
        {
            _vm.RefreshPeopleCommand.Execute(null);

            Assert.IsNotNull(_vm.People);
            Assert.AreEqual(2, _vm.People.Count());

            _vm.ClearPeopleCommand.Execute(null);

            Assert.AreEqual(0, _vm.People.Count());
        }
    }
}

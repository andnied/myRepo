using ContactsBook.Controllers;
using ContactsBook.Data.Models;
using ContactsBook.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContactsBook.Tests
{
    [TestClass]
    public class ContactsControllerTest
    {
        private ContactsController _controller;

        [TestInitialize]
        public void Setup()
        {
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@gmail.com" },
                new Contact { Id = 2, FirstName = "Andrew", LastName = "Smith", Email = "andrewsmith@gmail.com" },
                new Contact { Id = 3, FirstName = "Kate", LastName = "Morgan", Address = "abc street 5" },
                new Contact { Id = 4, FirstName = "michael", LastName = "Jackson", Phone = "501 000 000" }
            };

            var mock = new Mock<IContactsRepository>();
            mock.Setup(m => m.GetAll()).Returns(contacts);
            mock.Setup(m => m.GetById(It.IsAny<int>())).Returns((int id) => contacts.FirstOrDefault(c => c.Id == id));
            mock.Setup(m => m.Add(It.IsAny<Contact>())).Callback<Contact>(contacts.Add);
            mock.Setup(m => m.Delete(It.IsAny<int>())).Callback((int id) => contacts.Remove(contacts.FirstOrDefault(c => c.Id == id)));
            mock.Setup(m => m.ContactExists(It.IsAny<int>())).Returns((int id) => contacts.Any(c => c.Id == id));
            mock.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<Contact>())).Callback((int id, Contact c) =>
            {
                var index = contacts.IndexOf(contacts.FirstOrDefault(co => co.Id == id));
                contacts[index] = c;
            });

            _controller = new ContactsController(mock.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [TestMethod]
        public void ControllerGetAllReturnsRecords()
        {
            var response = _controller.Get();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result;

            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void ControllerGetByIdReturnsContact()
        {
            var response = _controller.Get(1);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = response.Content.ReadAsAsync<Contact>().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void ControllerAddContactIncrementsCount()
        {
            var toBeAdded = new Contact
            {
                FirstName = "Artur", LastName = "Tester", Email = "fake@gmail.com"
            };

            var response = _controller.Get();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var startingCount = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result.Count();

            response = _controller.Post(toBeAdded);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            response = _controller.Get();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var thenCount = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result.Count();

            Assert.AreEqual(startingCount + 1, thenCount);
        }

        [TestMethod]
        public void ControllerDeleteContactDecrementsCount()
        {
            var response = _controller.Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var startingCount = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result.Count();

            response = _controller.Post(2);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            response = _controller.Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var thenCount = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result.Count();

            Assert.AreEqual(startingCount - 1, thenCount);
        }

        [TestMethod]
        public void ControllerUpdateContactChangesEntity()
        {
            var response = _controller.Get(1);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = response.Content.ReadAsAsync<Contact>().Result;

            Assert.IsNotNull(result);

            result.FirstName = "test";
            response = _controller.Post(1, result);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            response = _controller.Get(1);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            result = response.Content.ReadAsAsync<Contact>().Result;

            Assert.AreEqual("test", result.FirstName);
        }
    }
}

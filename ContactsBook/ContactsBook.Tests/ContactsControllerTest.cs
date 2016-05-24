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
        public void ControllerGetContactByTextReturnsRecord()
        {
            var response = _controller.Get("mi");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContactsBook.Data.Models;
using System.Collections.Generic;
using ContactsBook.Data;
using System.Linq;
using ContactsBook.Interface;
using ContactsBook.SqlRepository;
using System.Data.Entity;
using System.Diagnostics;

namespace ContactsBook.Tests
{
    [TestClass]
    public class ContactsSqlRepositoryTest
    {
        private IContactsRepository _repo;

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

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(contacts.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(contacts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(contacts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(contacts.GetEnumerator());
            mockDbSet.Setup(m => m.Add(It.IsAny<Contact>())).Callback<Contact>(contacts.Add);
            mockDbSet.Setup(m => m.Remove(It.IsAny<Contact>())).Callback((Contact c) => contacts.Remove(c));

            var mockContext = new Mock<ContactsContext>();
            mockContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            _repo = new ContactsSqlRepository(mockContext.Object);
        }

        [TestMethod]
        public void SqlRepositoryGetAllReturnsCount()
        {
            Assert.AreEqual(4, _repo.GetAll().Count());
        }

        [TestMethod]
        public void SqlRepositoryGetByIdReturnsContact()
        {
            Assert.IsNotNull(_repo.GetById(1));
        }

        [TestMethod]
        public void SqlRepositoryAddIncrementsCount()
        {
            var startingCount = _repo.GetAll().Count();
            var toBeAdded = new Contact { Id = 5, FirstName = "Artur", LastName = "Kor", Phone = "000 000 000" };
            _repo.Add(toBeAdded);

            Assert.AreEqual(startingCount + 1, _repo.GetAll().Count());
        }

        [TestMethod]
        public void SqlRepositoryUpdateChangesEntity()
        {
            var contact = _repo.GetById(1);
            Assert.IsNotNull(contact);

            contact.FirstName = "test";
            _repo.Update(1, contact);

            Assert.AreEqual(_repo.GetById(1).FirstName, "test");
        }

        [TestMethod]
        public void SqlRepositoryDeleteDecrementsCount()
        {
            var startingCount = _repo.GetAll().Count();
            _repo.Delete(1);

            Assert.AreEqual(startingCount - 1, _repo.GetAll().Count());
        }
    }
}

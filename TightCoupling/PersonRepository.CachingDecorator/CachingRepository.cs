using PeopleViewer.SharedObjects;
using PersonRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonRepository.CachingDecorator
{
    public class CachingRepository : IPersonRepository
    {
        private TimeSpan _cacheDuration = TimeSpan.FromSeconds(30);
        private DateTime _dataDateTime;
        private IPersonRepository _personRepository;
        private IEnumerable<Person> _cachedItems;

        private bool IsCacheValid
        {
            get
            {
                var _cacheAge = DateTimeOffset.Now - _dataDateTime;
                return _cacheAge < _cacheDuration;
            }
        }

        private void ValidateCache()
        {
            if (_cachedItems == null || !IsCacheValid)
            {
                try
                {
                    _cachedItems = _personRepository.GetPeople();
                    _dataDateTime = DateTime.Now;
                }
                catch
                {
                    _cachedItems = new List<Person>()
                    {
                        new Person(){ FirstName="No Data Available", LastName = string.Empty, Rating = 0, StartDate = DateTime.Today},
                    };
                }
            }
        }

        private void InvalidateCache()
        {
            _cachedItems = null;
        }

        public CachingRepository(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IEnumerable<Person> GetPeople()
        {
            ValidateCache();
            return _cachedItems;
        }

        public Person GetPerson(string lastName)
        {
            ValidateCache();
            return _cachedItems.FirstOrDefault(p => p.LastName == lastName);
        }

        public void AddPerson(Person newPerson)
        {
            _personRepository.AddPerson(newPerson);
            InvalidateCache();
        }

        public void UpdatePerson(string lastName, Person updatedPerson)
        {
            _personRepository.UpdatePerson(lastName, updatedPerson);
            InvalidateCache();
        }

        public void DeletePerson(string lastName)
        {
            _personRepository.DeletePerson(lastName);
            InvalidateCache();
        }

        public void UpdatePeople(IEnumerable<Person> updatedPeople)
        {
            _personRepository.UpdatePeople(updatedPeople);
            InvalidateCache();
        }
    }
}

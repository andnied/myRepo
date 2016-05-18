using PeopleViewer.SharedObjects;
using PersonRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PersonRepository.Caching
{
    public class CachingRepository : IPersonRepository
    {
        private IPersonRepository _repository;
        private IEnumerable<Person> _people = new List<Person>();
        private Timer _timer;
        private bool _modified = false;

        public CachingRepository(IPersonRepository repository)
        {
            _repository = repository;
            _timer = new Timer(30000);
            _timer.Elapsed += _timer_Elapsed;
            _timer_Elapsed(null, null);
            _timer.Start();
        }

        #region IPersonRepository

        public void AddPerson(Person newPerson)
        {
            _modified = true;
            _repository.AddPerson(newPerson);
        }

        public void DeletePerson(string lastName)
        {
            _modified = true;
            _repository.DeletePerson(lastName);
        }

        public IEnumerable<Person> GetPeople()
        {
            return _people;
        }

        public Person GetPerson(string lastName)
        {
            return _people.FirstOrDefault(p => p.LastName == lastName);
        }

        public void UpdatePeople(IEnumerable<Person> updatedPeople)
        {
            _modified = true;
            _repository.UpdatePeople(updatedPeople);
        }

        public void UpdatePerson(string lastName, Person updatedPerson)
        {
            _modified = true;
            _repository.UpdatePerson(lastName, updatedPerson);
        }

        #endregion

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_modified)
                    _people = _repository.GetPeople();
            }
            catch (Exception)
            {
                _people = new List<Person>()
                    {
                        new Person(){ FirstName="No Data Available", LastName = string.Empty, Rating = 0, StartDate = DateTime.Today},
                    };
            }
        }
    }
}

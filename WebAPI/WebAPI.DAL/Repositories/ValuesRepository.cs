using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Contracts.DAL;
using WebAPI.Model;
using WebAPI.Model.Models;
using WebAPI.Common.SearchParams;
using WebAPI.Common.Extensions;
using WebAPI.Common.Exceptions;
using System.Collections;
using System.Linq.Dynamic;

namespace WebAPI.DAL
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly WebAPIContext _context;

        public ValuesRepository(WebAPIContext context)
        {
            _context = context;
        }

        public async Task<Value> Add(Value entity)
        {
            var added = _context.Values.Add(entity);

            await _context.SaveChangesAsync();

            return added;
        }

        public async Task Delete(int id)
        {
            var value = _context.Values.FindAsync<Value, NotFoundException>(string.Format("Value with id {0} not found.", id), id);

            _context.Values.Remove(await value);

            await _context.SaveChangesAsync();
        }

        public async Task<Value> Get(int id)
        {
            var value = _context.Values.FindAsync<Value, NotFoundException>(string.Format("Value with id {0} not found.", id), id);

            return await value;
        }

        public Task<ApiCollection> Get(BaseSearchParams s)
        {
            return Task.Factory.StartNew(() =>
            {
                var items = _context.Values.AsQueryable();
                var count = items.Count();

                items = items.DynamicSort(s.Sort);
                items = items.Page(s.Page.Value, s.Items.Value);

                IEnumerable result = null;

                if (s.Fields != null)
                {
                    result = items.Select(string.Format("new ({0})", s.Fields));
                }
                else
                {
                    result = items.ToList();
                }
                
                var apiCollection = new ApiCollection(result) { TotalCount = count };

                return apiCollection;
            });
        }

        public async Task<Value> Update(int id, Value entity)
        {
            var value = await _context.Values.FindAsync<Value, NotFoundException>(string.Format("Value with id {0} not found.", id), id);
            
            _context.Entry(value).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return _context.Values.Find(id);
        }
    }
}

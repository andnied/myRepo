using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Contracts.DAL;
using WebAPI.DAL;
using WebAPI.DAL.Models;
using WebAPI.Model.SearchParams;
using WebAPI.Common.Extensions;
using System.Data.Entity;
using WebAPI.Common.Exceptions;

namespace WebAPI.Mocks
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

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Value> Get(int id)
        {
            var value = await _context.Values.FirstAsync<Value, NotFoundException>(v => v.Id == id, string.Format("Value with id {0} not found.", id));
            
            return value;
        }

        public Task<ApiCollection<Value>> Get(BaseSearchParams s)
        {
            return Task.Factory.StartNew(() =>
            {
                var items = _context.Values.AsQueryable();
                var count = items.Count();

                items = items.DynamicSort(s.Sort);
                items = items.Page(s.Page.Value, s.Items.Value);

                IEnumerable<Value> result = null;

                if (s.Fields != null)
                {
                    result = items.SelectAndExecute(s.Fields);
                }
                else
                {
                    result = items.ToList();
                }
                
                var apiCollection = new ApiCollection<Value>(result) { TotalCount = count };

                return apiCollection;
            });
        }

        public Task<Value> Update(int id, Value entity)
        {
            throw new NotImplementedException();
        }
    }
}

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Contracts.DAL;
using WebAPI.Model.Models;
using WebAPI.Mocks;
using WebAPI.Common.SearchParams;
using Xunit;
using WebAPI.Common.Extensions;
using WebAPI.Common.Dto.Read;
using WebAPI.Common.Exceptions;
using Shouldly;

namespace WebAPI.Common.Tests.Extensions
{
    public class IQueryableExtensionsTests
    {
        private readonly IValuesRepository _valuesRepository;
        private readonly IQueryable<Value> _defaultCollection;

        public IQueryableExtensionsTests()
        {
            _valuesRepository = ValuesMock.GetValueRepositoryMock();
            _defaultCollection = _valuesRepository.Get(new BaseSearchParams()).Result.Items.Cast<Value>().AsQueryable();
        }

        [Fact]
        public void dynamic_sort_sorts_by_name()
        {
            var sortedDynamically = _defaultCollection.DynamicSort("name");
            var sorted = _defaultCollection.OrderBy(c => c.Name);
            var result = sortedDynamically.SequenceEqual(sorted);

            Assert.True(result);
        }

        [Fact]
        public void dynamic_sort_sorts_by_name_descending()
        {
            var sortedDynamically = _defaultCollection.DynamicSort("-name");
            var sorted = _defaultCollection.OrderByDescending(c => c.Name);
            var result = sortedDynamically.SequenceEqual(sorted);

            Assert.True(result);
        }

        [Fact]
        public void dynamic_sort_sorts_by_name_then_by_id_descending()
        {
            var sortedDynamically = _defaultCollection.DynamicSort("name, -id");
            var sorted = _defaultCollection.OrderBy(c => c.Name).ThenByDescending(c => c.Id);
            var result = sortedDynamically.SequenceEqual(sorted);

            Assert.True(result);
        }

        [Fact]
        public void dynamic_sort_is_not_case_sensitive()
        {
            var sortedDynamically1 = _defaultCollection.DynamicSort("name, -id");
            var sortedDynamically2 = _defaultCollection.DynamicSort("Name, -Id");
            var result = sortedDynamically1.SequenceEqual(sortedDynamically2);

            Assert.True(result);
        }

        [Fact]
        public void dynamic_sort_is_not_space_sensitive()
        {
            var sortedDynamically1 = _defaultCollection.DynamicSort("name, -id");
            var sortedDynamically2 = _defaultCollection.DynamicSort("name,-id");
            var result = sortedDynamically1.SequenceEqual(sortedDynamically2);

            Assert.True(result);
        }

        [Fact]
        public void paging_page_1_1_element_takes_first_element()
        {
            var paged = _defaultCollection.Page(1, 1).FirstOrDefault();
            var selected = _defaultCollection.ElementAt(0);

            Assert.Equal(paged, selected);
        }

        [Fact]
        public void paging_page_2_2_elements_take_two_last_elements()
        {
            var paged = _defaultCollection.Page(2, 2);
            var selected = _defaultCollection.Skip(2).Take(2);
            var result = paged.SequenceEqual(selected);

            Assert.True(result);
        }

        [Fact]
        public void paging_page_2_3_elements_take_last_element()
        {
            var paged = _defaultCollection.Page(2, 3);
            var selected = _defaultCollection.Skip(3).Take(1);
            var result = paged.SequenceEqual(selected);

            Assert.True(result);
        }

        [Fact]
        public void first_incorrect_throws_exception()
        {
            var col = new List<string>
            {
                "a",
                "b"
            }.AsQueryable();

            var func = new Func<Task>(() => col.FirstAsync<string, NotFoundException>(c => c == "c", "not found"));

            Assert.ThrowsAsync<NotFoundException>(func);
        }
    }
}

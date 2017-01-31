using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.BLL.Facades;
using WebAPI.Common.Exceptions;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.Mocks;
using WebAPI.Model.SearchParams;
using Xunit;

namespace WebAPI.BLL.Tests.Facades
{
    public class ValuesFcdTests
    {
        private readonly IValuesFcd _fcd;
        private readonly IValuesRepository _repo;

        public ValuesFcdTests()
        {
            _repo = ValuesMock.GetValueRepositoryMock();
            _fcd = new ValuesFcd(_repo);
        }

        [Fact]
        public void get_by_id_returns_value()
        {
            var id = 1;
            var item = _fcd.Get(id).Result;

            item.Id.ShouldBe(id);
        }

        [Fact]
        public void get_by_incorrect_id_throws_NotFoundException()
        {
            var incorrectId = -1;
            var func = new Func<Task>(() => _fcd.Get(incorrectId));

            Assert.ThrowsAsync<NotFoundException>(func);
        }
    }
}

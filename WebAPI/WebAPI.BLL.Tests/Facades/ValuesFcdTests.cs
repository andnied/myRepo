using JsonPatch;
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
using WebAPI.Model.Dto.Update;
using WebAPI.Model.Dto.Write;
using WebAPI.Model.SearchParams;
using Xunit;

namespace WebAPI.BLL.Tests.Facades
{
    public class ValuesFcdTests
    {
        private readonly IValuesFcd _fcd;
        private readonly IValuesRepository _repo;
        private readonly ValueWriteDto _newValue = new ValueWriteDto
        {
            Name = "new value",
            Description = "desc"
        };

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

        [Fact]
        public void create_value_adds_value()
        {
            var initialCount = _repo.Get(new BaseSearchParams()).Result.Items.Count();
            var created = _fcd.Create(_newValue).Result;
            var laterCount = _repo.Get(new BaseSearchParams()).Result.Items.Count();

            laterCount.ShouldBe(initialCount + 1);

            created = _fcd.Get(created.Id).Result;
        }

        [Fact]
        public void partial_update_updates_value()
        {
            var newName = "updated name";
            var updateId = 4;
            var model = new JsonPatchDocument<ValueUpdateDto>();
            model.Add("Name", newName);

            var updated = _fcd.Update(updateId, model).Result;

            updated.Name.ShouldBe(newName);

            var value = _fcd.Get(updateId).Result;

            value.Name.ShouldBe(newName);
        }

        [Fact]
        public void full_update_replaces_value()
        {
            var updateId = 4;
            var updated = _fcd.Update(updateId, _newValue).Result;
            var value = _fcd.Get(updateId).Result;

            value.Name.ShouldBe(_newValue.Name);
            value.Description.ShouldBe(_newValue.Description);
        }

        [Fact]
        public void delete_values_deletes_value()
        {
            var deleteId = 4;
            var func = new Func<Task>(() => _fcd.Get(deleteId));

            _fcd.Delete(deleteId).ContinueWith((t) =>
            {
                Assert.ThrowsAsync<NotFoundException>(func);
            });
        }
    }
}

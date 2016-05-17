using BazaMvp.DataAccess;
using BazaMvp.DataAccess.Repos;
using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaIF.Model
{
    public class AllDataModel : IModel
    {
        public IEnumerable<InputFile> AllFiles { get; set; }
        public Task FetchingTask { get; set; }

        //public AllDataModel()
        //{
        //    FetchData();
        //}

        public void FetchData()
        {
            FetchingTask = Task.Factory.StartNew(() =>
            {
                using (var unitOfWork = UnitOfWork.Create())
                {
                    AllFiles = unitOfWork.Repo<FilesRepo>().GetAllFiles();
                }
            });
        }

        public bool DeleteFile(long id)
        {
            try
            {
                using (var unitOfWork = UnitOfWork.Create())
                {
                    unitOfWork.Repo<FilesRepo>().DeleteFile(id);
                    unitOfWork.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: log
                return false;
            }

            return true;
        }
    }
}

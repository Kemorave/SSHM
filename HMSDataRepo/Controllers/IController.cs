using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDataRepo.Controllers
{
    public interface IController<TModel,TResult>  where TModel : Model.IDBModel
    {
        Task<TResult> DeleteByID(int ID);
        Task<IEnumerable<TModel>> GetAllEntries();
        Task<TModel> GetByID(int ID);
        Task<TResult> SetByID(int ID, TModel data);
        Task<TResult> AddEntry(TModel data);
    }
}
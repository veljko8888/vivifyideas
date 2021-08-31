using System.Threading.Tasks;
using testVeljkara.Dtos;
using testVeljkara.Helpers;

namespace testVeljkara.ServiceLayer.Interfaces
{
    public interface IDataService
    {
        Task<Result<DataDto>> Insert(DataDto dataDto);
        Task<Result<DiffDto>> CheckDiff(int id);
    }
}

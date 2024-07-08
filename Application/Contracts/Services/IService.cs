using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Services
{
    public interface IService<Entity, Dto>
    {
        Task<Dto> Get(int id);
        Task<IReadOnlyList<Dto>> GetAll();
        Task<bool> Add(Dto dto);
        Task<bool> Update(Dto dto);
        Task<bool> Delete(int id);
    }
}

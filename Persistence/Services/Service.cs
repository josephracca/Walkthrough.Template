using AutoMapper;
using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.DTOs.Common;
using Application.Exceptions;
using Domain.Common;

namespace Persistence.Services
{
    public class Service<Entity, Dto> : IService<Entity, Dto> where Dto : BaseDto where Entity : BaseAuditEntity
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<Entity> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Add(Dto dto)
        {
            var entity = _mapper.Map<Entity>(dto);
            await _repository.Add(entity);
            return await _unitOfWork.Save() > 0;
        }

        public async Task<Dto> Get(int id)
        {
            var entity = await _repository.Get(id);

            if (entity == null)
            {
                throw new CustomException(404, $"Object not found");
            }

            return _mapper.Map<Dto>(entity);
        }

        public async Task<IReadOnlyList<Dto>> GetAll()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<IReadOnlyList<Dto>>(entities);
        }

        public async Task<bool> Update(Dto dto)
        {
            var entity = await _repository.Get(dto.Id);

            if (entity == null)
                throw new CustomException(400, "Data not found.");

            entity = _mapper.Map<Dto, Entity>(dto, entity);
            await _repository.Update(entity);
            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repository.Get(id);

            if (entity == null)
            {
                throw new CustomException(404, $"Object not found");
            }

            await _repository.Delete(entity);
            return await _unitOfWork.Save() > 0;

        }
    }
}

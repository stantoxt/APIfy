using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APIfy.Core.Repositories
{
    public class Repository<TModel, TKey, TDto> : DbRepository<TModel, TKey>
		where TDto : class
		where TModel : class
	{
		private Expression<Func<TModel, TDto>> _toDtoMapper;
		private Func<TModel, TDto> _compiledToDtoMapper;

		private Expression<Func<TDto, TModel>> _toModelMapper;
		private Func<TDto, TModel> _compiledToModelMapper;

		public Repository(DbContext dbContext,
			Expression<Func<TModel, TDto>> toDtoMapper,
			Expression<Func<TDto, TModel>> fromDtoMapper)
			: base(dbContext)
		{
			if (toDtoMapper == null)
			{
				throw new Exception("DTO mapper cannot be null.");
			}

			if (fromDtoMapper == null)
			{
				throw new Exception("Model mapper cannot be null.");
			}

			_toDtoMapper = toDtoMapper;
			_toModelMapper = fromDtoMapper;

			_compiledToDtoMapper = _toDtoMapper.Compile();
			_compiledToModelMapper = _toModelMapper.Compile();
		}

		public TDto Add(TDto dto)
        {
            var addedModel = base.Add(_compiledToModelMapper.Invoke(dto));

			return _compiledToDtoMapper.Invoke(addedModel);
        }

        public Task<TDto> AddAsync(TDto dto)
        {
            return Task.Run(() => {
                return Add(dto);
            });
        }

        public IEnumerable<TDto> Add(IEnumerable<TDto> dtos)
        {
            var models = dtos.Select(_compiledToModelMapper);

			return base.Add(models).Select(_compiledToDtoMapper);
        }

        public IEnumerable<TDto> Find(Expression<Func<TModel, bool>> predicate)
        {
            return base.Find(predicate).Select(_toDtoMapper).ToList();
        }

        new public IEnumerable<TDto> Get()
        {
            return base.Get().Select(_toDtoMapper).ToList();
        }

        new public Task<IEnumerable<TDto>> GetAsync()
        {
            return Task.Run(() => {
                return Get();
            });
        }

        new public TDto Get(TKey id)
        {
            var result = base.Get(id);

			if (result == null)
			{
				return null;
			}

			return _compiledToDtoMapper.Invoke(result);
        }

        new public Task<TDto> GetAsync(TKey id)
        {
            return Task.Run(() =>
			{
				return Get(id);
			});
        }

        public TDto Update(TDto dto)
        {
            var model = base.Update(_compiledToModelMapper.Invoke(dto));

			return _compiledToDtoMapper.Invoke(model);
        }

        public Task<TDto> UpdateAsync(TDto dto)
        {
            return Task.Run(() => {
                return Update(dto);
            });
        }
	}
}

using Ets.Data.Repository.IRepository;

namespace Ets.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IEtsRepository<TEntity> Repository<TEntity>() where TEntity : class;
        bool Save();
    }
}

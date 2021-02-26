using COL.XMEN.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COL.XMEN.Infraestructure.CosmosDB
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> Where(string conditional);

        Task<TEntity> AddAsync(TEntity item);
    }
}

using SendGrid.Helpers.Mail;
using System.Data.Common;
using System.Linq.Expressions;

namespace Financial.Cat.Domain.Interfaces.Repositories.Abstract
{
	public interface IBaseRepository<TEntity>
		where TEntity : class
	{
		IQueryable<TEntity> Query();

		Task<bool> AnyAsync(
		Expression<Func<TEntity, bool>> predicate,
		CancellationToken cancellationToken);

		Task<TEntity?> GetOneAsync(
			Expression<Func<TEntity, bool>> predicate,
			CancellationToken cancellationToken,
			Func<IIncludable<TEntity>, IIncludable> includes = default, bool isIgnoreIncludes = false);

		Task<IList<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> predicate,
			CancellationToken cancellationToken,
			Func<IIncludable<TEntity>, IIncludable> includes = default, bool isIgnoreIncludes = false);

		Task<TEntity> AddAsync(TEntity model, CancellationToken cancellationToken);

		Task AddRangeAsync(IList<TEntity> models, CancellationToken cancellationToken);

		Task<TEntity> UpdateAsync(TEntity model, CancellationToken cancellationToken);

		Task<TEntity> UpdatePartiallyAsync(TEntity model, IEnumerable<string> updatedProperties, CancellationToken cancellationToken);

		Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

		Task<bool> DeleteRangeAsync(IEnumerable<TEntity> models, CancellationToken cancellationToken);

		Task ExecuteScalarsAsync(string commandText, Action<DbDataReader> action = null, IList<(string key, object value)> parameters = null);

		Task<IDictionary<Tkey, TValue>> ExecuteScalarsToDictionaryAsync<Tkey, TValue>
			(string commandText, Func<DbDataReader, (Tkey key, TValue value)> map, IList<(string key, object value)> parameters = null);

		Task<List<T>> ExecuteScalarsToListAsync<T>(string commandText, Func<DbDataReader, T> map, IList<(string key, object value)> parameters = null);
	}
}
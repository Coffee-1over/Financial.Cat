using AutoMapper;
using System.Linq.Expressions;

namespace Financial.Cat.Infrustructure.Extensions.DbExtenstions
{
	public static class QueryExtensions
	{
		/// <summary>
		/// Use filter predicate for condition
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="query">Initial query</param>
		/// <param name="condition">Condition filtering predicate</param>
		/// <param name="predicate">Filtering predicate</param>
		/// <returns>Query</returns>
		public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
			=> condition ? query.Where(predicate) : query;

		/// <summary>
		/// Turn back count of entities according to condition
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="query">Query</param>
		/// <param name="condition">use condition</param>
		/// <param name="count">Count</param>
		/// <returns>Query</returns>
		public static IQueryable<T> TakeIf<T>(this IQueryable<T> query, bool condition, int count)
			=> condition ? query.Take(count) : query;

		/// <summary>
		/// Select and Map.
		/// </summary>
		/// <typeparam name="TEntity">Entity Type</typeparam>
		/// <typeparam name="TModel">Model Type</typeparam>
		/// <param name="query">Initial query</param>
		/// <param name="mapper">Mapper</param>
		/// <returns>Model query</returns>
		public static IQueryable<TModel> SelectMap<TEntity, TModel>(
			this IQueryable<TEntity> query,
			IMapper mapper)
			where TEntity : class
			where TModel : class
			=> query.Select(x => mapper.Map<TModel>(x));

		/*/// <summary>
		/// Update fields in entity by patch model
		/// </summary>
		/// <param name="entity">Entity</param>
		/// <param name="patchModel">Patch model</param>
		/// <param name="exceptionFields">Exception fields</param>
		/// <typeparam name="TEntity">Type of entity</typeparam>
		/// <typeparam name="TPatchModel">Type of patch model</typeparam>
		public static void UpdateFields<TEntity, TPatchModel>(
			this TEntity entity, TPatchModel patchModel, IEnumerable<string> exceptionFields) 
			where TEntity : class
			where TPatchModel : class, IPatchModel
		{
			var fields = patchModel.GetType().GetProperties();
			foreach (var field in fields.Where(i => !exceptionFields.Contains(i.Name)))
			{
				var o = field.GetValue(patchModel);
				var p = entity.GetType().GetProperty(field.Name);
				if (o != null && p != null)
				{
					Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
					try
					{
						object safeValue = Convert.ChangeType(o, t);
						p.SetValue(entity, safeValue);
					}
					catch
					{
						p.SetValue(entity, o);
					}
				}
			}
		}*/
	}
}
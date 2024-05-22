using AutoMapper;
using Financial.Cat.Domain.Exceptions;
using Financial.Cat.Domain.Interfaces.Repositories.Abstract;
using Financial.Cat.Infrustructure.DB.Contexts;
using Financial.Cat.Infrustructure.Extensions;
using Financial.Cat.Infrustructure.Extensions.DbExtenstions;
using Financial.Cat.Infrustructure.Extensions.Includes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data.Common;
using System.Linq.Expressions;

namespace Financial.Cat.Infrustructure.DB.Repository.Abstract
{
	public class BaseRepository<TEntity> : IBaseRepository<TEntity>
		where TEntity : class
	{
		protected readonly ApplicationContext _context;

		/// <summary>
		/// Logger
		/// </summary>
		protected ILogger Logger { get; }

		/// <summary>
		/// Mapper
		/// </summary>
		protected IMapper Mapper { get; }

		/// <summary>
		/// Base repository constructor
		/// </summary>
		/// <param name="context">Fabric data context</param>
		/// <param name="logger">Logger</param>
		/// <param name="mapper">Mapper</param>
		public BaseRepository(ApplicationContext context, ILogger logger, IMapper mapper)
		{
			Mapper = mapper;
			_context = context;
			Logger = logger;
		}

		/// <summary>
		/// Work with context db (with save) and user with catch exception
		/// </summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="func">Action inside context</param>
		/// <param name="cancellationToken">Cancel token</param>
		/// <returns>Result</returns>
		protected Task<T> SaveChangesAndHandleExceptionAsync<T>(Func<ApplicationContext, CancellationToken, Task<Func<T>>> func, CancellationToken cancellationToken)
			=> HandleAsync(async (dbContext, cToken) =>
			{
				var afterSaveFunc = await func(dbContext, cToken);
				await dbContext.SaveChangesAsync(cToken);
				return afterSaveFunc();
			}, cancellationToken);


		/// <summary>
		/// Work with context db with catch exception
		/// </summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="func">Action inside context</param>
		/// <param name="cancellationToken">Cancel token</param>
		/// <returns>Result</returns>
		protected async Task<T> HandleAsync<T>(
			Func<ApplicationContext, CancellationToken, Task<T>> func,
			CancellationToken cancellationToken)
		{
			try
			{
				var result = await func(_context, cancellationToken);
				return result;
			}
			catch (DbUpdateException e)
			{
				Logger.LogError("DbUpdateException error: {Error}", e.Message);
				throw new BaseApplicationException(e.Message, e);
			}
			catch (Exception e)
			{
				Logger.LogError("Exception error: {Error}", e.Message);
				throw;
			}
		}

		public IQueryable<TEntity> Query() => _context.Set<TEntity>().AsNoTracking();

		public IQueryable<TEntity> Queryable() => _context.Set<TEntity>().AsNoTracking().AsQueryable();

		public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
			=> HandleAsync((dbContext, cToken) =>
				dbContext.Set<TEntity>().AsQueryable().AnyAsync(predicate, cToken), cancellationToken);

		public virtual Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken,
			Func<IIncludable<TEntity>, IIncludable> includes = default, bool isIgnoreIncludes = false)
			=> HandleAsync(async (dbContext, cToken) =>
			{
				var query = dbContext.Set<TEntity>().AsQueryable().AsNoTracking();

				query = query.IncludeMultiple(includes);

				if (predicate is not null)
					query = query.Where(predicate);

				var entity = await query.FirstOrDefaultAsync(cToken);

				if (entity is null)
				{
					return default;
				}

				return Mapper.Map<TEntity>(entity);
			}, cancellationToken);

		public virtual Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken, Func<IIncludable<TEntity>, IIncludable> includes = default, bool isIgnoreIncludes = false)
			=> HandleAsync(async (dbContext, cToken) =>
			{
				var query = dbContext.Set<TEntity>().AsQueryable().AsNoTracking();

				if (!isIgnoreIncludes)
					query = query.IncludeMultiple(includes);
				else
					query = query.IgnoreAutoIncludes();

				if (predicate is not null)
				{
					query = query.Where(predicate);
				}

				var entities = await query.ToListAsync(cToken);

				if (entities.Count == 0)
				{
					return Array.Empty<TEntity>();
				}

				return Mapper.Map<IList<TEntity>>(entities);
			}, cancellationToken);

		public virtual Task<TEntity> AddAsync(TEntity model, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<TEntity>(async (dbContext, cToken) =>
			{
				var entity = Mapper.Map<TEntity>(model);
				var result = await dbContext.Set<TEntity>().AddAsync(entity, cToken);
				return () =>
				{
					var resulTEntity = Mapper.Map<TEntity>(result.Entity);
					dbContext.DetachAll();
					return resulTEntity;
				};
			}, cancellationToken);

		public virtual Task AddRangeAsync(IList<TEntity> models, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<Task>(async (dbContext, cToken) =>
			{
				var entites = Mapper.Map<IEnumerable<TEntity>>(models);
				await dbContext.Set<TEntity>().AddRangeAsync(entites, cToken);
				return () =>
				{
					dbContext.DetachAll();
					return Task.CompletedTask;
				};
			}, cancellationToken);

		public virtual Task<TEntity> UpdateAsync(TEntity model, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<TEntity>(async (dbContext, cToken) =>
			{
				var entity = Mapper.Map<TEntity>(model);
				dbContext.Attach(entity);
				dbContext.Entry(entity).State = EntityState.Modified;
				var result = dbContext.Set<TEntity>().Update(entity);

				return () =>
				{
					var resulTEntity = Mapper.Map<TEntity>(result.Entity);
					dbContext.DetachAll();
					return resulTEntity;
				};
			}, cancellationToken);

		public virtual Task<TEntity> UpdatePartiallyAsync(TEntity model, IEnumerable<string> updatedProperties, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<TEntity>(async (dbContext, cToken) =>
			{
				var entity = Mapper.Map<TEntity>(model);
				dbContext.Attach(entity);
				var keyNames = dbContext.Model
					.FindEntityType(typeof(TEntity))
					.FindPrimaryKey().Properties
					.Select(x => x.Name);

				updatedProperties.ForEach(item =>
				{
					if (!keyNames.Contains(item))
					{
						dbContext.Entry(entity).Property(item).IsModified = true;
					}
				});

				return () => model;
			}, cancellationToken);

		public virtual Task<bool> DeleteAsync(TEntity model, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<bool>(async (dbContext, cToken) =>
			{
				var entity = Mapper.Map<TEntity>(model);
				dbContext.Set<TEntity>().Remove(entity);
				return () =>
				{
					dbContext.DetachAll();
					return true;
				};
			}, cancellationToken);

		public virtual Task<bool> DeleteRangeAsync(IEnumerable<TEntity> models, CancellationToken cancellationToken)
			=> SaveChangesAndHandleExceptionAsync<bool>(async (dbContext, cToken) =>
			{
				var entities = Mapper.Map<IEnumerable<TEntity>>(models);
				dbContext.Set<TEntity>().RemoveRange(entities);
				return () =>
				{
					dbContext.DetachAll();
					return true;
				};
			}, cancellationToken);

		public async Task<List<T>> ExecuteScalarsToListAsync<T>(string commandText, Func<DbDataReader, T> map, IList<(string key, object value)> parameters = null)
		{
			var result = new List<T>();

			await ExecuteScalarsAsync(commandText, (reader) => { result.Add(map(reader)); }, parameters);

			return result;
		}

		public async Task<IDictionary<Tkey, TValue>> ExecuteScalarsToDictionaryAsync<Tkey, TValue>
			(string commandText, Func<DbDataReader, (Tkey key, TValue value)> map, IList<(string key, object value)> parameters = null)
		{
			var dictionary = new Dictionary<Tkey, TValue>();

			await ExecuteScalarsAsync(commandText, (reader) =>
			{
				var mapedPair = map(reader);
				dictionary.TryAdd(mapedPair.key, mapedPair.value);
			},
			parameters);

			return dictionary;
		}

		public async Task ExecuteScalarsAsync(string commandText, Action<DbDataReader> action = null, IList<(string key, object value)> parameters = null)
		{
			using var command = _context.Database.GetDbConnection().CreateCommand();

			command.CommandText = commandText;

			if (parameters != null && parameters.Count > 0)
			{
				foreach (var parameter in parameters)
				{
					command.Parameters.AddRange(createParameters(command, parameter.key, parameter.value));
				}
			}

			await _context.Database.OpenConnectionAsync();

			using var reader = await command.ExecuteReaderAsync();

			while (await reader.ReadAsync())
			{
				action?.Invoke(reader);
			}
			await _context.Database.CloseConnectionAsync();
		}

		private DbParameter[] createParameters(DbCommand command, string parameterName, object value)
		{
			if (value is IEnumerable collection && !(value is string))
			{
				var parameterNames = new List<string>();
				var parameterList = new List<DbParameter>();
				var paramNbr = 1;
				// Create parameters for each item in the collection
				foreach (var item in collection)
				{
					var itemParameter = command.CreateParameter();
					itemParameter.ParameterName = string.Format("@{0}{1}", parameterName, paramNbr++);
					parameterNames.Add(itemParameter.ParameterName);
					itemParameter.Value = item;
					parameterList.Add(itemParameter);
				}
				command.CommandText = command.CommandText.Replace($"{{{parameterName}}}", string.Join(", ", parameterNames));
				// Return the list of parameters
				return parameterList.ToArray();
			}

			// Преобразовать DateTime.MinValue в null 
			if (value is DateTime)
			{
				var dt = (DateTime)value;
				if (dt == DateTime.MinValue)
				{
					value = DBNull.Value;
				}
			}
			else if (value == null)
			{
				value = DBNull.Value;
			}
			var parameter = command.CreateParameter();
			parameter.ParameterName = string.Format("@{0}", parameterName);
			parameter.Value = value;

			return new[] { parameter };
		}


	}
}
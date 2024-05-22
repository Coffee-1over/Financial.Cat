using AutoMapper;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.DB.Contexts;
using Financial.Cat.Infrustructure.DB.Repository.Abstract;
using Microsoft.Extensions.Logging;

namespace Financial.Cat.Infrastructure.DB.Repository
{
	public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
	{
		public CategoryRepository(ApplicationContext context, ILogger<CategoryRepository> logger, IMapper mapper)
		: base(context, logger, mapper)
		{ }

		public async Task<List<CategoryEntity>> GetCategoryTreeAsync()
		{
			var query = @"
            WITH CategoryCTE AS
            (
                SELECT * FROM Categories
                WHERE ParentCategoryId IS NULL
                UNION ALL
                SELECT c.* FROM Categories c
                INNER JOIN CategoryCTE cte ON c.ParentCategoryId = cte.Id
            )
            SELECT * FROM CategoryCTE
        ";

			var categories = await ExecuteScalarsToListAsync<CategoryEntity>(
				query,
				reader => new CategoryEntity
				{
					Id = reader.GetInt64(reader.GetOrdinal("Id")),
					Name = reader.GetString(reader.GetOrdinal("Name")),
					ParentCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentCategoryId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("ParentCategoryId")),
					Created = reader.GetDateTime(reader.GetOrdinal("Created")),
					Updated = reader.GetDateTime(reader.GetOrdinal("Updated"))
				}
			);

			return BuildCategoryTree(categories);
		}

		private List<CategoryEntity> BuildCategoryTree(List<CategoryEntity> categories)
		{
			var categoryLookup = categories.ToLookup(c => c.ParentCategoryId);

			List<CategoryEntity> BuildTree(long? parentId)
			{
				return categoryLookup[parentId].Select(category => new CategoryEntity
				{
					Id = category.Id,
					Name = category.Name,
					ParentCategoryId = category.ParentCategoryId,
					Created = category.Created,
					Updated = category.Updated,
					ChildCategories = BuildTree(category.Id)
				}).ToList();
			}

			return BuildTree(null);
		}
	}
}

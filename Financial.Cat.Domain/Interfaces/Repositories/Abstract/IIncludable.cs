namespace Financial.Cat.Domain.Interfaces.Repositories.Abstract
{
	/// <summary>
	/// Table interface to be included
	/// </summary>
	public interface IIncludable
	{
	}

	public interface IIncludable<out TEntity> : IIncludable
	{
	}

	public interface IIncludable<out TEntity, out TProperty> : IIncludable<TEntity>
	{
	}
}
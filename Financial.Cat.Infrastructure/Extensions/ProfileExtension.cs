using AutoMapper;

namespace Financial.Cat.Infrustructure.Extensions
{
	/// <summary>
	/// Extension class for Automapper profiles
	/// </summary>
	public static class ProfileExtension
	{
		/// <summary>
		/// Ignore property
		/// </summary>
		/// <param name="source">Mapping expression</param>
		/// <param name="ignores">Ignore fields</param>
		/// <typeparam name="TSource">Source</typeparam>
		/// <typeparam name="TDestination">Destination</typeparam>
		/// <returns>Mapping expression</returns>
		public static IMappingExpression<TSource, TDestination> ForAllMembersIgnore<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> source, IEnumerable<string> ignores)
		{
			ignores.ForEach(ignore =>
			{
				source = source.ForMember(ignore, opt =>
					opt.Ignore());
			});

			return source;
		}

		/// <summary>
		/// Ignore all property in mapping except in list
		/// </summary>
		/// <param name="source">Mapping expression</param>
		/// <param name="notIgnores">Except list</param>
		/// <typeparam name="TSource">Source: from where start</typeparam>
		/// <typeparam name="TDestination">Destination: to where finish</typeparam>
		/// <returns>Mapping expression</returns>
		public static IMappingExpression<TSource, TDestination> ForMembersIgnoreExcept<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> source, IEnumerable<string> notIgnores)
		{
			var props = typeof(TDestination).GetProperties();

			props.ForEach(prop =>
			{
				if (notIgnores.Contains(prop.Name)) return;

				source = source.ForMember(prop.Name, opt =>
					opt.Ignore());
			});

			return source;
		}

		/// <summary>
		/// Ignore constructor params
		/// </summary>
		/// <param name="source">Mapping expression</param>
		/// <param name="ignores">Ignore constructor params</param>
		/// <typeparam name="TSource">Source: from where start</typeparam>
		/// <typeparam name="TDestination">Destination: to where finish</typeparam>
		/// <returns>Mapping expression</returns>
		public static IMappingExpression<TSource, TDestination> ForAllCtorParamsIgnore<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> source, IEnumerable<string> ignores)
		{
			var props = typeof(TDestination).GetProperties();

			ignores.ForEach(ignore =>
			{
				var prop = props.FirstOrDefault(propInner => propInner.Name.SequenceEqual(ignore));

				if (prop == null) return;

				var f = char.ToLower(ignore[0]).ToString();
				var res = string.Concat(f, ignore[1..^0]);

				source = source.ForCtorParam(res, opt =>
					opt.MapFrom(src => prop.PropertyType.GetDefault()));
			});

			return source;
		}

		/// <summary>
		/// Ignore all properties and params
		/// </summary>
		/// <param name="source">Mapping expression</param>
		/// <param name="ignores">list of ignore properties and params</param>
		/// <typeparam name="TSource">Source: from where start</typeparam>
		/// <typeparam name="TDestination">Destination: to where finish</typeparam>
		/// <returns>Mapping expression</returns>
		public static IMappingExpression<TSource, TDestination> ForAllIgnore<TSource, TDestination>(
			this IMappingExpression<TSource, TDestination> source, IEnumerable<string> ignores)
		{
			source = source.ForAllMembersIgnore(ignores);
			return source.ForAllCtorParamsIgnore(ignores);
		}
	}
}
using Financial.Cat.Infrustructure.Extensions.DbExtenstions;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Financial.Cat.Infrustructure.Extensions
{
	public static class FluentValidationExtensions
	{
		/// <summary>
		/// Check int: value not null and greater than 0
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, int> NotNullAndGreaterThanZero<T>(this IRuleBuilder<T, int> ruleBuilder)
		{
			return ruleBuilder
				.NotNull()
				.GreaterThan(0);
		}

		/// <summary>
		/// Check long: value not null and greater than 0
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, long> NotNullAndGreaterThanZero<T>(this IRuleBuilder<T, long> ruleBuilder)
		{
			return ruleBuilder
				.NotNull()
				.GreaterThan(0);
		}

		/// <summary>
		/// Check float: value not null and greater than 0
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, float> NotNullAndGreaterThanZero<T>(this IRuleBuilder<T, float> ruleBuilder)
		{
			return ruleBuilder
				.NotNull()
				.GreaterThan(0);
		}

		/// <summary>
		/// Check that number is correct for number with code
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			return ruleBuilder
				.Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
		}

		/// <summary>
		/// Check enum: value is not null and contains into enum
		/// </summary>
		/// <typeparam name="TProperty">Property of validity object</typeparam>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, TProperty> NotNullAndIsInEnum<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
		{
			return ruleBuilder
				.NotNull()
				.IsInEnum();
		}

		/// <summary>
		/// Check string: string is not empty and not above max length
		/// </summary>
		/// <param name="maxLength">Max string length</param>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, string> NotExceedMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
		{
			return ruleBuilder
				.MaximumLength(maxLength);
		}

		/// <summary>
		/// Check date: value should be in past
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, DateTime> PastDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
		{
			return ruleBuilder
				.Must(date => date < DateTime.Now)
				.WithMessage("The date is not in past");
		}

		/// <summary>
		/// Check nullable date: value should be in past
		/// </summary>
		/// <param name="ruleBuilder">Rule builder</param>
		/// <typeparam name="T">Validated type</typeparam>
		/// <returns>Rule builder</returns>
		public static IRuleBuilderOptions<T, DateTime?> PastDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
		{
			return ruleBuilder
				.Must(date => date < DateTime.Now)
				.WithMessage("The date is not in past");
		}

		/// <summary>
		/// Return max. size of field.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property"></param>
		/// <returns></returns>
		private static int? GetEntityMaxLengthProperty<T>(this Expression<Func<T, object>> property)
			=> property.GetPropertyInfo()?.GetAttributeValue<MaxLengthAttribute, int>(a => a.Length);

		/// <summary>
		/// Validate max size of field by entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="ruleBuilder"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, string?> MaxLengthFromEntity<T, TEntity>(
			this IRuleBuilder<T, string?> ruleBuilder,
			Expression<Func<TEntity, object>> property)
		{
			var maxLength = property.GetEntityMaxLengthProperty();
			if (maxLength.HasValue)
				return ruleBuilder.MaximumLength(maxLength.Value);
			return ruleBuilder.Must(s => true);
		}
	}
}
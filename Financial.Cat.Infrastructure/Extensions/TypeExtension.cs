namespace Financial.Cat.Infrustructure.Extensions
{
	/// <summary>
	/// Extension class for types
	/// </summary>
	public static class TypeExtension
	{
		/// <summary>
		/// Get default value of type
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns>Default value type</returns>
		public static object? GetDefault(this Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}

			if (type == typeof(string))
			{
				return string.Empty;
			}

			return null;
		}

		/// <summary>
		/// Get type string representation
		/// </summary>
		/// <param name="objType">Type</param>
		/// <param name="obj">Object</param>
		/// <returns>String representation</returns>
		public static string ToStringRepresentation(this Type objType, object obj) =>
			string.Join(
				", ",
				objType.GetProperties().Select(p => $"{p.Name}: {p.GetValue(obj)}"));
	}
}
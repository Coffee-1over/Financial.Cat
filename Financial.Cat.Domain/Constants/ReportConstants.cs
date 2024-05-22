namespace Financial.Cat.Domain.Constants
{
	public static class ReportConstants
	{
		public static string FileName => "Звіт про витрати";
		public static IList<string> ColumnsNames => new List<string>
		{
			"Назва товару",
			"Назва категорії",
			"Час покупки",
			"Ціна",
			"Кількість",
			"Загалом"
		};
	}
}

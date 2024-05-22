namespace Financial.Cat.Domain.Models.Business.Reports
{
	public class FileModel
	{
		public string Name { get; set; }
		public MemoryStream Content { get; set; }
		public string ContentType { get; set; }
	}
}

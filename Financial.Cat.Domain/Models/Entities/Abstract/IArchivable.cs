namespace Financial.Cat.Domain.Models.Entities.Abstract
{
    public interface IArchivable
    {
        bool IsActive { get; set; }
    }
}
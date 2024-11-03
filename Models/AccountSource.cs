namespace B1Task2.Models;

public partial class AccountSource
{
    public int Id { get; set; }

    public string SourceType { get; set; } = null!;

    public DateTime? UploadDate { get; set; }

    public virtual ICollection<AccountClass> AccountClasses { get; set; } = new List<AccountClass>();
}

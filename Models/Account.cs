namespace B1Task2.Models;

public partial class Account
{
    public int Id { get; set; }

    public int AccountCode { get; set; }

    public int ClassId { get; set; }

    public virtual AccountClass Class { get; set; } = null!;

    public virtual ICollection<Element> Elements { get; set; } = new List<Element>();

}

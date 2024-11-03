namespace B1Task2.Models;

public partial class ElementsType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Element> Elements { get; set; } = new List<Element>();
}

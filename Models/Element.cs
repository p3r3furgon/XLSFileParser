using System.Text.Json.Serialization;

namespace B1Task2.Models;

public partial class Element
{
    public int Id { get; set; }

    public int Accountid { get; set; }

    public int Elementtypeid { get; set; }

    public decimal Value { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;

    public virtual ElementsType ElementType { get; set; } = null!;
}

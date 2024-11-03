using System.Text.Json.Serialization;

namespace B1Task2.Models;

public partial class AccountClass
{
    public int Id { get; set; }

    public int ClassCode { get; set; }

    public string ClassName { get; set; } = null!;

    public int SourceId { get; set; }

    public virtual AccountSource Source { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

}

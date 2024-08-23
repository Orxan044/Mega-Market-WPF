using Mega_Market_Data.Models.Abstract;
using Mega_Market_Data.Models.Enum;

namespace Mega_Market_Data.Models.Concretes;

public class Message : BaseEntity
{
    public MessageType? Type { get; set; }
    public string? Messages { get; set; }
    public DateTime? SendTime { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }

    public override string ToString() => $"{Type}";

}

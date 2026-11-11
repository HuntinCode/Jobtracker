using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTracking.Data;

public class Contact {

    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }

    public required string UserId { get; set; }
    public required ApplicationUser User { get; set; }

    [StringLength(254)]
    [EmailAddress]
    public string? Email { get; set; }

    public int? CompanyId { get; set; }
    public Company? Company { get; set;}

    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.UtcNow;

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTracking.Data;

public enum JobStatus {
    Applied,
    Screening,
    Interviewing,
    Offered,
    Rejected
}


public class JobApplication {
    public int Id { get; set;}
    public required string UserId { get; set;}
    public required ApplicationUser User { get; set; }
    public string Position { get; set; } = string.Empty;
    
    public int YearlySalary { get; set; }

    public JobStatus Status { get; set; }

    public required DateOnly DateApplied { get; set; }
    public int CompanyId { get; set; }
    public required Company Company { get; set; }

    [Url]
    [StringLength(2048)]
    public string? JobPostLink { get; set;}

    [StringLength(500)]
    public string? Notes { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset LastUpdate {get; set;} = DateTimeOffset.UtcNow;

}
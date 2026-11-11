using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JobTracking.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<JobApplication> JobsApplied { get; set; } = new List<JobApplication>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

}


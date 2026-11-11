
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Data;

[Index(nameof(Name), IsUnique = true)]
public class Company {
    
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Contact> Contacts = new List<Contact>();
    public ICollection<JobApplication> JobsApplied = new List<JobApplication>();

}
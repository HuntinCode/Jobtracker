

using System.Security.Claims;
using JobTracking.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Services;

public class JobService  {
    private readonly IDbContextFactory<ApplicationDbContext> _dbfactory;
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ICompanyService _companyService;

    public JobService(IDbContextFactory<ApplicationDbContext> db, AuthenticationStateProvider authProvider, ICompanyService companyService) {
        _dbfactory = db;
        _authProvider = authProvider;
        _companyService = companyService;
    }

    public async Task<List<JobApplication>> GetMyJobApplications() {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var userClaims = authState.User;

        if (userClaims.Identity?.IsAuthenticated != true) return [];
        
        var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId)) return [];

        await using var context = await _dbfactory.CreateDbContextAsync();

        return await context.JobApplications
            .AsNoTracking()
            .Include(j => j.Company)
            .Where(j => j.UserId == userId)
            .ToListAsync();
        
    }

    public async Task<Dictionary<string,int>> GetJobApplicationStatusCountsAsync() {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var userClaims = authState.User;

        if (userClaims.Identity?.IsAuthenticated != true) return new Dictionary<string, int>();

        var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return new Dictionary<string, int>();

        await using var context = await _dbfactory.CreateDbContextAsync();

        return await context.JobApplications
            .Where(j => j.UserId == userId)
            .GroupBy(j => j.Status)
            .Select(group => new {
                Status = group.Key.ToString(),
                Count = group.Count()
            })
            .ToDictionaryAsync(
                result => result.Status,
                result => result.Count
            );

    }

    public async Task<JobApplication?> GetJobAppById(int id) {
        if (id <= 0) return null;

        await using var context = await _dbfactory.CreateDbContextAsync();

        return await context.JobApplications
            .FindAsync(id);
    }

    public async Task<JobApplication> CreateAsync(JobApplication newJobApp) {
        await using var context = await _dbfactory.CreateDbContextAsync();

        if (newJobApp.Company == null || string.IsNullOrWhiteSpace(newJobApp.Company.Name)) 
            throw new ArgumentException("A valid company and name is required");

        var targetCompany = newJobApp.Company.Name.ToLower();

        var existingCompany = await context.Companies
            .FirstOrDefaultAsync(c => c.Name == targetCompany);

        if (existingCompany != null) {
            newJobApp.CompanyId = existingCompany.Id;
             newJobApp.Company = null!;
        } else 
            newJobApp.Company.Name = targetCompany;
        
        context.JobApplications.Add(newJobApp);
        await context.SaveChangesAsync();
    
        return newJobApp;
    }

    public async Task DeleteAsync(JobApplication jobApplication) {
        await using var context = await _dbfactory.CreateDbContextAsync();

        context.JobApplications.Remove(jobApplication);
        await context.SaveChangesAsync();
    }

    public async Task<JobApplication> UpdateJobApplication(JobApplication updatedJobApp) {
    await using var context = await _dbfactory.CreateDbContextAsync();

        var oldJob = await context.JobApplications.FindAsync(updatedJobApp.Id);
        
        if (oldJob == null) 
            return await CreateAsync(updatedJobApp);

        oldJob.Status = updatedJobApp.Status;
        oldJob.YearlySalary = updatedJobApp.YearlySalary;
        oldJob.Notes = updatedJobApp.Notes;
        oldJob.Position = updatedJobApp.Position;
        oldJob.JobPostLink = updatedJobApp.JobPostLink;
        oldJob.LastUpdate = updatedJobApp.LastUpdate;

        if (updatedJobApp.Company != null) {
            var companyName = updatedJobApp.Company.Name.Trim();

            var existingCompany = await context.Companies.FirstOrDefaultAsync(
                c => c.Name == companyName
            );
            
            if (existingCompany != null) {
                oldJob.Company = existingCompany;

            } else {

                var newCompany = new Company {Name = companyName};
                context.Companies.Add(newCompany);
                oldJob.Company = newCompany;
            }
        }

        
        
        await context.SaveChangesAsync();
        return oldJob;
    }


}
using JobTracking.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Services;

public class CompanyService : ICompanyService {
    
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public CompanyService(IDbContextFactory<ApplicationDbContext> dbFactory) {
        _dbFactory = dbFactory;
    }
    
    public async Task<List<Company>> GetCompaniesAsync() {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.Companies.ToListAsync();
    }

    public async Task<List<Company>> GetCompanyContactsAsync() {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.Companies
            .Include(co => co.Contacts)
            .ToListAsync();
        
    }

    public async Task<List<Company>> GetCompanyApplications() {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.Companies
            .Include(co => co.JobsApplied)
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(int id) {
        await using var context = await _dbFactory.CreateDbContextAsync();

        return await context.Companies.FindAsync(id);
    }

    public async Task<Company?> GetCompanyByNameAsync(string companyName) {
        await using var context = await _dbFactory.CreateDbContextAsync();
        
        companyName = companyName.Trim().ToLower();
        
        return await context.Companies
            .FirstOrDefaultAsync(c => c.Name == companyName);
    }

    public async Task<Company> CreateAsync(Company newCompany) {
        newCompany.Name = newCompany.Name.Trim().ToLower();

        if (await CompanyExists(newCompany.Name))
            throw new InvalidOperationException("Cannot create new company because it already exists.");

        await using var context = await _dbFactory.CreateDbContextAsync();
        
        context.Companies.Add(newCompany);
        await context.SaveChangesAsync();
        return newCompany;
    }

    public async Task<bool> DeleteAsync(int id) {
        await using var context = await _dbFactory.CreateDbContextAsync();
        Company? company = await context.Companies.FindAsync(id);

        if (company == null) return false;

        context.Companies.Remove(company);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateNameAsync(Company updatedCompany) {
        await using var context = await _dbFactory.CreateDbContextAsync();
        Company? existingCompany = await context.Companies.FindAsync(updatedCompany.Id);

        if (existingCompany == null) return false;

        existingCompany.Name = updatedCompany.Name;

        await context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> CompanyExists(string name) {
        await using var context = await _dbFactory.CreateDbContextAsync();
        name = name.Trim().ToLower();
    
        return await context.Companies
            .AnyAsync(company => company.Name == name);
    }

}
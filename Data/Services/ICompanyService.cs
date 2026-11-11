
namespace JobTracking.Services;

using JobTracking.Data;

public interface ICompanyService {
    
    public Task<List<Company>> GetCompaniesAsync();
    public Task<List<Company>> GetCompanyContactsAsync();
    public Task<List<Company>> GetCompanyApplications();
    public Task<Company?> GetCompanyByIdAsync(int id);
    public Task<Company?> GetCompanyByNameAsync(string companyName);
    public Task<Company> CreateAsync(Company newCompany);
    public Task<bool> DeleteAsync(int id);
    public Task<bool> UpdateNameAsync(Company updatedCompany);
    public Task<bool> CompanyExists(string name);
        
    

}
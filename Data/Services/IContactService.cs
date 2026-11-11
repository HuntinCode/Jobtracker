using JobTracking.Data;

namespace JobTracking.Services;

public interface IContactService {
    
    public Task<List<Contact>> GetContactsAsync();
    public Task<Contact?> GetContactAsync(int id);
    public Task<List<Contact>> GetContactsCompany();
    public Task<Contact> CreateAsync(Contact contact);
    public Task<bool> UpdateAsync(Contact updatedContact);
    public Task<bool> DeleteAsync(int id);



}
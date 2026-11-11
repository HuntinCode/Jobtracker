
using JobTracking.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Services;

public class ContactService : IContactService {
    private readonly ApplicationDbContext _db;

    public ContactService(ApplicationDbContext db) {
        _db = db;
    }

    public async Task<List<Contact>> GetContactsAsync() {
        return await _db.Contacts.ToListAsync();
    }

    public async Task<Contact?> GetContactAsync(int id) {
        return await _db.Contacts.FindAsync(id);
    }

    public async Task<List<Contact>> GetContactsCompany() {
        return await _db.Contacts
            .Include(con => con.Company)
            .ToListAsync();
    }

    public async Task<Contact> CreateAsync(Contact newContact) {
        _db.Contacts.Add(newContact);
       await _db.SaveChangesAsync();

       return newContact;
    }

    public async Task<bool> UpdateAsync(Contact updatedContact) {
        Contact? currentContact = await _db.Contacts.FindAsync(updatedContact.Id);

        if (currentContact == null) return false;

        currentContact.FirstName = updatedContact.FirstName;
        currentContact.LastName = updatedContact.LastName;
        currentContact.Email = updatedContact.Email;
        currentContact.CompanyId = updatedContact.CompanyId;
        currentContact.Company = updatedContact.Company;
        currentContact.LastUpdate = DateTimeOffset.UtcNow;
            
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id) {
        Contact? contact = await _db.Contacts.FindAsync(id);

        if (contact == null) return false;

        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
        return true;
    }


}
using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // equal to [Route("api/contacts")]
public class ContactsController : Controller
{
    private readonly ContactsAPIDbContext dbContext;

    public ContactsController(ContactsAPIDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetContactsAsync()
    {
        List<Contact> contacts = await dbContext.Contacts.ToListAsync().ConfigureAwait(false);

        return Ok(contacts);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetContactAsync([FromRoute] Guid id)
    {
        Contact contact = await dbContext.Contacts.FindAsync(id);

        if (contact is null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> AddContactAsync(AddContactRequest model)
    {
        Contact contact = new Contact()
        {
            Id = Guid.NewGuid(),
            Address = model.Address,
            Email = model.Email,
            FullName = model.FullName,
            Phone = model.Phone
        };

        await dbContext.Contacts.AddAsync(contact).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return Ok(contact);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateContactAsync([FromRoute] Guid id, UpdateContactRequest model)
    {
        Contact contact = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);

        if (contact is null)
            return NotFound();

        contact.FullName = model.FullName;
        contact.Phone = model.Phone;
        contact.Email = model.Email;
        contact.Address = model.Address;

        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return Ok(contact);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteContactAsync([FromRoute] Guid id)
    {
        Contact contact = await dbContext.Contacts.FindAsync(id).ConfigureAwait(false);

        if (contact is null)
            return NotFound();

        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return Ok(contact);
    }
}

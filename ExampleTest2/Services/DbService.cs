using ExampleTest2.Data;
using ExampleTest2.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CharacterExists(int characterId)
    {
        return await _context.Characters
            .Where(c => c.Id == characterId)
            .AnyAsync();
    }

    public async Task<Character> GetCharacterData(int characterId)
    {
        return await _context.Characters
            .Include(c => c.Backpacks)
            .ThenInclude(b => b.Item)
            .Include(c => c.CharacterTiles)
            .ThenInclude(ct => ct.Title)
            .Where(c => c.Id == characterId)
            .FirstAsync();
    }

    public async Task<bool> ItemsExist(List<int> itemIds)
    {
        var uniqueIds = itemIds.Distinct().ToHashSet();
        return await _context.Items.CountAsync(i => uniqueIds.Contains(i.Id)) == uniqueIds.Count;
    }

    public async Task<List<Item>> GetItems(List<int> itemIds)
    {
        return await _context.Items.Where(e => itemIds.Contains(e.Id)).ToListAsync();
    }

    public async Task AddBackpackItems(List<Backpack> items)
    {
        await _context.Backpacks.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }

    // public async Task<bool> PatientExists(int patientId)
    // {
    //     return await _context.Patients
    //         .Where(e => e.IdPatient == patientId)
    //         .AnyAsync();
    // }
}
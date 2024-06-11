using System.Transactions;
using ExampleTest2.Models;
using ExampleTest2.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleTest2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly IDbService _dbService;

    public CharacterController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    [Route("{idCharacter:int}/")]
    public async Task<IActionResult> GetCharacter(int idCharacter)
    {
        if (!await _dbService.CharacterExists(idCharacter))
            return NotFound("Character doesn't exist");
        Character character = await _dbService.GetCharacterData(idCharacter);
        // Musimy zwrócić obiekt w innym "kształcie" zgodnie z zadaniem.
        // Można też użyć DTO, ja wybrałem obiekty anonimowe.
        return Ok(new
        {
            firstName = character.FirstName,
            lastName = character.LastName,
            currentWeight = character.CurrentWeight,
            maxWeight = character.MaxWeight,
            backpackItems =
                character.Backpacks.Select(b =>
                    new
                    {
                        itemName = b.Item.Name,
                        itemWeight = b.Item.Weight,
                        amount = b.Amount
                    }).ToList(),
            titles = character.CharacterTiles.Select(t =>
                new
                {
                    title = t.Title.Name,
                    aquiredAt = t.AcquiredAt
                })
        });
    }

    [HttpPost]
    [Route("{idCharacter:int}/backpacks")]
    public async Task<IActionResult> AddBackpackItem(List<int> itemIds, int idCharacter)
    {
        var itemCounts = itemIds
            .GroupBy(s => s)
            .Select(s => new
            {
                Id = s.Key,
                Count = s.Count()
            })
            .ToDictionary(g => g.Id, g => g.Count);
        if (!await _dbService.CharacterExists(idCharacter))
            return NotFound("Character doesn't exist");
        Character character = await _dbService.GetCharacterData(idCharacter);
        if (!await _dbService.ItemsExist(itemIds))
            return BadRequest("One or more items from the list don't exist");
        List<Item> items = await _dbService.GetItems(itemCounts.Select(e => e.Key).ToList());
        List<Backpack> newItems = new List<Backpack>();
        int weightsSum = 0;
        foreach (var item in items)
        {
            var amount = itemCounts[item.Id];
            weightsSum += item.Weight * amount;
            newItems.Add(new Backpack()
            {
                CharacterId = idCharacter,
                ItemId = item.Id,
                Amount = amount
            });
        }
        if (weightsSum > (character.MaxWeight - character.CurrentWeight))
            return BadRequest("Character cannot carry all those items");
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            // Zgodnie z odpowiedzią prowadzącego: Nie musimy sprawdzać czy przedmioty już istnieją i nie musimy
            // aktualizować liczności istniejących, zakładamy, że postać nie ma dodawanych (typów) przedmiotów.
            await _dbService.AddBackpackItems(newItems);
            scope.Complete();
        }

        return Ok(newItems.Select(e => new
        {
            amount = e.Amount,
            itemId = e.ItemId,
            characterId = e.CharacterId
        }));
    }
}
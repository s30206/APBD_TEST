using Microsoft.IdentityModel.Tokens;
using TavernSystem.Models.DTO;
using TaverSystem.Repositories;

namespace TavernSystem.Application;

public class TavernService : ITavernService
{
    private readonly ITavernRepository _tavernRepository;

    public TavernService(ITavernRepository tavernRepository)
    {
        _tavernRepository = tavernRepository;
    }

    public async Task<List<object>> GetAllTravelersAsync()
    {
        var result = await _tavernRepository.GetAllAdventurersAsync();
        return result;
    }

    public async Task<object> GetTravellerByIdAsync(int id)
    {
        if (id < 1)
            throw new Exception("Id must be higher than 0");

        var result = await _tavernRepository.GetAdventurerByIdAsync(id);
        return result;
    }

    public async Task<bool> AddTravelerAsync(AdventurerDTO request)
    {
        var result = await _tavernRepository.AddAdventurerAsync(request);
        return result;
    }
}
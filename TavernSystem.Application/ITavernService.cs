using TavernSystem.Models.DTO;

namespace TavernSystem.Application;

public interface ITavernService
{
    public Task<List<object>> GetAllTravelersAsync();
    public Task<object> GetTravellerByIdAsync(int id);
    public Task<bool> AddTravelerAsync(AdventurerDTO request);
}
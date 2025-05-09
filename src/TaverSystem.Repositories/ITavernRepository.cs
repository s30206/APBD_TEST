using TavernSystem.Models.DTO;

namespace TaverSystem.Repositories;

public interface ITavernRepository
{
    public Task<List<object>> GetAllAdventurersAsync();
    public Task<object> GetAdventurerByIdAsync(int id);
    public Task<bool> AddAdventurerAsync(AdventurerDTO request);
}
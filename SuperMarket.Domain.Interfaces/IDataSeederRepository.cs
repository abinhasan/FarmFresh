using SuperMarket.Domain.DTO;

namespace SuperMarket.Domain.Interfaces
{
    public interface IDataSeederRepository
    {
        Task SeedData();
    }
}

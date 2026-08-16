using ProductOrderApi.Data.Repositories;

namespace ProductOrderApi.Data
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        Task<int> SaveChangesAsync();
    }
}

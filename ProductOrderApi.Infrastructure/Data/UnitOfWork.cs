using ProductOrderApi.Data;
using ProductOrderApi.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderContext _context;
    private IProductRepository? _products;
    private IOrderRepository? _orders;

    public UnitOfWork(OrderContext context)
    {
        _context = context;
    }

    public IProductRepository Products => _products ??= new ProductRepository(_context);

    public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
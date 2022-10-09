using SignalRApp.API.Models;

namespace SignalRApp.API.Hubs
{
    public interface IProductHub
    {
        Task ReceiveProduct(Product p);
    }
}

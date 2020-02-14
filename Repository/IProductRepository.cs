using ProductsAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Repository
{
    public interface IProductRepository
    {
        void DeleteProduct(int productId);
        Task<Dictionary<string, object>> GetAsyncProductByID(int productId);
        //Task<Dictionary<string, object>> GetInventoryForProduct(int productId);
        Task<Product> GetAsyncProductInternalByID(int productId);
        //Task<List<Product>> GetAsyncProducts(IEnumerable<int> ids);
        //Task<List<Product>> GetAsyncProductsItemsForPage(int pageSize, int pageIndex);
        Task<long> GetAsyncProductCount();
        Dictionary<string, object> GetProductByID(int productId);
        //IEnumerable<Product> GetProducts();
        Task<List<Dictionary<string, object>>> GetProducts();
        void InsertProduct(Product product);
        void Save();
        void UpdateProduct(Product product);
        Task<List<Dictionary<string, object>>> GetSubcategorys();

        // Task SaveChangesAsync();
    }
}
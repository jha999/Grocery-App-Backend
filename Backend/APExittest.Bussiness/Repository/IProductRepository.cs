using APExittest.Bussiness.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APExittest.Bussiness.Repository
{
    public interface IProductRepository
    {
        Task<int> AddProduct(ProductModel _event);
        Task<ProductModel> DeleteProduct(ProductModel product);
        Task<ProductModel> EditProduct(ProductModel _product);
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
    }
}
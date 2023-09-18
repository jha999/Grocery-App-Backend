using APExittest.Bussiness.Data;
using APExittest.Bussiness.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APExittest.Bussiness.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<int> AddProduct(ProductModel _event)
        {
            await _context.products.AddAsync(_event);
            await _context.SaveChangesAsync();
            return _event.Id;
        }

        public async Task<ProductModel> EditProduct(ProductModel _product)
        {
            var dbproduct = await _context.products.FindAsync(_product.Id);

            dbproduct.Product_Name = _product.Product_Name;
            dbproduct.Product_Description = _product.Product_Description;
            dbproduct.Product_Category = _product.Product_Category;
            dbproduct.Available_Quantity = _product.Available_Quantity;
            dbproduct.Available_Discount = _product.Available_Discount;
            dbproduct.Image = _product.Image;
            dbproduct.Available_Price = _product.Available_Price;
            dbproduct.Specification = _product.Specification;

            await _context.SaveChangesAsync();
            return dbproduct;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var dbproduct = await _context.products.FindAsync(id);
            return dbproduct;
        }


        public async Task<ProductModel> DeleteProduct(ProductModel product)
        {
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _context.products.ToListAsync();
        }

    }
}

using APExittest.Bussiness.Data;
using APExittest.Bussiness.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APExittest.Bussiness.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<int> AddCartOrder(Ordertable _event)
        {
            var newOrder = new Ordertable()
            {
                ProductId = _event.ProductId,
                UserEmail = _event.UserEmail,
                OrderQuantity = _event.OrderQuantity,
                isOrderConfirmed = _event.isOrderConfirmed
            };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return newOrder.Id;
        }

        public async Task<Ordertable> EditCart(Ordertable _product)
        {
            var dborder = await _context.Orders.FindAsync(_product.Id);

            dborder.isOrderConfirmed = _product.isOrderConfirmed;
            await _context.SaveChangesAsync();
            return dborder;
        }

        public async Task<List<Ordertable>> GetAllCartProducts()
        {
            return await _context.Orders.ToListAsync();
        }

    }
}

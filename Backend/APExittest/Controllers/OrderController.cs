using APExittest.Bussiness.Model;
using APExittest.Bussiness.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace APExittest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("addcart")]

        public async Task<IActionResult> AddProduct(Ordertable product)
        {
            if (product != null)
            {
                var id = await _orderRepository.AddCartOrder(product);
                if (id < 0)
                {
                    return BadRequest();
                }
                return Ok(product);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("editcart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<Ordertable> UpdateCart(Ordertable product)
        {
            
            var answer = await _orderRepository.EditCart(product);
            if (answer.Id > 0)
            {
                return product;
            }
            return product;
        }


        [HttpGet]
        [Route("getcartorder")]
        public async Task<IActionResult> GetAllCartProducts()
        {
            var products = await _orderRepository.GetAllCartProducts();
            return Ok(products);
        }
    }
}

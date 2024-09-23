using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cart_Api.Model;
using System.Net.NetworkInformation;
using System.Linq;

namespace Cart_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private static List<CartItem> items = new List<CartItem>()
        {
            new CartItem(){id=1, product="Chips", price=3.50, quantity= 100},
            new CartItem(){id=2, product="Soda", price=1.25, quantity=200},
            new CartItem(){id=3, product="Candy", price=0.99, quantity=150},
            new CartItem(){id=4, product="Bread", price=2.50, quantity=50},
            new CartItem(){id=5, product="Milk", price=3.00, quantity=30},
            new CartItem(){id=6, product="Eggs", price=2.75, quantity=60},
            new CartItem(){id=7, product="Cheese", price=4.00, quantity=40},
            new CartItem(){id=8, product="Butter", price=3.25, quantity=25},
            new CartItem(){id=9, product="Juice", price=2.00, quantity=80},
            new CartItem(){id=10, product="Cookies", price=3.75, quantity=90}
        };
        private static int nextId = 11;


        //      curl -X 'GET' \
        //'https://localhost:7070/api/CartItems' \
        //-H 'accept: */*'
        [HttpGet]
        public IActionResult GetCartItems(double? maxPrice, string prefix, int? pageSize)
        {
            List<CartItem> results = items;

            if (maxPrice.HasValue)
            {
                results = results.Where(item => item.price <= maxPrice.Value).ToList();
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                results = results.Where(item => item.product.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (pageSize.HasValue)
            {
                results = results.Take(pageSize.Value).ToList();
            }


            return Ok(results);
        }


  //      curl -X 'GET' \
  //'https://localhost:7070/api/CartItems/2' \
  //-H 'accept: */*'
          [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CartItem C = items.FirstOrDefault(i => i.id == id);
            if (C == null)
            {
                return NotFound("ID not found");
            }
            return Ok(C);
        }

//        Curl

//curl -X 'POST' \
//  'https://localhost:7070/api/CartItems' \
//  -H 'accept: */*' \
//  -H 'Content-Type: application/json' \
//  -d '{
//  "id": 0,
//  "product": "string",
//  "price": 0,
//  "quantity": 0
//}'
        [HttpPost()]
        public IActionResult AddItem([FromBody] CartItem newItem)
        {
            newItem.id = nextId;
            items.Add(newItem);
            nextId++;
            return Created($"/api/CartItems/{newItem.id}", newItem);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCart(int id, [FromBody] CartItem updatedItem)
        {
            if (id != updatedItem.id) { return BadRequest(); }
            if (items.Any(i => i.id == id)) { return NotFound(); }
            int index = items.FindIndex(i => i.id == id);
            items[index] = updatedItem;
            return Ok(updatedItem);
        }


//        Curl

//curl -X 'DELETE' \
//  'https://localhost:7070/api/CartItems' \
//  -H 'accept: */*'
        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            int index = items.FindIndex((i) => i.id == id);
            if (index == -1)
            { return NotFound("item not found"); }
            else
            {
                items.RemoveAt(index);
                return NoContent();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGC.Data;
using PGC.Models;

namespace PGC.Controllers {
    [Produces ("application/json")]
    [Route ("api/Orders")]
    public class OrdersController : Controller {
        private readonly AspiranturaContext _context;

        public OrdersController (AspiranturaContext context) {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<Order> GetOrders () {
            return _context.Orders;
        }

        // Добавить запись - Create
        // POST: api/Order
        [HttpPost]
        
//        public async Task<IActionResult> PostOrder ([FromBody] OrderFromUI order) {
        public async Task<IActionResult> PostOrder ([FromBody] Order order) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            _context.Orders.Add (order);            
            await _context.SaveChangesAsync ();
            await AddAssignedAspirants(order);            
            return Ok (order.Id);
        }

        // изменить список аспирантов, принадлежащих к приказу
        private async Task AddAssignedAspirants(Order order)
        {
            foreach (var aspirant in order.Aspirants)
            {
                AspirantOrder aspirantOrder = new AspirantOrder { AspirantId = aspirant.Id, OrderId = order.Id };
                _context.AspirantOrders.Add(aspirantOrder);
            }
            await _context.SaveChangesAsync();
        }

        // Удалить запись
        // DELETE: api/Order/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAspirant ([FromRoute] int id) {
            
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var order = await _context.Orders.SingleOrDefaultAsync (m => m.Id == id);
            if (order == null) {
                return NotFound ();
            }

            _context.Orders.Remove (order);
            await _context.SaveChangesAsync ();

            return Ok (order);
        }

        // Редактировать запись
        // PUT: api/SpecialitiesApi/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> PutOrder ([FromRoute] int id, [FromBody] Order order) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != order.Id) {
                return BadRequest ();
            }

            _context.Entry (order).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!OrderExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        // ===========================================================================================================    

        [HttpGet ("types")]
        public IEnumerable<ItemData> GetTypes () {            
            return Helper.GetOrderTypeList();
        }

        private bool OrderExists (int id) {
            return _context.Orders.Any (e => e.Id == id);
        }
        
        public class OrderFromUI{
        public int Id { get; set; }
        
        public string Number { get; set; }

        public string Context { get; set; }

        public DateTime Date { get; set; }

        public int OrdertypeId { get; set; }

        // public OrderType OrderType { get; set; }

        public ICollection<Aspirant> Aspirants { get; set; }
        }
    }
}
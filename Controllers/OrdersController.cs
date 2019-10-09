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

        // список приказов
        // GET: api/Orders
        [HttpGet]
        public IEnumerable<OrderView> Get () {

            var list = _context.Orders.ToList ().Select (i => new OrderView {
                Context = i.Context,
                    Date = i.Date,
                    Id = i.Id,
                    Number = i.Number,
                    OrderType = i.OrderType.GetDisplayName ()
            }).ToList ();
            // var list = _context.Orders.ToList ();
            return list;
        }

        // GET: api/Departments/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> Get ([FromRoute] int id) {
            var order = await _context.Orders.SingleOrDefaultAsync (m => m.Id == id);
            if (order == null) {
                return NotFound ();
            }
            return Ok (order);
        }

        // Добавить запись - Create
        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> PostOrder ([FromBody] OrderView orderView) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            ICollection<Aspirant> aspirants = _context.Aspirants.Where (a => orderView.AspirantIdList.Contains (a.Id)).ToList ();

            Order order = new Order () { Id = orderView.Id, Context = orderView.Context, Date = orderView.Date, OrderType = (OrderType) orderView.OrderTypeValue, Number = orderView.Number, Aspirants = aspirants };
            _context.Orders.Add (order);

            Implementation (orderView, order.OrderType, aspirants);

            await _context.SaveChangesAsync ();
            await AddAssignedAspirants (order);
            return Ok (order.Id);
        }

        // Удалить запись
        // DELETE: api/Order/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id) {

            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var order = await _context.Orders.SingleOrDefaultAsync (m => m.Id == id);
            if (order == null) {
                return NotFound ();
            }

            // Many to Many - выбрать аспирантов по приказу
            var aspirants = GetAspirantsInOrder (id);
            // _context.AspirantOrders.Where (ao => ao.Order.Id == id).Select (ao => ao.Aspirant);

            Otkat (order.OrderType, aspirants);

            _context.Orders.Remove (order);
            await _context.SaveChangesAsync ();

            return Ok (order);
        }

        // Редактировать запись
        // PUT: api/SpecialitiesApi/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put ([FromRoute] int id, [FromBody] OrderView orderView) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            // аспиранты из _context.Aspirants, которые входили в приказ до изменений
            ICollection<Aspirant> oldAspirants = GetAspirantsInOrder (id);

            // аспиранты из _context.Aspirants, которые по Id совпадают с order.AspirantIdList - измененный список
            ICollection<Aspirant> currentAspirants = _context.Aspirants.Where (a => orderView.AspirantIdList.Contains (a.Id)).ToList ();

            // список удаленных из приказа аспирантов 
            ICollection<Aspirant> deletedAspirants = oldAspirants.Where (oa => !currentAspirants.Any (ca => ca.Id == oa.Id)).ToList ();

            // список добавленных в приказа аспирантов 
            ICollection<Aspirant> addedAspirants = currentAspirants.Where (ca => !oldAspirants.Any (oa => oa.Id == ca.Id)).ToList ();

            Order order = _context.Orders.FirstOrDefault (o => o.Id == id);
            order.Aspirants = currentAspirants;
            order.Context = orderView.Context;
            order.Date = orderView.Date;
            order.Number = orderView.Number;
            //  order.OrderType - тип менять не будем - коней на переправе не меняют

            Implementation (orderView, order.OrderType, addedAspirants);
            Otkat (order.OrderType, deletedAspirants);

            _context.Entry (order).State = EntityState.Modified;

            try {
                await EditAssignedAspirantsInOrder (order.Id, deletedAspirants, addedAspirants);
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) { }

            return Ok (orderView);
        }

        // ===========================================================================================================    

        // Список видів наказів
        [HttpGet ("types")]
        public IEnumerable<ItemData> GetTypes () {
            return Helper.GetOrderTypes ();
        }

        // изменить список аспирантов в приказе - в _context.AspirantOrders
        private async Task EditAssignedAspirantsInOrder (int orderId, ICollection<Aspirant> deletedAspirants, ICollection<Aspirant> addedAspirants) {
            // пересечение двух множеств по Id
            // Many to Many - выбрать аспирантов по приказу из _context.Aspirants, которые входили в приказ до изменений
            List<Aspirant> oldAspirants = GetAspirantsInOrder (orderId).ToList ();

            List<AspirantOrder> aspirantsAO = _context.AspirantOrders.Where (i => i.OrderId == orderId).ToList ();

            // выбрать в AspirantOrders записи, которые отсутствуют в aspirantsInOrder (deletedAspirants) - для удаления          
            ICollection<AspirantOrder> forDelete = aspirantsAO.Where (ao => deletedAspirants.Any (a => a.Id == ao.AspirantId)).ToList ();

            // Добавить новых аспирантов в AspirantOrders
            foreach (var aspirant in addedAspirants) {
                AspirantOrder aspirantOrder = new AspirantOrder { AspirantId = aspirant.Id, OrderId = orderId };
                _context.AspirantOrders.Add (aspirantOrder);
            }

            // Удалить  аспирантов из AspirantOrders
            _context.AspirantOrders.RemoveRange (forDelete);

            await _context.SaveChangesAsync ();
        }

        // выполнить приказ - изменить у аспирантов поля, связанные с приказом
        private void Implementation (OrderView orderView, OrderType orderType, ICollection<Aspirant> aspirants) {
            switch (orderType) {
                case OrderType.Зарахування:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Course = 1;
                            aspirant.InputDate = orderView.Date;
                            //   aspirant.Present = true;
                            aspirant.StatusType = StatusType.навчання;
                        }
                        break;
                    }
                case OrderType.Переведення:
                    {
                        foreach (var aspirant in aspirants) {
                            if (aspirant.Course < 5)
                                aspirant.Course++;
                        }
                        break;
                    }
                case OrderType.Закінчення:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.GraduationDate = orderView.Date;
                            aspirant.StatusType = StatusType.закінчив;
                        }
                        break;
                    }
                case OrderType.Відрахування:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.GraduationDate = orderView.Date;
                            aspirant.StatusType = StatusType.відрахований;
                        }
                        break;
                    }
                case OrderType.Академвідпустка:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Vacation = true;
                            aspirant.StatusType = StatusType.академ_відпустка;
                        }
                        break;
                    }
                case OrderType.Вихід_з_відпустки:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Vacation = false;
                            aspirant.StatusType = StatusType.навчання;
                        }
                        break;
                    }
                case OrderType.Зміна_форми_навчання:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Budget = orderView.Budget;
                            aspirant.StudyForm = orderView.StudyForm;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // удалить аспирантов из приказа - отменить - Откат - - изменить у аспирантов поля, связанные с приказом
        private void Otkat (OrderType orderType, ICollection<Aspirant> aspirants) {
            switch (orderType) {
                case OrderType.Зарахування:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Course = 0;
                            aspirant.InputDate = null;
                            // aspirant.Present = false;
                            aspirant.StatusType = StatusType.новий;
                            _context.Entry (aspirant).State = EntityState.Modified;
                        }
                        break;
                    }
                case OrderType.Переведення:
                    {
                        foreach (var aspirant in aspirants) {
                            if (aspirant.Course > 2)
                                aspirant.Course--;
                        }
                        break;
                    }
                case OrderType.Закінчення:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.GraduationDate = null;
                            aspirant.StatusType = StatusType.навчання;
                        }
                        break;
                    }
                case OrderType.Відрахування:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.GraduationDate = null;
                            aspirant.StatusType = StatusType.навчання;
                        }
                        break;
                    }
                case OrderType.Академвідпустка:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Vacation = false;
                            aspirant.StatusType = StatusType.навчання;
                        }
                        break;
                    }
                case OrderType.Вихід_з_відпустки:
                    {
                        foreach (var aspirant in aspirants) {
                            aspirant.Vacation = true;
                            aspirant.StatusType = StatusType.академ_відпустка;
                        }
                        break;
                    }
                case OrderType.Зміна_форми_навчання:
                    {
                        // Ничего не удалять - но сюда и UI не пустит
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // Список аспірантів в AspirantOrders, включених до наказу
        [HttpGet ("aspirants/{orderId}")]
        public ICollection<Aspirant> GetAspirantsInOrder (int orderId) {
            List<Aspirant> o = _context.AspirantOrders.Where (ao => ao.Order.Id == orderId).Select (ao => ao.Aspirant).ToList ();
            return o;
        }

        // добавить в AspirantOrder список аспирантов, принадлежащих к приказу
        private async Task AddAssignedAspirants (Order order) {
            foreach (var aspirant in order.Aspirants) {
                AspirantOrder aspirantOrder = new AspirantOrder { AspirantId = aspirant.Id, OrderId = order.Id };
                _context.AspirantOrders.Add (aspirantOrder);
            }
            await _context.SaveChangesAsync ();
        }

        // изменить поля в _context.Aspirants в соответствии с изменениями в приказе

        private bool OrderExists (int id) {
            return _context.Orders.Any (e => e.Id == id);
        }

        public class OrderFromUI {
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private static List<Car> Cars { get; set; } = new()
        {
            new Car { Id = 1,Model="Bmw",price=100000,ProductionDate=new DateTime(2015,5,3),type="Gas "},
            new Car { Id = 2,Model="Kia",price=150000,ProductionDate=new DateTime(2017,2,25),type="Gas "}
        };

        private readonly ILogger<CarController> logger;

        public CarController(ILogger<CarController> logger )
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Car>> Getall()

        {
            logger.LogCritical(" get all cars");
            return Cars;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Car> Getbyid(int id)
        {
            var car = Cars.FirstOrDefault(c => c.Id == id);
            if (car is null)
            {
                return NotFound();
            }
            return car;
        }


        [HttpPost]
        [Route("v1")]
        public ActionResult Add_v1(Car car)
        {
            car.Id = Cars.Count + 1;
            car.type = "Gas";
            Cars.Add(car);

            return CreatedAtAction(actionName: nameof(Getbyid),
                                    routeValues: new { id = car.Id },
                                    new MessageResponse { Message = "The car has been added successfully" });
        }


        [HttpPost]
        [Route("v2")]
        [ValidateCarType]
        public ActionResult Add_v2(Car car)
        {
            car.Id = Cars.Count + 1;
            Cars.Add(car);

            return CreatedAtAction(actionName: nameof(Getbyid),
                                    routeValues: new { id = car.Id },
                                    new MessageResponse { Message = "The car has been added successfully" });
        }





        [HttpPut]
        [Route("{id}")]
        public ActionResult Edit (Car carrequest ,int id) 
        {
            if(carrequest.Id != id)
            {
                return BadRequest(); 
            }

            var car= Cars.FirstOrDefault(c=>c.Id==id);
            if (car is null) {
                return NotFound();
            }
            
            
                car.price = carrequest.price;
                car.type = carrequest.type;
                car.ProductionDate=carrequest.ProductionDate;
                car.Model=carrequest.Model;

                return NoContent();
            
        
         }

        [HttpDelete]
        [Route("{id}")]

        public ActionResult Delete(int id)
        {
            var car= Cars.FirstOrDefault(c=>c.Id==id);
            if (car is null)
            {
                return NotFound();
            }
            
            
                Cars.Remove(car);
                return NoContent();
            
        }


    }
}

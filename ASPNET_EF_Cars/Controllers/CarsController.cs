using ASPNET_EF_Cars.Data;
using ASPNET_EF_Cars.Models;
using ASPNET_EF_Cars.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ASPNET_EF_Cars.Controllers
{
    public class CarsController : Controller
    {
        private readonly AspcarsContext _context;

        public CarsController(AspcarsContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            var cates = await _context.Categories.ToListAsync();
            var vmodel = new CarViewModel { cars = cars, categories = cates };
            return View(vmodel);
        }


        [HttpPost]
        public async Task<IActionResult> Search(string request)
        {
            var cars = await _context.Cars.ToListAsync();
            var cates = await _context.Categories.ToListAsync();
            decimal val = 0;
            DateOnly dat = new DateOnly();
            if (!request.IsNullOrEmpty())
            {
                if (decimal.TryParse(request, out val))
                {
                    var filteredCars = from p in cars where p.Price == val select p;
                    filteredCars = filteredCars.Union(from p in cars where p.Speed == Convert.ToDouble(val) select p);
                    var model = new CarViewModel { cars = filteredCars, categories = cates };
                    return View("Index", model);
                }
                else
                {
                    var filteredCars = from p in cars where p.Brand == request select p;
                    filteredCars = filteredCars.Union(from p in cars where p.Model == request select p);
                    int catId = cates.SingleOrDefault(p => p.Title == request).CategoryId;
                    filteredCars = filteredCars.Union(from p in cars where p.CategoryId == catId select p);
                    if (DateOnly.TryParse(request, out dat))
                    {
                        filteredCars = filteredCars.Union(from p in cars where p.Year == dat select p);
                    }
                    var v = new CarViewModel { cars = filteredCars, categories = cates };
                    return View("Index", v);
                }
            }
            
            var vmodel = new CarViewModel { cars = cars, categories = cates };
            return View("Index", vmodel);
        }

        [Route("Cars/category/{Title}")]
        public IActionResult CarsByCat(string Title)
        {
            var cates = _context.Categories.ToList();
            int catId = cates.SingleOrDefault(p => p.Title == Title).CategoryId;
            var filteredCars = from p in _context.Cars.ToList() where p.CategoryId == catId select p;
            var vmodel = new CarViewModel { cars = filteredCars, categories = cates };
            return View("Index", vmodel);
        }

        [Route("Cars/price/{price}")]
        public IActionResult CarsByPrice(decimal price)
        {
            var cates = _context.Categories.ToList();
            var filteredCars = from p in _context.Cars.ToList() where p.Price <= price select p;
            var vmodel = new CarViewModel { cars = filteredCars, categories = cates };
            return View("Index", vmodel);
        }

        [Route("Cars/brand/{Brand}")]
        public IActionResult CarsByBrand(string Brand)
        {
            var cates = _context.Categories.ToList();
            var filteredCars = from p in _context.Cars.ToList() where p.Brand == Brand select p;
            var vmodel = new CarViewModel { cars = filteredCars, categories = cates };
            return View("Index", vmodel);
        }

        public async Task<IActionResult> CarsCategories()
        {
            var cars = await _context.Cars.ToListAsync();
            var cates = await _context.Categories.ToListAsync();
            var vmodel = new CarViewModel { cars = cars, categories = cates };
            return View(vmodel);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cates = await _context.Categories.ToListAsync();
            var car = await _context.Cars.FirstOrDefaultAsync(m => m.CarId == id);
            var vmodel = new CarCategory { car = car, categories = cates };
            if (car == null)
            {
                return NotFound();
            }

            return View(vmodel);
        }

        // GET: Cars/Create
        public async Task<IActionResult> Create()
        {
            Car car = new Car();
            var cates = await _context.Categories.ToListAsync();
            var vmodel = new CarCategory { car = car, categories = cates };
            return View(vmodel);
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Brand,Model,Speed,Price,Year,CategoryId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            var cates = await _context.Categories.ToListAsync();
            CarCategory carCategory = new CarCategory { car = car, categories = cates };
            if (car == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Brand,Model,Speed,Price,Year,CategoryId")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}

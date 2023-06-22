using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {
        // GET: DogController1

        private readonly IDogRepository _dogRepo;

        public DogController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }

        public ActionResult Index()
        {
            List<Dog> dogs = _dogRepo.GetAllDogs();

            return View(dogs);
        }

        // GET: DogController1/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController1/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController1/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // POST: DogController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }
    }
}

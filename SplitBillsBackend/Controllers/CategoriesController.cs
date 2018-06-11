using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private ICategoriesRepository _repo;

        public CategoriesController(ICategoriesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var qwe = _repo.GetAll();
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _repo.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Post(Category entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Insert(entity);
            _repo.Save();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Category entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != entity.Id)
            {
                return BadRequest();
            }
            if (_repo.Get(id) == null)
            {
                return NotFound();
            }
            _repo.Update(entity);
            _repo.Save();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Category entity = _repo.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            _repo.Delete(entity);
            _repo.Save();
            return Ok(entity);
        }
    }
}

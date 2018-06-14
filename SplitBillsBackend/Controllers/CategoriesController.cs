using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Models;

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
        public IEnumerable<CategoryModel> Get()
        {
            var all = _repo.GetAll();
            var model = Mapper.Map<IEnumerable<CategoryModel>>(all);
            return (model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _repo.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = Mapper.Map<CategoryModel>(entity);

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody]CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Mapper.Map<Category>(model);
            _repo.Insert(entity);
            _repo.Save();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest();
            }

            var entity = _repo.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(model, entity);

            _repo.Update(entity);
            _repo.Save();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _repo.Get(id);
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

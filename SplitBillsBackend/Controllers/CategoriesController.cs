using System.Collections.Generic;
using AutoMapper;
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
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<CategoryModel> Get()
        {
            var all = _unitOfWork.Categories.GetAll();
            var model = Mapper.Map<IEnumerable<CategoryModel>>(all);
            return model;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _unitOfWork.Categories.Get(id);
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
            _unitOfWork.Categories.Add(entity);
            _unitOfWork.Complete();
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

            var entity = _unitOfWork.Categories.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(model, entity);

            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.Categories.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            _unitOfWork.Categories.Remove(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }
    }
}

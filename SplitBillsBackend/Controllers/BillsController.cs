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
    public class BillsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<BillModel> Get()
        {
            var all = _unitOfWork.BillsRepository.GetAll();
            var model = Mapper.Map<IEnumerable<BillModel>>(all);
            return model;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _unitOfWork.BillsRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Post(Bill entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BillsRepository.Add(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Bill entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != entity.Id)
            {
                return BadRequest();
            }
            if (_unitOfWork.BillsRepository.Get(id) == null)
            {
                return NotFound();
            }
            _unitOfWork.BillsRepository.Update(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Bill entity = _unitOfWork.BillsRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            _unitOfWork.BillsRepository.Remove(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }
    }
}

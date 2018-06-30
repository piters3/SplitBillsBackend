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
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            var all = _unitOfWork.UsersRepository.GetAll();
            var model = Mapper.Map<IEnumerable<UserModel>>(all);
            return (model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _unitOfWork.UsersRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = Mapper.Map<UserModel>(entity);

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Mapper.Map<User>(model);
            _unitOfWork.UsersRepository.Remove(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest();
            }

            var entity = _unitOfWork.UsersRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(model, entity);

            _unitOfWork.UsersRepository.Update(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.UsersRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            _unitOfWork.UsersRepository.Remove(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }
    }
}
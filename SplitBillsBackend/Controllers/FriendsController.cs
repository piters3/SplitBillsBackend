using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Models;

namespace SplitBillsBackend.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<FriendModel> Get()
        {
            var all = _unitOfWork.FriendsRepository.GetAll();
            var model = Mapper.Map<IEnumerable<FriendModel>>(all);
            return model;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _unitOfWork.FriendsRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = Mapper.Map<FriendModel>(entity);

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody]FriendModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Mapper.Map<Friend>(model);
            _unitOfWork.FriendsRepository.Add(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]FriendModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest();
            }

            var entity = _unitOfWork.FriendsRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(model, entity);

            _unitOfWork.FriendsRepository.Update(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _unitOfWork.FriendsRepository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            _unitOfWork.FriendsRepository.Remove(entity);
            _unitOfWork.Complete();
            return Ok(entity);
        }
    }
}
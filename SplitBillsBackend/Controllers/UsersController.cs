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
        private IUsersRepository _repo;

        public UsersController(IUsersRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<UserModel> Get()
        {
            var all = _repo.GetAll();
            var model = Mapper.Map<IEnumerable<UserModel>>(all);
            return (model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var entity = _repo.Get(id);
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
            _repo.Insert(entity);
            _repo.Save();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]UserModel model)
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
        public IActionResult Delete(string id)
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
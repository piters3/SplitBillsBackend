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
    public class FriendsController : Controller
    {
        private readonly IFriendsRepository _repo;

        public FriendsController(IFriendsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<FriendModel> Get()
        {
            //var id = User.Claims.Single(c => c.Type == "id");
            var all = _repo.GetAll();
            var model = Mapper.Map<IEnumerable<FriendModel>>(all);
            return model;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _repo.Get(id);
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
            _repo.Insert(entity);
            _repo.Save();
            return Ok(entity);
        }

        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody]FriendModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (id != model.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var entity = _repo.Get(id);
        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    Mapper.Map(model, entity);

        //    _repo.Update(entity);
        //    _repo.Save();
        //    return Ok(entity);
        //}

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
using EAD_Project.Entities;
using EAD_Project.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EAD_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;
        public UserController(MongoDbService mongoDbService) 
        {
            _users = mongoDbService.Database?.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _users.Find(Builders<User>.Filter.Empty).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetById(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var user = _users.Find(filter).FirstOrDefault();
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user );
        }

        [HttpPut]
        public async Task<ActionResult> Update(User user)
        {
           var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            //var update = Builders<User>.Update
            //    .Set(x => x.Email, user.Email)
            //    .Set(x => x.Password, user.Password);
            //await _users.UpdateOneAsync(filter, update);
            await _users.ReplaceOneAsync(filter, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            await _users.DeleteOneAsync(filter);
            return Ok();
        }

    }
}

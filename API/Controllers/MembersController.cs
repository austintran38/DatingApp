using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")] //localhost:5001/api/members
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        /*
        Use asynchronous methos to allow multiple task to run simultaeously 
        without waiting for previous ones to finish
        */
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers() //user don't need to search, sort, or manipulate the result => return read only list
        {
            var members = await context.Users.ToListAsync();
            return members;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id) //localhost:5001/api/members/bob-id
        {
            var member = await context.Users.FindAsync(id);

            if(member == null) return NotFound();
            return member;
        }
        
    }
}

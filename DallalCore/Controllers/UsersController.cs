using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DallalCore.Data;
using DallalCore.Models;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace DallalCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DallalCoreContext _context;

        public UsersController(DallalCoreContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.userID)
            {
                return BadRequest();
            }
            var checkEmail = _context.User.Where(x => x.Email == user.Email).ToList();
            
            var user2 = _context.User.Find(id);
            if (checkEmail.Count != 0 && user2.Email!=user.Email)
                return new ObjectResult("The Email is already exsists") { StatusCode = 205 };
            user2.addressId = user.addressId;
            user2.Email = user.Email;
            user2.Fname = user.Fname;
            user2.Lname = user.Fname;
            user2.PhoneNumber = user.PhoneNumber;
            user2.WhatsApp = user.WhatsApp;

            _context.Entry(user2).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            var checkEmail = _context.User.Where(x => x.Email == user.Email).ToList();
            if(checkEmail.Count!=0)
                return new ObjectResult("The Email is already exsists") { StatusCode = 205 };

            user.IsActive = true;
            user.IsAdmin = false;
            user.UserPassword = HashPassword(user.UserPassword);
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.userID }, user);
        }
        [HttpPost]
        public async Task<ActionResult<User>> Login(User user)
        {
            bool verify = false;

            var user2 = _context.User.Include(x=>x.address).Where(x => x.Email == user.Email).FirstOrDefault();
            if (user2 != null)
            {
                 verify = VerifyHashedPassword(user2.UserPassword,user.UserPassword);
                if (verify) 
                { 
                    if (user2.IsActive)
                    {
                        return user2;
                    }
                    else
                    {
                        return new ObjectResult("The account is not active,Contact us") { StatusCode = 205 };
                    }
                }else
                    return new ObjectResult("The email or password is not valid") { StatusCode = 205 };
            }
            return new ObjectResult("The email or password is not valid") { StatusCode = 205 };

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = false;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.userID == id);
        }
    }
}

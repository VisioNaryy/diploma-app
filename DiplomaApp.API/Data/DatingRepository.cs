using System.Collections.Generic;
using System.Threading.Tasks;
using DiplomaApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaApp.API.Data {
    public class DatingRepository : IDatingRepository {
        private readonly DataContext _context;
        public DatingRepository (DataContext context) {
            _context = context;
        }


        public void Add<T> (T entity) where T : class {
            _context.Add(entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove(entity);
        }

        public async Task<User> GetUser (int id) {
            //get user including photos
            var user = await _context.Users.Include( p=>p.Photos).FirstOrDefaultAsync(u => u.Id==id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers () {
            //get users with photos
            var users = await _context.Users.Include(p=> p.Photos).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll () {
            //if amounts of changes is move than zero, it will return true

            //if it is equal to zero, it means that nothing was saved to database
            //and that will return false from this method
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
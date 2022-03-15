using Microsoft.EntityFrameworkCore;
using Passman.Data;

namespace Passman
{
    public class PassManService : IDisposable
    {
        private readonly PassManDbContext _db;

        public PassManService()
        {
            _db = new PassManDbContext();
        }

        public void Create(Password password)
        {
            if (password == null)
                throw new NullReferenceException();

            _db.Passwords.Add(password);
            _db.SaveChanges();
        }
        public async Task Update(string site, Password newPassword)
        {
            Password password = await GetAsync(site);
            password.Username = newPassword.Username;
            password.SecretPassword = newPassword.SecretPassword;
            _ = _db.Passwords.Update(newPassword);
            _ = SaveChanges();
        }
        public async Task<Password> GetAsync(string site)
        {
            Password passItem = await _db.Passwords.FirstOrDefaultAsync(x => x.Website == site);
            return passItem ?? throw new Exception($"Password for website {site} cannot be found");
        }

        public async Task<List<Password>> GetAll()
        {
            return await _db.Passwords.ToListAsync();
        }

        private bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        ~PassManService()
        {
            Dispose();
        }
    }
}
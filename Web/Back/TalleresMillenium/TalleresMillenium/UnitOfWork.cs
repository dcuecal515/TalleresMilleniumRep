using TalleresMillenium.Repositories;

namespace TalleresMillenium
{
    public class UnitOfWork
    {
        private readonly TalleresMilleniumContext _context;

        public UnitOfWork(TalleresMilleniumContext context)
        {
            _context = context;
        }
        public TalleresMilleniumContext Context => _context;

        private UserRepository _userRepository;

        public UserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

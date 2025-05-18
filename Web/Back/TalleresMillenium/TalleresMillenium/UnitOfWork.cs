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

        private CocheRepository _cocheRepository;

        public CocheRepository CocheRepository => _cocheRepository ??= new CocheRepository(_context);

        private ChatRepository _chatRepository;

        public ChatRepository ChatRepository => _chatRepository ??= new ChatRepository(_context);

        private MensajeRepository _mensajeRepository;

        public MensajeRepository MensajeRepository => _mensajeRepository ??= new MensajeRepository(_context);

        private ServiceRepository _serviceRepository;

        public ServiceRepository ServiceRepository => _serviceRepository ??= new ServiceRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

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

        private ReviewRepository _reviewRepository;

        public ReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(_context);

        private ProductRepository _productRepository;

        public ProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);

        private Coche_ServicioRepository _coche_SercicioRepository;

        public Coche_ServicioRepository Coche_ServicioRepository => _coche_SercicioRepository ??= new Coche_ServicioRepository(_context);
        
        private ChatUsuarioRepository _chatUsarioRepository;

        public ChatUsuarioRepository ChatUsuarioRepository => _chatUsarioRepository ??= new ChatUsuarioRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

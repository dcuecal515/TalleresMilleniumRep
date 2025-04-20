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

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

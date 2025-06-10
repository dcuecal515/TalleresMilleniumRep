using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ProductRepository : Repository<Producto, int>
    {
        public ProductRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<IEnumerable<Producto>> GetAllProduct()
        {
            return await GetQueryable().Include(s => s.valoraciones).OrderBy(s => s.Nombre).ToListAsync();
        }
        public async Task<Producto> GetProductById(int id)
        {
            Producto producto = await GetQueryable().Include(producto => producto.valoraciones).ThenInclude(valoracion => valoracion.Usuario).FirstOrDefaultAsync(s => s.Id == id);
            return producto;
        }
        public async Task<IEnumerable<Producto>> GetAllProductFull()
        {
            return await GetQueryable().OrderBy(p => p.Nombre) .ToListAsync();
        }
        public async Task<Producto> GetExixtsProductName(string name)
        {
            return await GetQueryable().FirstOrDefaultAsync(p => p.Nombre == name);
        }
    }
}

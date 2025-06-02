

using System.Globalization;
using System.Text;
using TalleresMillenium.DTOs;
using TalleresMillenium.Mappers;
using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class ProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ReviewMapper _reviewMapper;
        public ProductService(UnitOfWork unitOfWork,ReviewMapper reviewMapper) {
            _unitOfWork = unitOfWork;
            _reviewMapper = reviewMapper;
        }
        public async Task<ProductFullDto> GetallProduct(QueryDto queryDto)
        {
            IEnumerable<Producto> productos = await _unitOfWork.ProductRepository.GetAllProduct();
            var productdto = new List<ProductDto>();
            var totalproductos = 0;
            if (queryDto.busqueda != null)
            {
                string separatebusqueda = queryDto.busqueda.Normalize(NormalizationForm.FormD);

                StringBuilder newname = new StringBuilder();
                foreach (char c in separatebusqueda)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        newname.Append(c);
                    }
                }
                string searchbusqueda = newname.ToString().Normalize(NormalizationForm.FormC);

                foreach (Producto producto in productos)
                {
                    string separatenamedatabase = producto.Nombre.Normalize(NormalizationForm.FormD);

                    StringBuilder newnamedatabase = new StringBuilder();
                    foreach (char c in separatenamedatabase)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        {
                            newnamedatabase.Append(c);
                        }
                    }
                    string searchnamedatabase = newnamedatabase.ToString().Normalize(NormalizationForm.FormC);

                    if (searchnamedatabase.ToLower().Contains(searchbusqueda.ToLower()))
                    {
                        var dto = new ProductDto()
                        {
                            Id = producto.Id,
                            Nombre = producto.Nombre,
                            Imagen = producto.Imagen,
                            Disponible=producto.Disponible,
                            Valoraciones = producto.valoraciones.Select(v => v.Puntuacion).ToList()
                        };
                        productdto.Add(dto);
                    }
                }
                totalproductos = productdto.Count();
            }
            else
            {
                totalproductos = productos.Count();
                foreach (var producto in productos)
                {
                    var dto = new ProductDto()
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Imagen = producto.Imagen,
                        Disponible=producto.Disponible,
                        Valoraciones = producto.valoraciones.Select(v => v.Puntuacion).ToList()
                    };
                    productdto.Add(dto);
                }
            }
            ProductFullDto productFullDto = new ProductFullDto { productDto = productdto, totalproduct = totalproductos };

            productFullDto.productDto = productFullDto.productDto.Skip((queryDto.ActualPage - 1) * queryDto.ServicePageSize).Take(queryDto.ServicePageSize).ToList();
            return productFullDto;
        }
        public async Task<ProductoDto> GetProductById(int id)
        {
            Producto producto = await _unitOfWork.ProductRepository.GetProductById(id);
            ProductoDto productoDto = new ProductoDto();

            productoDto.Id = id;
            productoDto.Nombre = producto.Nombre;
            productoDto.Descripcion = producto.Descripcion;
            productoDto.Disponible = producto.Disponible;
            productoDto.Imagen = producto.Imagen;
            foreach (Valoracion valoracion in producto.valoraciones)
            {
                ValoracionDto valoracionDto = _reviewMapper.Todto(valoracion);
                productoDto.valoracionesDto.Add(valoracionDto);
            }

            return productoDto;
        }
        public async Task<ICollection<ProductAdminDto>> GetAllProductFull()
        {
            IEnumerable<Producto> productos = await _unitOfWork.ProductRepository.GetAllProductFull();
            var products = new List<ProductAdminDto>();
            foreach (Producto producto in productos)
            {
                ProductAdminDto productAdminDto = new ProductAdminDto();
                productAdminDto.Id= producto.Id;
                productAdminDto.Nombre= producto.Nombre;
                productAdminDto.Descripcion= producto.Descripcion;
                productAdminDto.Disponible= producto.Disponible;
                productAdminDto.Imagen= producto.Imagen;
                products.Add(productAdminDto);
            }
            return products;
        }
        public async Task<Producto> getProductByIdOnlyAsync(int id)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(id);
        }
        public async Task DeleteProduct(Producto producto)
        {
            _unitOfWork.ProductRepository.Delete(producto);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateProduct(Producto producto)
        {
            _unitOfWork.ProductRepository.Update(producto);
            await _unitOfWork.SaveAsync();
        }
        public async Task InsertProduct(Producto producto)
        {
            await _unitOfWork.ProductRepository.InsertAsync(producto);
            await _unitOfWork.SaveAsync();
        }
    }
}

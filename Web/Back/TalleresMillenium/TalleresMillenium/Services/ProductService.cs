

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
            Producto producto = await _unitOfWork.ProductRepository.GetServiceById(id);
            ProductoDto productoDto = new ProductoDto();

            productoDto.Id = id;
            productoDto.Nombre = producto.Nombre;
            productoDto.Descripcion = producto.Descripcion;
            productoDto.Imagen = producto.Imagen;
            foreach (Valoracion valoracion in producto.valoraciones)
            {
                ValoracionDto valoracionDto = _reviewMapper.Todto(valoracion);
                productoDto.valoracionesDto.Add(valoracionDto);
            }

            return productoDto;
        }
    }
}

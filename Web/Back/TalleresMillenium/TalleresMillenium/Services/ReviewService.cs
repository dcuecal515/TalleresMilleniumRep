using TalleresMillenium.DTOs;
using TalleresMillenium.Mappers;
using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class ReviewService
    {
        private readonly UnitOfWork _unitOfWork;

        public ReviewService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InsertReview(ReviewDto reviewDto,int id)
        {
            ReviewMapper mapper = new ReviewMapper();

            Valoracion valoracion = mapper.Toentity(reviewDto);
            valoracion.UsuarioId = id;

            Servicio servicio = await ObtainService(valoracion.ServicioId.Value);
            List<Valoracion> valoraciones = (List<Valoracion>)servicio.valoraciones;
            valoraciones.Add(valoracion);
            _unitOfWork.ServiceRepository.Update(servicio);

            
            await _unitOfWork.ReviewRepository.InsertAsync(valoracion);
            await _unitOfWork.SaveAsync();

        }

        public async Task InsertReviewProduct(ReviewDto reviewDto,int id)
        {
            ReviewMapper mapper = new ReviewMapper();

            Valoracion valoracion = mapper.ProductToentity(reviewDto);
            valoracion.UsuarioId = id;

            Producto producto = await ObtainProducto(valoracion.ProductoId.Value);
            List<Valoracion> valoraciones = (List<Valoracion>)producto.valoraciones;
            valoraciones.Add(valoracion);
            _unitOfWork.ProductRepository.Update(producto);


            await _unitOfWork.ReviewRepository.InsertAsync(valoracion);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Servicio> ObtainService(int id)
        {
            Servicio servicio = await _unitOfWork.ServiceRepository.GetServiceById(id);
            if (servicio == null)
            {
                throw new Exception($"No se encontró el servicio");
            }
            return servicio;
        }
        public async Task<Producto> ObtainProducto(int id)
        {
            Producto producto = await _unitOfWork.ProductRepository.GetProductById(id);
            if (producto == null)
            {
                throw new Exception($"No se encontró el servicio");
            }
            return producto;
        }
    }
}

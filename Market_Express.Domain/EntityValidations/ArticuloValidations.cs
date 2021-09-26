using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.EntityValidations
{
    public class ArticuloValidations : IArticuloValidations
    {
        private Article _articulo;
        private readonly IUnitOfWork _unitOfWork;

        public ArticuloValidations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Article Articulo
        {
            private get { return _articulo; }
            set { _articulo = value; }
        }

        public bool ExistsCodigoBarras()
        {
            return _unitOfWork.Articulo.GetFirstOrDefault(x => x.CodigoBarras == Articulo.CodigoBarras) != null;
        }
    }
}
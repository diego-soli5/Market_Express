using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IArticleValidations
    {
        bool ExistsCodigoBarras();
        Article Articulo { set; }
    }
}

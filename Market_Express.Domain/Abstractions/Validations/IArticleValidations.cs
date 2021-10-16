using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IArticleValidations
    {
        bool ExistsBarCode();
        Article Article { set; }
    }
}

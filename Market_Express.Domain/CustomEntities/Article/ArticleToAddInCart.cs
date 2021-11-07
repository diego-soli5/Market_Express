namespace Market_Express.Domain.CustomEntities.Article
{
    public class ArticleToAddInCart : Entities.Article
    {
        public int CountInCart { get; set; }

        public bool IsInClientCart => CountInCart > 0;
    }
}

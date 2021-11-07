namespace Market_Express.Domain.CustomEntities.Article
{
    public class ArticleForAuthenticatedUser : Entities.Article
    {
        public int CountInCart { get; set; }

        public bool IsInClientCart => CountInCart > 0;
    }
}

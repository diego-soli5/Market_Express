using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleEditDTO
    {
        [BindProperty(Name = "Article.Id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.CategoryId")]
        public Guid? CategoryId { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.Description")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.BarCode")]
        [StringLength(255)]
        public string BarCode { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.Price")]
        public int Price { get; set; }

        [BindProperty(Name = "Article.Image")]
        public string Image { get; set; }

        [BindProperty(Name = "Article.NewImage")]
        public IFormFile NewImage { get; set; }

        [BindProperty(Name = "Article.AutoSync")]
        public bool AutoSync { get; set; }

        [BindProperty(Name = "Article.AutoSyncDescription")]
        public bool AutoSyncDescription { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.Status")]
        public EntityStatus Status { get; set; }

        [BindProperty(Name = "Article.AddedBy")]
        public string AddedBy { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleCreateDTO
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.CategoryId")]
        public Guid CategoryId { get; set; }

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

        [Required(ErrorMessage = "El campo es obligatorio.")]
        [BindProperty(Name = "Article.NewImage")]
        public IFormFile NewImage { get; set; }
    }
}

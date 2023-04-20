using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Certificate_Generator.Infrastructure.Validation;

namespace Certificate_Generator.Models
{
	public class Template
	{
        
        public int Id { get;  set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageName { get; set; } = string.Empty;

        [NotMapped]
        [FileExtension]
        [Required(ErrorMessage = "You must choose an Image")]
        public IFormFile? ImageUpload { get; set; }
    }
}


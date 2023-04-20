using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Certificate_Generator.Infrastructure.Validation;

namespace Certificate_Generator.Models
{
    public class Certificate
	{
        public long Id { get; set; }

        [Display(Name = "Title")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a Title")]
        public int TitleId { get; set; }

        public Title? Title { get; set; }

        [Display(Name ="Course")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a Course")]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;


        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;


        [Display(Name = "Other Name")]
        public string? OName { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display (Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNum { get; set; } = string.Empty;

        public string ImageName { get; set; } = string.Empty;

        [NotMapped]
        [FileExtension]
        [Required(ErrorMessage = "You must choose an Image")]
        public IFormFile? ImageUpload { get; set; }

        //public string ImageName { get; set; } = string.Empty;

    }
}


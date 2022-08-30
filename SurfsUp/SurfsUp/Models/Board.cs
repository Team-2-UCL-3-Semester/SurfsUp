using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUp.Models
{
    public class Board
    {
        public Guid Id{ get; set; }
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Length { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Width { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Thickness { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Volume { get; set; }
        [Required(ErrorMessage = "*")]
        public string Type { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Price { get; set; }
        public string? Equipment { get; set; }
        public string? imgPath { get; set; }
    }
}

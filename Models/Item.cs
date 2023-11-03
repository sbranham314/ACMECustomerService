﻿using System.ComponentModel.DataAnnotations;

namespace ACMECorpCustomerService.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string? Image {  get; set; }

        [Required]
        public double price { get; set; }

    }
}

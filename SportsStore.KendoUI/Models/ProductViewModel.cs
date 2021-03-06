﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SportsStore.Domain.Entities;

namespace SportsStore.KendoUI.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        
        public Category Category { get; set; }

        [UIHint("GridForeignKey")]
        public int CatID { get; set; }

        [UIHint("ImageModel")]
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
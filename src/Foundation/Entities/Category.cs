using Attributes.WebAttributes.Repository;
using Foundation.Dtos;
using Foundation.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Entities
{
    [Entity(typeof(CategoryDto))]
    public partial class Category : BaseEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Product { get; set; }
    }
    [Entity(typeof(ProductDto))]
    public partial class Product : BaseEntity
    {
        public int ProductId { get; set; }
        public Category Category { get; set; }
    }
}

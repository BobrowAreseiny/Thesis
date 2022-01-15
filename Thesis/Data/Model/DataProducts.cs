using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Thesis.Data.Model
{

    public class DataProduct
    {
        public int Id { get; set; }

        public string ProductCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int? CountOnStorage { get; set; }

        public BitmapFrame ProductImage { get; set; }

        public DataProduct(Product product)
        {
            Id = product.Id;
            ProductCode = product.ProductCode;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            CountOnStorage = product.CountOnStorage;
            ProductImage = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\Image\" + product.ProductImage));
        }
    }
}

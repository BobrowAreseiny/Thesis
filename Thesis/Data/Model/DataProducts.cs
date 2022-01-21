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

        public DateTime? DateOfCreation { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int? CountOnStorage { get; set; }

        public BitmapFrame ProductImage { get; set; }

        public int? SeasonId { get; set; }

        public int? MaterialId { get; set; }

        public int? SexId { get; set; }

        public DataProduct(Product product)
        {
            try
            {
                ProductImage = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\Image\" + product.ProductImage));
            }
            catch
            {
                ProductImage = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\noimage.png"));
            }          
            Id = product.Id;
            ProductCode = product.ProductCode;
            DateOfCreation = product.DateOfCreation;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            CountOnStorage = product.CountOnStorage;
            SeasonId = product.SeasonId;
            MaterialId = product.MaterialId;
            SexId = product.SexId;
        }
    }
}

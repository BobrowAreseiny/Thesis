using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Thesis.Data.Model
{
    public class CreelData
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public int SizeId { get; set; }

        public BitmapFrame Image { get; set; }

        public int Count { get; set; }

        public CreelData(Basket basket, Product product)
        {
            try
            {
                Image = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\Image\" + product.ProductImage));
            }
            catch
            {
                Image = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\noimage.png"));
            }
            Name = product.Name;
            Size = basket.Size.Size;
            SizeId = basket.Size.Id;
            Count = basket.Count;
        }
    };
}

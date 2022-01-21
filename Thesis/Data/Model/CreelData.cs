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
            Uri uri = new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\Image\" + product.ProductImage);
            if (uri != null)
            {
                Image = BitmapFrame.Create(uri);
            }
            Name = product.Name;
            Size = basket.Size.Size;
            SizeId = basket.Size.Id;
            Count = basket.Count;
        }
    };
}

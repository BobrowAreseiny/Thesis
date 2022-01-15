using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Thesis.Data.Model
{
    public class CreelData
    {
        public string Name;

        public string Size;

        public int SizeId;

        public BitmapFrame Image;

        public int Count;

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

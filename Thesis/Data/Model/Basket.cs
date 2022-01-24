
namespace Thesis.Data.Model
{
    public class Basket
    {
        public ProductSize Size { get; set; }

        public string Product { get; set; }

        public string SizeName { get; set; }

        public int Count { get; set; }

        public double Cost { get; set; }

        public Basket(ProductSize productSize, int count)
        {
            Size = productSize;
            Product = productSize.Product.Name;
            SizeName = productSize.Size;
            Count = count;
            Cost = (double)productSize.Product.Price * count;
        }
    }
}


namespace Thesis.Data.Model
{
    public class Basket
    {
        public ProductSize Size { get; set; }

        public int Count { get; set; }

        public Basket(ProductSize productSize, int count)
        {
            Size = productSize;
            Count = count;
        }
    }
}

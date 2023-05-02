using store_backend.Context;
using store_backend.Dto;

namespace store_backend.Dao
{
    public class ProductDao : IProductDao
    {
        ApplicationContext context;

        public ProductDao(ApplicationContext dbcontext) {
            context = dbcontext;
        }
        public IEnumerable<ProductDto> Get()
        {
            return context.Products;
        }

        public async Task<int> AddProduct(ProductDto product)
        {

                    context.Add(product);

                    return await context.SaveChangesAsync();
        }

        public async Task UpdateProduct(int id, ProductDto product)
        {
            var productoActual= context.Products.Find(id);
            if (productoActual != null)
            {
                productoActual.Name = product.Name;
                productoActual.Code = product.Code;
                productoActual.Description = product.Description;
                productoActual.Image = product.Image;
                productoActual.Price = product.Price;
                productoActual.Category = product.Category;
                productoActual.Quantity = product.Quantity;
                productoActual.InventoryStatus = product.InventoryStatus;
                productoActual.Rating = product.Rating;

               await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("There was an error in the execution");

            }
        }

        public async Task DeleteProduct(int id)
        {
            var productoActual = context.Products.Find(id);

            try
            {
                if (productoActual != null)
                {
                    context.Remove(productoActual);

                    await context.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    public interface IProductDao
    {
        IEnumerable<ProductDto> Get();
        Task<int> AddProduct(ProductDto producto);
        Task UpdateProduct(int id, ProductDto producto);
        Task DeleteProduct(int id);
    }
}


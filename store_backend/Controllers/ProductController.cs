using Microsoft.AspNetCore.Mvc;
using store_backend.Dao;
using store_backend.Dto;
using store_backend.Authorization;
using store_backend.Enums;

namespace store_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        IProductDao productDao;
        
        public ProductController(IProductDao products)
        {
            this.productDao = products;
        }
        /// <summary>
        /// This method returns the products inside the database
        /// </summary>
        /// <response code="200">Returns the list of products</response>
        /// <response code="400">If the the products cannot be returned</response>
        [Authorize(Roles.AdministradorInventario)]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public IActionResult Get()
        {
            return Ok(productDao.Get());
        }

        /// <summary>
        /// This method create a product into the db
        /// </summary>
        /// <response code="201">Insert a new product into the db</response>
        /// <response code="400">If the product cannot be inserted into the db</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] ProductDto product)
        {
            try
            {
                var response= await productDao.AddProduct(product);
                
                if(response == 1)
                    return StatusCode(201, new {Message="Item created correctly"});
                else
                   return BadRequest(StatusCode(404,new { Message="There was an error creating the product"}));

            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method update a product into the db
        /// </summary>
        /// <param name="id">This is the product id</param>
        /// <returns></returns>
        /// <response code="202">Update an existing product in the db</response>
        /// <response code="400">If the product cannot be updated into the db</response>
        [HttpPut("/{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] ProductDto product)
        {
            try { 
                await productDao.UpdateProduct(id, product);

                return StatusCode(202, new { Message = "Product succesfully updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method delete a product according the provided product id
        /// </summary>
        /// <param name="id">This is the product id to be deleted</param>
        /// <returns></returns>
        /// <response code="202">Delete a product in the database</response>
        /// <response code="400">If the product cannot be deleted into the db</response>
        [HttpDelete("/{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteProduct(int id, [FromBody] ProductDto product)
        {
            try
            {
                await productDao.DeleteProduct(id);

                return StatusCode(202, new { Message = "Product succesfully deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Produtos_rabbitMq.UseCases.Product.ProductBuy;

namespace Produtos_rabbitMq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> BuyProduct([FromServices] IProductBuyUseCase productBuyUseCase,  int productId, int quantity)
        {
            try
            { 
                await productBuyUseCase.ExecuteAsync(productId, quantity);
                return Ok("Notificaremos quando a compra for concluida");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

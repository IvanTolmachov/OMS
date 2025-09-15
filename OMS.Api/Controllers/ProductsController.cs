using Microsoft.AspNetCore.Mvc;
using OMS.Api.Models;
using OMS.Api.Services;
using FluentValidation;
using FluentValidation.Results;

namespace OMS.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IValidator<ProductDto> _productValidator;

        public ProductsController(IProductService service, IValidator<ProductDto> productValidator)
        {
            _service = service;
            _productValidator = productValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await _service.GetAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            ValidationResult validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            var created = await _service.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto product)
        {
            ValidationResult validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            return await _service.UpdateAsync(id, product) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
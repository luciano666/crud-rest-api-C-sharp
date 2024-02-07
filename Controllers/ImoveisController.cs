namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Imoveis;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class ImoveisController : ControllerBase
{
    private IImovelService _imovelService;

    public ImoveisController(IImovelService imovelService)
    {
        _imovelService = imovelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var imoveis = await _imovelService.GetAll();
        return Ok(imoveis);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var imovel = await _imovelService.GetById(id);
        return Ok(imovel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRequest model)
    {
        await _imovelService.Create(model);
        return Ok(new { message = "Imovel criado com sucesso" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRequest model)
    {
        await _imovelService.Update(id, model);
        return Ok(new { message = "Imovel atualizado com sucesso" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _imovelService.Delete(id);
        return Ok(new { message = "Imovel excluido com sucesso" });
    }
}

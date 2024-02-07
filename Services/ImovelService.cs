namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Imoveis;
using WebApi.Repositories;

public interface IImovelService
{
    Task<IEnumerable<Imovel>> GetAll();
    Task<Imovel> GetById(int id);
    Task Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
    Task Delete(int id);
}

public class ImovelService : IImovelService
{
    private IImovelRepository _imovelRepository;
    private readonly IMapper _mapper;

    public ImovelService(
        IImovelRepository imovelRepository,
        IMapper mapper)
    {
        _imovelRepository = imovelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Imovel>> GetAll()
    {
        return await _imovelRepository.GetAll();
    }

    public async Task<Imovel> GetById(int id)
    {
        var imovel = await _imovelRepository.GetById(id);

        if (imovel == null)
            throw new KeyNotFoundException("Imovel nao encontrado");

        return imovel;
    }

    public async Task Create(CreateRequest model)
    {
        // validate
        if (await _imovelRepository.GetByEndereco(model.Endereco!) != null)
            throw new AppException("Jah existe imovel com o endereco '" + model.Endereco);

        // mapeia model para objeto imovel
        var imovel = _mapper.Map<Imovel>(model);

        // hash password
        imovel.PasswordHash = BCrypt.HashPassword(model.Password);

        // salva Imovel
        await _imovelRepository.Create(imovel);
    }

    public async Task Update(int id, UpdateRequest model)
    {
        var imovel = await _imovelRepository.GetById(id);

        if (imovel == null)
            throw new KeyNotFoundException("Imovel nao encontrado");

        // validate
        var enderecoAlterado = !string.IsNullOrEmpty(model.Endereco) && imovel.Endereco != model.Endereco;
        if (enderecoAlterado && await _imovelRepository.GetByEndereco(model.Endereco!) != null)
            throw new AppException("Jah existe imovel com o endereco '" + model.Endereco);

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            imovel.PasswordHash = BCrypt.HashPassword(model.Password);

        // copia model para imovel
        _mapper.Map(model, imovel);

        // salva imovel
        await _imovelRepository.Update(imovel);
    }

    public async Task Delete(int id)
    {
        await _imovelRepository.Delete(id);
    }
}

namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IImovelRepository
{
    Task<IEnumerable<Imovel>> GetAll();
    Task<Imovel> GetById(int id);
    Task<Imovel> GetByEndereco(string email);
    Task Create(Imovel imovel);
    Task Update(Imovel imovel);
    Task Delete(int id);
}

public class ImovelRepository : IImovelRepository
{
    private DataContext _context;

    public ImovelRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Imovel>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Imoveis
        """;
        return await connection.QueryAsync<Imovel>(sql);
    }

    public async Task<Imovel> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Imoveis 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Imovel>(sql, new { id });
    }

    public async Task<Imovel> GetByEndereco(string endereco)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Imoveis 
            WHERE Endereco = @endereco
        """;
        return await connection.QuerySingleOrDefaultAsync<Imovel>(sql, new { imovel });
    }

    public async Task Create(Imovel imovel)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Imoveis (Descricao, Endereco, Role, PasswordHash)
            VALUES (@Descricao, @Endereco, @Role, @PasswordHash)
        """;
        await connection.ExecuteAsync(sql, imovel);
    }

    public async Task Update(Imovel imovel)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Imoveis 
            SET Descricao = @Descricao,
                Endereco = @Endereco, 
                Role = @Role, 
                PasswordHash = @PasswordHash
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, imovel);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Imoveis 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}

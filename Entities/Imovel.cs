namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class Imovel
{
    public int Id { get; set; }
    public string? Descricao { get; set; }
    public string? Endereco { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public string? PasswordHash { get; set; }
}

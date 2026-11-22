using System.Text.Json.Serialization;

namespace IbgeNomes.Models;

/// <summary>
/// Mapeia um item de: GET /api/v1/localidades/estados/{UF}/municipios
/// (mesmo formato de /api/v1/localidades/municipios).
/// Só os campos usados pelo app são mapeados; o resto do JSON é ignorado.
/// </summary>
public class Municipio
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>Item especial do Picker que significa "ver o estado inteiro".</summary>
    [JsonIgnore]
    public bool EhTodoEstado => Id == 0;

    public static Municipio TodoEstado() => new() { Id = 0, Nome = "— Todo o estado —" };
}

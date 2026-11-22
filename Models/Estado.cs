using System.Text.Json.Serialization;

namespace IbgeNomes.Models;

/// <summary>
/// Mapeia um item de: GET /api/v1/localidades/estados
/// </summary>
public class Estado
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sigla")]
    public string Sigla { get; set; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("regiao")]
    public Regiao? Regiao { get; set; }

    /// <summary>Texto exibido no Picker. Ex.: "Minas Gerais (MG)"</summary>
    [JsonIgnore]
    public string NomeExibicao => $"{Nome} ({Sigla})";
}

public class Regiao
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sigla")]
    public string Sigla { get; set; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
}

using System.Text.Json.Serialization;

namespace IbgeNomes.Models;

/// <summary>
/// Mapeia exatamente o JSON de: GET /api/v3/nomes/2022/localidade/{id}
///
/// Exemplo real (id 31 = Minas Gerais):
/// {
///   "localidade": 31,
///   "mulheres":   [ { "nome": "maria", "frequencia": 1297501 }, ... ],
///   "homens":     [ { "nome": "jose",  "frequencia": 528206 },  ... ],
///   "sobrenomes": [ { "nome": "silva", "frequencia": 3162886 }, ... ]
/// }
/// Cada lista vem com os 10 mais frequentes.
/// </summary>
public class RespostaNomesLocalidade
{
    [JsonPropertyName("localidade")]
    public int Localidade { get; set; }

    [JsonPropertyName("mulheres")]
    public List<NomeApi> Mulheres { get; set; } = new();

    [JsonPropertyName("homens")]
    public List<NomeApi> Homens { get; set; } = new();

    [JsonPropertyName("sobrenomes")]
    public List<NomeApi> Sobrenomes { get; set; } = new();
}

/// <summary>Um item das listas acima.</summary>
public class NomeApi
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("frequencia")]
    public long Frequencia { get; set; }
}

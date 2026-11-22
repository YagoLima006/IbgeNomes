using System.Net.Http.Json;
using IbgeNomes.Models;

namespace IbgeNomes.Services;

/// <summary>
/// Consome a API de Nomes do IBGE (Censo 2022) usando o id vindo da API de Localidades.
/// </summary>
public class NomesService : INomesService
{
    private readonly HttpClient _http;

    public NomesService(HttpClient http) => _http = http;

    public async Task<ResultadoNomes> ObterNomesAsync(int localidadeId, CancellationToken ct = default)
    {
        RespostaNomesLocalidade? resposta;

        try
        {
            // GET /api/v3/nomes/2022/localidade/{id}  →  desserializa direto no Model
            resposta = await _http.GetFromJsonAsync<RespostaNomesLocalidade>(
                IbgeApi.NomesPorLocalidade(localidadeId), ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new IbgeApiException("Não foi possível consultar a API de Nomes do IBGE.", ex);
        }

        var resultado = new ResultadoNomes
        {
            LocalidadeId = localidadeId,
            Masculinos = Converter(resposta?.Homens),
            Femininos = Converter(resposta?.Mulheres),
            Sobrenomes = Converter(resposta?.Sobrenomes),
        };

        if (resultado.EstaVazio)
            throw new IbgeApiException("A API não retornou nomes para essa localidade.");

        return resultado;
    }

    /// <summary>Ordena por frequência e numera o ranking (1º, 2º, ...).</summary>
    private static List<NomeFrequencia> Converter(List<NomeApi>? itens) =>
        (itens ?? new())
            .OrderByDescending(n => n.Frequencia)
            .Select((n, i) => new NomeFrequencia
            {
                Ranking = i + 1,
                Nome = n.Nome,
                Frequencia = n.Frequencia,
            })
            .ToList();
}

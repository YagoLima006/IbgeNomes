using System.Net.Http.Json;
using IbgeNomes.Models;

namespace IbgeNomes.Services;

/// <summary>
/// Consome a API de Localidades do IBGE (estados e municípios).
/// </summary>
public class LocalidadesService : ILocalidadesService
{
    private readonly HttpClient _http;

    public LocalidadesService(HttpClient http) => _http = http;

    public async Task<List<Estado>> ObterEstadosAsync(CancellationToken ct = default)
    {
        try
        {
            var estados = await _http.GetFromJsonAsync<List<Estado>>(IbgeApi.Estados, ct);
            return (estados ?? new()).OrderBy(e => e.Nome).ToList();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new IbgeApiException("Não foi possível carregar os estados do IBGE.", ex);
        }
    }

    public async Task<List<Municipio>> ObterMunicipiosAsync(int estadoId, CancellationToken ct = default)
    {
        try
        {
            var municipios = await _http.GetFromJsonAsync<List<Municipio>>(
                IbgeApi.MunicipiosDoEstado(estadoId), ct);
            return (municipios ?? new()).OrderBy(m => m.Nome).ToList();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new IbgeApiException("Não foi possível carregar os municípios do estado.", ex);
        }
    }
}

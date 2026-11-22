using IbgeNomes.Models;

namespace IbgeNomes.Services;

public interface INomesService
{
    /// <summary>
    /// Busca os nomes mais comuns (homens, mulheres e sobrenomes) de um estado ou município.
    /// </summary>
    Task<ResultadoNomes> ObterNomesAsync(int localidadeId, CancellationToken ct = default);
}

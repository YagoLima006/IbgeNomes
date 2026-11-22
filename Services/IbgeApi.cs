namespace IbgeNomes.Services;

/// <summary>
/// Endereços das APIs do IBGE usados pelo app (centralizados num lugar só).
/// </summary>
public static class IbgeApi
{
    public const string BaseUrl = "https://servicodados.ibge.gov.br/api/";

    // API de Localidades (v1)
    public const string Estados = "v1/localidades/estados?orderBy=nome";
    public static string MunicipiosDoEstado(int estadoId) =>
        $"v1/localidades/estados/{estadoId}/municipios?orderBy=nome";

    // API de Nomes - Censo 2022 (v3): recebe o id do estado ou do município
    public static string NomesPorLocalidade(int localidadeId) =>
        $"v3/nomes/2022/localidade/{localidadeId}";
}

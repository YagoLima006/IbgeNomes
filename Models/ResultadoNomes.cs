namespace IbgeNomes.Models;

/// <summary>
/// Resultado já processado da API de Nomes, pronto para a tela.
/// </summary>
public class ResultadoNomes
{
    public int LocalidadeId { get; set; }

    public List<NomeFrequencia> Masculinos { get; set; } = new();
    public List<NomeFrequencia> Femininos { get; set; } = new();
    public List<NomeFrequencia> Sobrenomes { get; set; } = new();

    public bool EstaVazio =>
        Masculinos.Count == 0 && Femininos.Count == 0 && Sobrenomes.Count == 0;
}

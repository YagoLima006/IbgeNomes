using System.Globalization;

namespace IbgeNomes.Models;

/// <summary>
/// Um nome (ou sobrenome) e quantas pessoas o têm na localidade.
/// </summary>
public class NomeFrequencia
{
    public int Ranking { get; set; }
    public string Nome { get; set; } = string.Empty;
    public long Frequencia { get; set; }

    private static readonly CultureInfo PtBr = new("pt-BR");

    // ---- Propriedades prontas para a tela ----

    /// <summary>"MARIA" / "maria" → "Maria"</summary>
    public string NomeFormatado =>
        PtBr.TextInfo.ToTitleCase(Nome.Trim().ToLower(PtBr));

    /// <summary>12284478 → "12.284.478 pessoas"</summary>
    public string FrequenciaFormatada =>
        $"{Frequencia.ToString("N0", PtBr)} pessoas";

    public string RankingFormatado => $"{Ranking}º";
}

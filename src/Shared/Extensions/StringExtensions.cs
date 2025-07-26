using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Formata uma string como CPF (000.000.000-00).
    /// </summary>
    public static string ToCPFFormat(this string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !Regex.IsMatch(cpf, @"^\d+$"))
            return cpf; // Retorna a string original se não puder formatar

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    /// <summary>
    /// Remove caracteres não numéricos de uma string.
    /// </summary>
    public static string RemoveNonNumeric(this string input)
    {
        return Regex.Replace(input, @"\D", string.Empty);
    }

    /// <summary>
    /// Normaliza uma string, removendo acentos e caracteres especiais.
    /// </summary>
    public static string NormalizeString(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var normalized = input.Normalize(NormalizationForm.FormD);
        var regex = new Regex(@"[^a-zA-Z0-9\s]");
        return regex.Replace(normalized, "");
    }
}

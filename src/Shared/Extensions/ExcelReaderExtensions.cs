
using ExcelDataReader;
using System.Data;

namespace Shared.Extensions;
public static class ExcelReaderExtensions
{
    /// <summary>
    /// Lê um arquivo Excel (.xls ou .xlsx) e retorna uma lista de dicionários com os dados.
    /// Cada dicionário representa uma linha, onde a chave é o nome da coluna.
    /// </summary>
    /// <param name="fileStream">Stream do arquivo Excel.</param>
    /// <returns>Lista de linhas representadas por dicionários.</returns>
    public static List<Dictionary<string, object>> ReadExcel(this Stream fileStream)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var reader = ExcelReaderFactory.CreateReader(fileStream);
        var result = new List<Dictionary<string, object>>();

        // Ler o cabeçalho
        var dataTable = reader.AsDataSet().Tables[0];
        var headers = new List<string>();

        foreach (DataColumn column in dataTable.Columns)
            headers.Add(column.ColumnName);

        // Processar as linhas
        foreach (DataRow row in dataTable.Rows)
        {
            var rowDict = new Dictionary<string, object>();
            for (int i = 0; i < headers.Count; i++)
            {
                rowDict[headers[i]] = row[i];
            }
            result.Add(rowDict);
        }

        return result;
    }
}


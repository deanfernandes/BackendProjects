
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BackendProjects.DataExporter
{
    public class JsonDataExporter : IDataExporter
    {
        public string Export<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // pretty print
            };

            return JsonSerializer.Serialize(data, options);
        }
    }

    public class CsvDataExporter : IDataExporter
    {
        public string Export<T>(IEnumerable<T> data)
        {
            if (data == null || !data.Any())
                return string.Empty;

            var sb = new StringBuilder();

            // Get all public properties of T
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Header row
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Data rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var val = p.GetValue(item, null);
                    return Escape(val?.ToString() ?? string.Empty);
                });
                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString();
        }

        // Escape commas, quotes and newlines in CSV values
        private string Escape(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }
    }

    public class DataExporterFactory
    {
        public static IDataExporter CreateDataExporter(string format)
        {
            return format.ToLower() switch
            {
                "json" => new JsonDataExporter(),
                "csv" => new CsvDataExporter(),
                _ => throw new NotSupportedException($"Format '{format}' not supported.")
            };
        }
    }

    public static class DataExporterHelper
    {
        public static void ExportToFile<T>(IEnumerable<T> data, params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                string extension = Path.GetExtension(filePath).TrimStart('.').ToLower();
                try
                {
                    IDataExporter dataExporter = DataExporterFactory.CreateDataExporter(extension);
                    File.WriteAllText(filePath, dataExporter.Export(data));
                    Console.WriteLine($"Data exported to {filePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
        }
    }
}

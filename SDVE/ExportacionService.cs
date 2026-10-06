using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace SDVE
{
    /// <summary>Todo lo que se exporta: filtro usado, resultados y estadisticas.</summary>
    public class ReporteResultados
    {
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public FiltroResultados Filtro { get; set; } = new();
        public List<ResultadoConvocatoria> Resultados { get; set; } = new();
        /// <summary>Nulo si no se indico el padron.</summary>
        public Estadisticas? ParticipacionGeneral { get; set; }
        public List<Estadisticas> ParticipacionPorConvocatoria { get; set; } = new();
    }

    /// <summary>Exporta un reporte a CSV (Excel), JSON o XML.</summary>
    public class ExportacionService
    {
        private const string EncabezadoCsv =
            "Convocatoria,Candidato,Tipo,Votos,Porcentaje,TotalVotosConvocatoria,Grupo,Carrera,Centro";

        /// <summary>CSV con UTF-8 (BOM) para que Excel muestre bien los acentos.</summary>
        public void ExportarCsv(string ruta, ReporteResultados reporte)
        {
            string grupo = Texto(reporte.Filtro.Grupo);
            string carrera = Texto(reporte.Filtro.Carrera);
            string centro = Texto(reporte.Filtro.CentroUniversitario);

            var lineas = new List<string> { EncabezadoCsv };
            foreach (var conv in reporte.Resultados)
            {
                foreach (var c in conv.Candidatos)
                {
                    lineas.Add(string.Join(",",
                        Esc(conv.Convocatoria),
                        Esc(c.Candidato),
                        c.EsWriteIn ? "No registrado" : "Registrado",
                        c.Votos.ToString(CultureInfo.InvariantCulture),
                        c.Porcentaje.ToString("0.00", CultureInfo.InvariantCulture),
                        conv.TotalVotos.ToString(CultureInfo.InvariantCulture),
                        Esc(grupo), Esc(carrera), Esc(centro)));
                }
            }
            File.WriteAllLines(ruta, lineas, new UTF8Encoding(true));
        }

        public void ExportarJson(string ruta, ReporteResultados reporte)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // acentos legibles
            };
            File.WriteAllText(ruta, JsonSerializer.Serialize(reporte, opciones), new UTF8Encoding(false));
        }

        public void ExportarXml(string ruta, ReporteResultados reporte)
        {
            var serializador = new XmlSerializer(typeof(ReporteResultados));
            var config = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };
            using var writer = XmlWriter.Create(ruta, config);
            serializador.Serialize(writer, reporte);
        }

        private static string Texto(string? filtro) =>
            string.IsNullOrWhiteSpace(filtro) ? "Todos" : filtro.Trim();

        /// <summary>
        /// Escapa para CSV. Los textos que empiezan con = + - @ se marcan con apostrofo
        /// para que Excel no los ejecute como formula (los alumnos pueden escribir cualquier nombre).
        /// </summary>
        private static string Esc(string s)
        {
            if (s.Length > 0 && "=+-@\t\r".Contains(s[0])) s = "'" + s;
            return s.Contains(',') || s.Contains('"') || s.Contains('\n')
                ? "\"" + s.Replace("\"", "\"\"") + "\""
                : s;
        }
    }
}

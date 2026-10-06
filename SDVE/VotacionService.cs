using System.Globalization;
using System.Text;

namespace SDVE
{
    /// <summary>Registra votos, controla duplicados y los guarda en un CSV (votos.csv).</summary>
    public class VotacionService
    {
        private const string Encabezado = "AlumnoId,Grupo,Carrera,Centro,Convocatoria,Candidato,EsWriteIn,FechaHora";
        private readonly string _ruta;
        private readonly List<Voto> _votos = new();
        private readonly object _lock = new();

        public VotacionService(string? rutaArchivo = null)
        {
            _ruta = rutaArchivo ?? Path.Combine(AppContext.BaseDirectory, "votos.csv");
            Cargar();
        }

        /// <summary>Copia de todos los votos registrados (para el modulo de resultados).</summary>
        public IReadOnlyList<Voto> ObtenerVotos()
        {
            lock (_lock) return _votos.ToList();
        }

        /// <summary>Devuelve las convocatorias (de las indicadas) en las que el alumno ya voto.</summary>
        public List<string> ConvocatoriasYaVotadas(string alumnoId, IEnumerable<string> convocatorias)
        {
            lock (_lock)
            {
                return convocatorias
                    .Where(c => _votos.Any(v =>
                        string.Equals(v.AlumnoId, alumnoId, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(v.Convocatoria, c, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
        }

        /// <summary>Registra los votos del alumno. Falla si ya voto en alguna de esas convocatorias.</summary>
        public bool RegistrarVotos(Alumno alumno, IEnumerable<Voto> votos, out string error)
        {
            var lista = votos.ToList();
            if (lista.Count == 0) { error = "No hay votos para registrar."; return false; }

            lock (_lock)
            {
                var repetidas = ConvocatoriasYaVotadas(alumno.Id, lista.Select(v => v.Convocatoria));
                if (repetidas.Count > 0)
                {
                    error = "El alumno ya voto en: " + string.Join(", ", repetidas) + ".";
                    return false;
                }

                try
                {
                    foreach (var v in lista)
                    {
                        v.AlumnoId = alumno.Id;
                        v.Grupo = alumno.Grupo;
                        v.Carrera = alumno.Carrera;
                        v.CentroUniversitario = alumno.CentroUniversitario;
                        v.FechaHora = DateTime.Now;
                    }

                    bool nuevo = !File.Exists(_ruta);
                    var lineas = new List<string>();
                    if (nuevo) lineas.Add(Encabezado);
                    lineas.AddRange(lista.Select(ALinea));
                    File.AppendAllLines(_ruta, lineas, Encoding.UTF8);

                    _votos.AddRange(lista);
                    error = "";
                    return true;
                }
                catch (Exception ex)
                {
                    error = "No se pudo guardar el voto: " + ex.Message;
                    return false;
                }
            }
        }

        // ---------- CSV ----------
        private static string ALinea(Voto v) => string.Join(",",
            Esc(v.AlumnoId), Esc(v.Grupo), Esc(v.Carrera), Esc(v.CentroUniversitario),
            Esc(v.Convocatoria), Esc(v.Candidato), v.EsWriteIn ? "1" : "0",
            v.FechaHora.ToString("o", CultureInfo.InvariantCulture));

        private static string Esc(string s) =>
            s.Contains(',') || s.Contains('"') || s.Contains('\n')
                ? "\"" + s.Replace("\"", "\"\"") + "\""
                : s;

        private void Cargar()
        {
            if (!File.Exists(_ruta)) return;
            foreach (var linea in File.ReadLines(_ruta, Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var c = Separar(linea);
                if (c.Count < 8) continue;
                _votos.Add(new Voto
                {
                    AlumnoId = c[0], Grupo = c[1], Carrera = c[2], CentroUniversitario = c[3],
                    Convocatoria = c[4], Candidato = c[5], EsWriteIn = c[6] == "1",
                    FechaHora = DateTime.TryParse(c[7], CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind, out var f) ? f : DateTime.MinValue
                });
            }
        }

        private static List<string> Separar(string linea)
        {
            var campos = new List<string>();
            var sb = new StringBuilder();
            bool comillas = false;
            for (int i = 0; i < linea.Length; i++)
            {
                char ch = linea[i];
                if (comillas)
                {
                    if (ch == '"' && i + 1 < linea.Length && linea[i + 1] == '"') { sb.Append('"'); i++; }
                    else if (ch == '"') comillas = false;
                    else sb.Append(ch);
                }
                else if (ch == '"') comillas = true;
                else if (ch == ',') { campos.Add(sb.ToString()); sb.Clear(); }
                else sb.Append(ch);
            }
            campos.Add(sb.ToString());
            return campos;
        }
    }
}

namespace SDVE
{
    /// <summary>
    /// Procesa los votos registrados: conteo, agrupacion, filtros, participacion y abstencionismo.
    /// Todos los metodos leen los votos al momento de llamarlos (siempre datos actuales).
    /// </summary>
    public class ResultadoService
    {
        public const string NombreGeneral = "General";

        private readonly Func<IEnumerable<Voto>> _fuente;

        /// <summary>Uso normal: toma los votos del servicio de votacion.</summary>
        public ResultadoService(VotacionService votacion)
        {
            _fuente = () => votacion.ObtenerVotos();
        }

        /// <summary>Uso para pruebas: toma una lista de votos fija.</summary>
        public ResultadoService(IEnumerable<Voto> votos)
        {
            var lista = votos.ToList();
            _fuente = () => lista;
        }

        // ---------- Listas para llenar los filtros (ComboBox del modulo de reportes) ----------
        public List<string> ObtenerGrupos() => Distintos(v => v.Grupo);
        public List<string> ObtenerCarreras() => Distintos(v => v.Carrera);
        public List<string> ObtenerCentros() => Distintos(v => v.CentroUniversitario);

        // ---------- Conteo ----------
        /// <summary>Votos por candidato en cada convocatoria, con filtro opcional.</summary>
        public List<ResultadoConvocatoria> ContarVotos(FiltroResultados? filtro = null) =>
            Contar(VotosFiltrados(filtro));

        /// <summary>Total de votos (filtrados) emitidos.</summary>
        public int TotalVotos(FiltroResultados? filtro = null) => VotosFiltrados(filtro).Count;

        /// <summary>Resultados separados por grupo, carrera o centro universitario.</summary>
        public List<ResultadoGrupo> AgruparPor(DimensionAgrupacion dimension, FiltroResultados? filtro = null)
        {
            Func<Voto, string> clave = dimension switch
            {
                DimensionAgrupacion.Grupo => v => v.Grupo,
                DimensionAgrupacion.Carrera => v => v.Carrera,
                _ => v => v.CentroUniversitario
            };

            return VotosFiltrados(filtro)
                .GroupBy(v => clave(v).Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key, StringComparer.CurrentCultureIgnoreCase)
                .Select(g => new ResultadoGrupo { Clave = g.Key, Convocatorias = Contar(g.ToList()) })
                .ToList();
        }

        // ---------- Participacion y abstencionismo ----------
        /// <summary>
        /// Participacion general: alumnos distintos que votaron en al menos una convocatoria.
        /// padron = total de alumnos que podian votar (del filtro aplicado, si hay filtro).
        /// </summary>
        public Estadisticas ParticipacionGeneral(int padron, FiltroResultados? filtro = null)
        {
            int votantes = VotosFiltrados(filtro)
                .Select(v => v.AlumnoId.Trim().ToUpperInvariant())
                .Distinct()
                .Count();
            return CrearEstadisticas(NombreGeneral, votantes, padron);
        }

        /// <summary>Participacion y abstencionismo de cada convocatoria.</summary>
        public List<Estadisticas> ParticipacionPorConvocatoria(int padron, FiltroResultados? filtro = null) =>
            VotosFiltrados(filtro)
                .GroupBy(v => v.Convocatoria.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key, StringComparer.CurrentCultureIgnoreCase)
                .Select(g => CrearEstadisticas(
                    g.Key,
                    g.Select(v => v.AlumnoId.Trim().ToUpperInvariant()).Distinct().Count(),
                    padron))
                .ToList();

        // ---------- Internos ----------
        private List<Voto> VotosFiltrados(FiltroResultados? filtro) =>
            _fuente().Where(v => filtro == null || filtro.Coincide(v)).ToList();

        private List<string> Distintos(Func<Voto, string> selector) =>
            _fuente()
                .Select(selector)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

        private static List<ResultadoConvocatoria> Contar(List<Voto> votos) =>
            votos
                .GroupBy(v => v.Convocatoria.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key, StringComparer.CurrentCultureIgnoreCase)
                .Select(conv =>
                {
                    int total = conv.Count();
                    return new ResultadoConvocatoria
                    {
                        Convocatoria = conv.Key,
                        TotalVotos = total,
                        // Mismo nombre con distinta mayuscula (ej. "juan perez") cuenta como un solo candidato.
                        Candidatos = conv
                            .GroupBy(v => v.Candidato.Trim(), StringComparer.OrdinalIgnoreCase)
                            .Select(c => new ResultadoCandidato
                            {
                                Convocatoria = conv.Key,
                                Candidato = c.Key,
                                EsWriteIn = c.Any(v => v.EsWriteIn),
                                Votos = c.Count(),
                                Porcentaje = Porcentaje(c.Count(), total)
                            })
                            .OrderByDescending(r => r.Votos)
                            .ThenBy(r => r.Candidato, StringComparer.CurrentCultureIgnoreCase)
                            .ToList()
                    };
                })
                .ToList();

        private static Estadisticas CrearEstadisticas(string nombre, int votantes, int padron)
        {
            if (padron <= 0)
                throw new ArgumentOutOfRangeException(nameof(padron), "El padron debe ser mayor a 0.");

            double participacion = Porcentaje(votantes, padron);
            return new Estadisticas
            {
                Convocatoria = nombre,
                Votantes = votantes,
                Padron = padron,
                Abstenciones = Math.Max(0, padron - votantes),
                PorcentajeParticipacion = participacion,
                PorcentajeAbstencionismo = Math.Max(0, Math.Round(100 - participacion, 2))
            };
        }

        private static double Porcentaje(int parte, int total) =>
            total == 0 ? 0 : Math.Round(parte * 100.0 / total, 2);
    }
}

namespace SDVE
{
    /// <summary>Criterio por el cual se agrupan resultados.</summary>
    public enum DimensionAgrupacion
    {
        Grupo,
        Carrera,
        CentroUniversitario
    }

    /// <summary>
    /// Filtro opcional. Una propiedad nula o vacia significa "sin filtrar por eso".
    /// Se pueden combinar (ej. Carrera + Centro).
    /// </summary>
    public class FiltroResultados
    {
        public string? Grupo { get; set; }
        public string? Carrera { get; set; }
        public string? CentroUniversitario { get; set; }

        public bool Coincide(Voto v) =>
            Igual(Grupo, v.Grupo) && Igual(Carrera, v.Carrera) && Igual(CentroUniversitario, v.CentroUniversitario);

        private static bool Igual(string? filtro, string valor) =>
            string.IsNullOrWhiteSpace(filtro) ||
            string.Equals(filtro.Trim(), valor.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Votos de un candidato dentro de una convocatoria.</summary>
    public class ResultadoCandidato
    {
        public string Convocatoria { get; set; } = "";
        public string Candidato { get; set; } = "";
        /// <summary>true si es candidato no registrado (write-in).</summary>
        public bool EsWriteIn { get; set; }
        /// <summary>Votos absolutos.</summary>
        public int Votos { get; set; }
        /// <summary>Porcentaje sobre los votos de esa convocatoria (0 a 100, 2 decimales).</summary>
        public double Porcentaje { get; set; }
    }

    /// <summary>Resultados completos de una convocatoria (candidatos ordenados de mayor a menor).</summary>
    public class ResultadoConvocatoria
    {
        public string Convocatoria { get; set; } = "";
        public int TotalVotos { get; set; }
        public List<ResultadoCandidato> Candidatos { get; set; } = new();
    }

    /// <summary>Resultados de un grupo, carrera o centro (segun la dimension usada).</summary>
    public class ResultadoGrupo
    {
        /// <summary>Nombre del grupo / carrera / centro.</summary>
        public string Clave { get; set; } = "";
        public List<ResultadoConvocatoria> Convocatorias { get; set; } = new();
    }
}

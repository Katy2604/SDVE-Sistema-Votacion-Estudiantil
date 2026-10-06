namespace SDVE
{
    /// <summary>
    /// Un voto en UNA convocatoria. Incluye grupo, carrera y centro del alumno
    /// para que el modulo de resultados pueda agrupar sin consultar otra clase.
    /// </summary>
    public class Voto
    {
        public string AlumnoId { get; set; } = "";
        public string Grupo { get; set; } = "";
        public string Carrera { get; set; } = "";
        public string CentroUniversitario { get; set; } = "";
        public string Convocatoria { get; set; } = "";
        public string Candidato { get; set; } = "";
        /// <summary>true si es un candidato no registrado (write-in).</summary>
        public bool EsWriteIn { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
    }
}

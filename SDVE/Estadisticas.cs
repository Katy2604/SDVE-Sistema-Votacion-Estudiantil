namespace SDVE
{
    /// <summary>Participacion y abstencionismo (de una convocatoria o general).</summary>
    public class Estadisticas
    {
        /// <summary>Nombre de la convocatoria, o "General" si cuenta alumnos que votaron en cualquiera.</summary>
        public string Convocatoria { get; set; } = "";
        /// <summary>Alumnos distintos que votaron.</summary>
        public int Votantes { get; set; }
        /// <summary>Total de alumnos que podian votar (lo indica quien llama).</summary>
        public int Padron { get; set; }
        /// <summary>Alumnos que no votaron.</summary>
        public int Abstenciones { get; set; }
        public double PorcentajeParticipacion { get; set; }
        public double PorcentajeAbstencionismo { get; set; }
    }
}

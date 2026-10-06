namespace SDVE
{
    /// <summary>Datos del alumno que emite el voto.</summary>
    public class Alumno
    {
        public string Id { get; set; } = "";
        public string Grupo { get; set; } = "";
        public string Carrera { get; set; } = "";
        public string CentroUniversitario { get; set; } = "";

        /// <summary>Valida los datos capturados. Devuelve true si todo es correcto.</summary>
        public bool Validar(out string error)
        {
            Id = Id.Trim();
            Grupo = Grupo.Trim();
            Carrera = Carrera.Trim();
            CentroUniversitario = CentroUniversitario.Trim();

            if (Id.Length == 0) { error = "Ingresa el ID del alumno."; return false; }
            if (Id.Length > 20 || !Id.All(char.IsLetterOrDigit))
            {
                error = "El ID solo puede contener letras y numeros (maximo 20 caracteres).";
                return false;
            }
            if (Grupo.Length == 0) { error = "Ingresa el grupo."; return false; }
            if (Carrera.Length == 0) { error = "Ingresa la carrera."; return false; }
            if (CentroUniversitario.Length == 0) { error = "Ingresa el centro universitario."; return false; }

            error = "";
            return true;
        }
    }
}

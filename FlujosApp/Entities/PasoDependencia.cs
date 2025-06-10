using System.Text.Json.Serialization;

namespace FlujosApp.Entities
{
    public class PasoDependencia
    {
        public int PasoId { get; set; }

        [JsonIgnore]
        public Paso? Paso { get; set; }

        public int DependeDePasoId { get; set; }

        [JsonIgnore]
        public Paso? DependeDePaso { get; set; }
    }
}

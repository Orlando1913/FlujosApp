namespace FlujosApp.Entities
{
    public class PasoDependencia
    {
        public int PasoId { get; set; }
        public Paso Paso { get; set; } = null!;

        public int DependeDePasoId { get; set; }
        public Paso DependeDePaso { get; set; } = null!;
    }
}

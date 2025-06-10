namespace FlujosApp.Entities
{
    public class Paso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Orden { get; set; }

        public int FlujoId { get; set; }
        public Flujo? Flujo { get; set; }

        public ICollection<Campo> Campos { get; set; } = new List<Campo>();
        public ICollection<PasoDependencia> Dependencias { get; set; } = new List<PasoDependencia>();
        public ICollection<PasoDependencia> DependeDeEste { get; set; } = new List<PasoDependencia>();
    }
}

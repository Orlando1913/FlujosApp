namespace FlujosApp.Entities
{
    public class Flujo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Paso> Pasos { get; set; } = new List<Paso>();
    }
}

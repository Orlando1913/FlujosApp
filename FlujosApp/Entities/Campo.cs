namespace FlujosApp.Entities
{
    public class Campo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool YaProcesado { get; set; }

        public int PasoId { get; set; }
        public Paso? Paso { get; set; }
    }
}

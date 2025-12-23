namespace SiteCrestBackend.Models
{
    // EF model for Chemical
    public class Chemical
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<ChemicalPathway> Values { get; set; } = new List<ChemicalPathway>();
    }
}

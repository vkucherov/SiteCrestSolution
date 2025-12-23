namespace SiteCrestBackend.Models
{
    // Represents the association between a Chemical and a Pathway with a specific value.
    // This is a join entity in a many-to-many relationship with additional data (Value).
    public class ChemicalPathway
    {
        public int ChemicalId { get; set; }

        public int PathwayId { get; set; }
        public Pathway Pathway { get; set; } = null!;

        public double Value { get; set; }

    }
}

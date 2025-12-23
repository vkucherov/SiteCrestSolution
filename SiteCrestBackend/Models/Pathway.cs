namespace SiteCrestBackend.Models
{
    // EF model for Pathway. Support different soil texture options
    public class Pathway
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SoilTexture Soil {  get; set; }

        public int SortValue { get; set; }

        public string DisplayName => $"{Name} ({Soil})";

    }

    public enum SoilTexture
    {
        NA, Fine, Coarse
    }

    
}

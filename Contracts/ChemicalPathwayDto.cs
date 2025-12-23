namespace Contracts
{
    public class ChemicalPathwayDto
    {
        public int PathwayId { get; set; }
        public string? PathwayName { get; set; }
        public double Value { get; set; }
    }

    public class EditChemicalValuesDto
    {
        public int ChemicalId { get; set; }
        public string? ChemicalName { get; set; } 
        public List<ChemicalPathwayDto> Values { get; set; } = new();
    }

    public class CreateChemicalPathwayValueDto
    {
        public int ChemicalId { get; set; }
        public int PathwayId { get; set; }
        public double Value { get; set; }
    }
}

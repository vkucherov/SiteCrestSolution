using Contracts;

namespace SiteCrestFrontend
{
    public class Calculator
    {
        public int ChemicalId { get; set; }
        public double MeasuredValue { get; set; }
        public List<int> SelectedPathwayIds { get; set; } = new();
        public EditChemicalValuesDto? GuidelineData { get; set; }
    }
}

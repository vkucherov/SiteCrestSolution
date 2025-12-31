namespace Contracts
{
    public class PathwayDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public SoilTextureDto Soil { get; set; }

        public int SortValue { get; set; }

        protected string? _displayName;
        public string DisplayName
        {
            set { _displayName = value; }
            get { return (_displayName == null) ? $"{Name} ({Soil})" : _displayName; }
        }

    }

    public enum SoilTextureDto
    {
        NA, Fine, Coarse
    }
}

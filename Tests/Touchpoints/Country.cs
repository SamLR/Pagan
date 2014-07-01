using Pagan.Relationships;

namespace Pagan.Tests.Touchpoints
{
    [UseAsTableName]
    public class Country
    {
        public Table Countries { get; set; }

        [DbName("CountryId")]
        public Column Id { get; set; }

        [DbName("CountryName")]
        public Column Name { get; set; }

        public Column RegionId { get; set; }
        public WithOne<Region> Region { get; set; }
    }
}
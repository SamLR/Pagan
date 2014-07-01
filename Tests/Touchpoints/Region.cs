using Pagan.Queries;
using Pagan.Relationships;

namespace Pagan.Tests.Touchpoints
{
    [UseAsTableName]
    public class Region
    {
        public Table Regions { get; set; }

        [DbName("RegionId")]
        public Column Id { get; set; }

        [DbName("RegionName")]
        public Column Name { get; set; }

        public HasMany<Country> Countries { get; set; }

        #region Queries

        public Query Find(int regionId)
        {
            return Regions.Where(Id == regionId);
        }

        public Query FindWithCountries(int regionId)
        {
            return Regions.Where(Id == regionId) + Countries.Query();
        }

        public Query All()
        {
            return Regions;
        }

        public Query AllWithCountries()
        {
            return Regions + Countries.Query();
        }

        #endregion
    }
}

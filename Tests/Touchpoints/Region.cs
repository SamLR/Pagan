using Pagan.Commands;
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
            return Regions.Where(Id == regionId) + Countries;
        }

        public Query All()
        {
            return Regions;
        }

        public Query AllWithCountries()
        {
            return Regions + Countries;
        }

        #endregion

        #region Commands

        public Command Insert(int id, string name)
        {
            return Regions.Insert(Id.Is(id), Name.Is(name));
        }

        public Command Update(int id, string name)
        {
            return Regions.Update(Id.Is(id), Name.Is(name));
        }

        public Command Delete(int id)
        {
            return Regions.Delete(Id.Is(id));
        }

        #endregion
    }
}

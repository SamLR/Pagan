using System;
using NUnit.Framework;
using Pagan.Tests.Touchpoints;

namespace Pagan.Tests
{
    [TestFixture]
    public class StraightThru
    {
        [Test]
        public void RegionsAndCountries()
        {
            using (var db = new Database("Touchpoints"))
            {
                var regions = db.Query<Region>(x => x.AllWithCountries());
                foreach (var region in regions)
                {
                    Console.WriteLine("Id: {0}\tName: {1}", region.Id, region.Name);
                    Console.WriteLine("Countries:");
                    foreach(var country in region.Countries)
                        Console.WriteLine("\t\tId: {0}\tName: {1}", country.Id, country.Name);
                }
            }
        }

        [Test]
        public void AllRegions()
        {
            using (var db = new Database("Touchpoints"))
            {
                var regions = db.Query<Region>(x => x.All());
                foreach (var region in regions)
                {
                    Console.WriteLine("Id: {0}\tName: {1}", region.Id, region.Name);
                }
            }
        }

        [Test]
        public void UpdateRegion()
        {
            using (var db = new Database("Touchpoints"))
            {
                db.GetDbCommand(db.GetPaganCommand<Region>(x => x.Update(1, "The Moon")));
            }
        }

        [Test]
        public void InsertRegion()
        {
            using (var db = new Database("Touchpoints"))
            {
                db.GetDbCommand(db.GetPaganCommand<Region>(x => x.Insert(1, "The Moon")));
            }
        }

        [Test]
        public void DeleteRegion()
        {
            using (var db = new Database("Touchpoints"))
            {
                db.GetDbCommand(db.GetPaganCommand<Region>(x => x.Delete(1)));
            }
        }
    }
}

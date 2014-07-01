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
    }
}

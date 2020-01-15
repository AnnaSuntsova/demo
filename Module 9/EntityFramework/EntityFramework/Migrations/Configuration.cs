namespace EFModel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFModel.NorthwindDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EFModel.NorthwindDB context)
        {
            context.Categories.AddOrUpdate(c => c.CategoryName,
                new Category
                {
                    CategoryName = "Vehicles"
                },
                new Category
                {
                    CategoryName = "Fastfood"
                }
                );

            context.Regions.AddOrUpdate(r => r.RegionID,
                new Region
                {
                    RegionDescription = "Izhevsk", RegionID = 18
                }
                );

            context.Territories.AddOrUpdate(t => t.TerritoryID,
                new Territory
                {
                    TerritoryID = "12345",
                    TerritoryDescription = "Red square",
                    RegionID = 18
                },
                new Territory
                {
                    TerritoryID = "343241232",
                    TerritoryDescription = "Theatre",
                    RegionID = 18
                }
                );
        }
    }
}

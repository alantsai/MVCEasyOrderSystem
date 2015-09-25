namespace MvcEasyOrderSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcEasyOrderSystem.Models.EOSystemContex>
    {
        public Configuration()
        {
            //把自動更新資料庫打開。如果增加的Model在資料庫對應的Table已經存在會出現問題，
            //這時候記得執行 Add-Migration 然後把裏面的UP()和Down()清空
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MvcEasyOrderSystem.Models.EOSystemContex context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

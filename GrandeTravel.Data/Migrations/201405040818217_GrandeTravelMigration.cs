namespace GrandeTravel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrandeTravelMigration : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Packages", "Name", unique: true, name: "PackageNameIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Packages", "PackageNameIndex");
        }
    }
}

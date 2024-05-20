namespace ElectricityDefect.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteFasaFromIncident : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.INCIDENTs", "FASA");
        }
        
        public override void Down()
        {
            AddColumn("dbo.INCIDENTs", "FASA", c => c.Short(nullable: false));
        }
    }
}

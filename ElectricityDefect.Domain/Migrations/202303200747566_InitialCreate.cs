namespace ElectricityDefect.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.INCIDENTs",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        N_SH = c.Long(nullable: false),
                        FASA = c.Short(nullable: false),
                        BEGIN = c.DateTime(nullable: false),
                        END = c.DateTime(nullable: false),
                        COMMENT = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ELECTRICITYLIMITS",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        N_SH = c.Long(nullable: false),
                        FASA = c.Short(nullable: false),
                        UMIN = c.Double(),
                        UMAX = c.Double(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ELECTRICITYLIMITS");
            DropTable("dbo.INCIDENTs");
        }
    }
}

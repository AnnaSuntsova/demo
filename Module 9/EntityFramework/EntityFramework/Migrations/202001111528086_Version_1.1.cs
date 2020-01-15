namespace EFModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerCards",
                c => new
                {
                    CardNumber = c.Int(nullable: false, identity: true),
                    EmployeeID = c.Int(nullable: false),
                    ExpirationDate = c.DateTime(),
                    CardHolder = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.CardNumber)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerCards", "EmployeeID", "dbo.Employees");
            DropTable("dbo.CustomerCards");
        }
    }
}

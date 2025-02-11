namespace AlShifa_Task.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceItemId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.InvoiceItemId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        VendorId = c.Int(nullable: false),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Contact = c.String(),
                    })
                .PrimaryKey(t => t.VendorId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityInStock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Invoices", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropIndex("dbo.Invoices", new[] { "VendorId" });
            DropIndex("dbo.InvoiceItems", new[] { "ItemId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropTable("dbo.Items");
            DropTable("dbo.Vendors");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceItems");
        }
    }
}

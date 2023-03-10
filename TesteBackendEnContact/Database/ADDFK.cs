using FluentMigrator;

namespace TesteBackendEnContact.Database
{
    public class ADDFK : Migration
    {
        public override void Up()
        {
            Create.Table("Contact")
          .WithColumn("Id").AsInt32().PrimaryKey().Identity()
          .WithColumn("ContactBookId").AsInt32().NotNullable()
          .WithColumn("CompanyId").AsInt32().Nullable()
          .WithColumn("Name").AsString(50).NotNullable()
          .WithColumn("Phone").AsString(20).Nullable()
          .WithColumn("Email").AsString(50).Nullable()
          .WithColumn("Address").AsString(100).Nullable()
          .ForeignKey("FK_Contact_ContactBook", "ContactBook", "Id")
          .ForeignKey("FK_Contact_Company", "Company", "Id")
      ;

            Create.Table("Company")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ContactBookId").AsInt32().NotNullable()
                .WithColumn("Name").AsString(50).NotNullable()
                .ForeignKey("FK_Company_ContactBook", "ContactBook", "Id")
            ;
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Contact_ContactBook").OnTable("Contact");
            Delete.ForeignKey("FK_Contact_Company").OnTable("Contact");
            Delete.ForeignKey("FK_Company_ContactBook").OnTable("Company");
            Delete.Table("Company");
            Delete.Table("Contact");
            Delete.Table("ContactBook");
        }
    }
}

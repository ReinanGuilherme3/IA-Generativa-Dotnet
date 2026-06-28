using FluentMigrator;

namespace IAGenerativa.Embeddings.Data.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_PRODUCTS, "Cria tabela products")]
public class Version2026062703 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("products")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("category").AsString().NotNullable()
            .WithColumn("summary").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();
    }
}

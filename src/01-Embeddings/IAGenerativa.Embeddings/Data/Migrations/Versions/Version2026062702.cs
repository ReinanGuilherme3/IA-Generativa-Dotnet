using FluentMigrator;

namespace IAGenerativa.Embeddings.Data.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_RECOMMENDATIONS, "Cria tabela recomendations e índice HNSW")]
public class Version2026062702 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("recomendations")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("category").AsString().NotNullable()
            .WithColumn("embedding").AsCustom("vector(1024)").NotNullable();

        Execute.Sql("CREATE INDEX idx_recomendations ON recomendations USING HNSW (embedding vector_l2_ops)");
    }
}

using FluentMigrator;

namespace IAGenerativa.Embeddings.Data.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_VECTOR_EXTENSION, "Habilita extensão pgvector")]
public class Version2026062701 : ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Sql("CREATE EXTENSION IF NOT EXISTS vector");
    }
}

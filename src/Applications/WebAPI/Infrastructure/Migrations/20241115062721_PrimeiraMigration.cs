using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Infrastructure.Migrations
{
    public partial class PrimeiraMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criação da tabela tb_perfil
            migrationBuilder.CreateTable(
                name: "tb_perfil",
                columns: table => new
                {
                    id_perfil = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_perfil", x => x.id_perfil);
                });

            // Criação da tabela tb_usuario
            migrationBuilder.CreateTable(
                name: "tb_usuario",
                columns: table => new
                {
                    id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    cpf = table.Column<string>(type: "text", nullable: false),
                    telefone = table.Column<string>(type: "text", nullable: false),
                    senha = table.Column<string>(type: "text", nullable: false),
                    foto = table.Column<string>(type: "text", nullable: true),
                    tentativas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    bloqueado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    codigo_confirmacao = table.Column<string>(type: "text", nullable: true),
                    dt_expiracao_codigo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id_usuario);
                });

            // Criação da tabela tb_usuario_perfil (tabela de relacionamento)
            migrationBuilder.CreateTable(
                name: "tb_usuario_perfil",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    id_perfil = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario_perfil", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuario_perfil_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "tb_usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuario_perfil_perfil_id_perfil",
                        column: x => x.id_perfil,
                        principalTable: "tb_perfil",
                        principalColumn: "id_perfil",
                        onDelete: ReferentialAction.Cascade);
                });

            // Criação da tabela tb_usuario_pre_cadastro
            migrationBuilder.CreateTable(
                name: "tb_usuario_pre_cadastro",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_perfil = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    cpf = table.Column<string>(maxLength: 11, nullable: false),
                    cadastro_concluido = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_usuario_pre_cadastro", x => x.id);
                });

            // Criando índices
            migrationBuilder.CreateIndex(
                name: "ix_usuario_perfil_id_usuario",
                table: "tb_usuario_perfil",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_perfil_id_perfil",
                table: "tb_usuario_perfil",
                column: "id_perfil");

            migrationBuilder.CreateIndex(
                name: "ix_pre_cadastro_cpf",
                table: "tb_usuario_pre_cadastro",
                column: "cpf");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_email",
                table: "tb_usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuario_cpf",
                table: "tb_usuario",
                column: "cpf",
                unique: true);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover tabelas na ordem inversa de dependência
            migrationBuilder.DropTable(
                name: "tb_usuario_perfil");

            migrationBuilder.DropTable(
                name: "tb_usuario_pre_cadastro");

            migrationBuilder.DropTable(
                name: "tb_usuario");

            migrationBuilder.DropTable(
                name: "tb_perfil");
        }
    }
}

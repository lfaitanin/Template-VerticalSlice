using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_usuario_perfil");

            migrationBuilder.DropTable(
                name: "tb_usuario_pre_cadastro");

            migrationBuilder.DropTable(
                name: "tb_perfil");

            migrationBuilder.DropTable(
                name: "tb_usuario");

            migrationBuilder.CreateTable(
                name: "tb_profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    documentNumber = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    photo = table.Column<string>(type: "text", nullable: true),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    blocked = table.Column<bool>(type: "boolean", nullable: false),
                    confirmationCode = table.Column<string>(type: "text", nullable: true),
                    codeExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_pre_registration",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profileId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    documentNumber = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    registrationCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user_pre_registration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_user_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user_profile", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_user_profile_tb_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "tb_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_user_profile_tb_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_profile_profile_id",
                table: "tb_user_profile",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_profile_user_id",
                table: "tb_user_profile",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_user_pre_registration");

            migrationBuilder.DropTable(
                name: "tb_user_profile");

            migrationBuilder.DropTable(
                name: "tb_profile");

            migrationBuilder.DropTable(
                name: "tb_user");

            migrationBuilder.CreateTable(
                name: "tb_perfil",
                columns: table => new
                {
                    id_perfil = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_perfil", x => x.id_perfil);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuario",
                columns: table => new
                {
                    id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    bloqueado = table.Column<bool>(type: "boolean", nullable: false),
                    codigo_confirmacao = table.Column<string>(type: "text", nullable: true),
                    cpf = table.Column<string>(type: "text", nullable: false),
                    dt_expiracao_codigo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    foto = table.Column<string>(type: "text", nullable: true),
                    nome = table.Column<string>(type: "text", nullable: false),
                    senha = table.Column<string>(type: "text", nullable: false),
                    telefone = table.Column<string>(type: "text", nullable: false),
                    tentativas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuario_pre_cadastro",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cadastro_concluido = table.Column<bool>(type: "boolean", nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    id_perfil = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuario_pre_cadastro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuario_perfil",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    id_perfil = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuario_perfil", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_usuario_perfil_tb_perfil_id_perfil",
                        column: x => x.id_perfil,
                        principalTable: "tb_perfil",
                        principalColumn: "id_perfil",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_usuario_perfil_tb_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "tb_usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuario_perfil_id_perfil",
                table: "tb_usuario_perfil",
                column: "id_perfil");

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuario_perfil_id_usuario",
                table: "tb_usuario_perfil",
                column: "id_usuario");
        }
    }
}

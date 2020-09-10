using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialDBViewer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateOfBirth = table.Column<DateTime>(nullable: false),
                    gender = table.Column<int>(nullable: false),
                    lastVisit = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 255, nullable: false),
                    isOnline = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("userId", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "friends",
                columns: table => new
                {
                    friendId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sendDate = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    fromUserId = table.Column<int>(nullable: false),
                    toUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("friendId", x => x.friendId);
                    table.ForeignKey(
                        name: "FK_friends_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_friends_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    messageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    authorId = table.Column<int>(nullable: false),
                    sendDate = table.Column<DateTime>(nullable: false),
                    text = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("messageId", x => x.messageId);
                    table.ForeignKey(
                        name: "FK_messages_users_authorId",
                        column: x => x.authorId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false),
                    messageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_likes_messages_messageId",
                        column: x => x.messageId,
                        principalTable: "messages",
                        principalColumn: "messageId");
                    table.ForeignKey(
                        name: "FK_likes_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_friends_fromUserId",
                table: "friends",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_friends_toUserId",
                table: "friends",
                column: "toUserId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_messageId",
                table: "likes",
                column: "messageId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_userId",
                table: "likes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_authorId",
                table: "messages",
                column: "authorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friends");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

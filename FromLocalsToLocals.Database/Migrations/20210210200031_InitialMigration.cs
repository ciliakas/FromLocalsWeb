using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FromLocalsToLocals.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    Name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<string>("text", nullable: false),
                    Image = table.Column<byte[]>("bytea", nullable: true),
                    VendorsCount = table.Column<int>("integer", nullable: false),
                    Subscribe = table.Column<bool>("boolean", nullable: false),
                    UserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>("boolean", nullable: false),
                    PasswordHash = table.Column<string>("text", nullable: true),
                    SecurityStamp = table.Column<string>("text", nullable: true),
                    ConcurrencyStamp = table.Column<string>("text", nullable: true),
                    PhoneNumber = table.Column<string>("text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>("boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>("boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>("timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>("boolean", nullable: false),
                    AccessFailedCount = table.Column<int>("integer", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>("text", nullable: false),
                    ClaimType = table.Column<string>("text", nullable: true),
                    ClaimValue = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>("text", nullable: false),
                    ClaimType = table.Column<string>("text", nullable: true),
                    ClaimValue = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider = table.Column<string>("text", nullable: false),
                    ProviderKey = table.Column<string>("text", nullable: false),
                    ProviderDisplayName = table.Column<string>("text", nullable: true),
                    UserId = table.Column<string>("text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId = table.Column<string>("text", nullable: false),
                    RoleId = table.Column<string>("text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId = table.Column<string>("text", nullable: false),
                    LoginProvider = table.Column<string>("text", nullable: false),
                    Name = table.Column<string>("text", nullable: false),
                    Value = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Vendors",
                table => new
                {
                    ID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserID = table.Column<string>("text", nullable: false),
                    Title = table.Column<string>("text", nullable: false),
                    About = table.Column<string>("text", nullable: true),
                    Address = table.Column<string>("text", nullable: false),
                    DateCreated = table.Column<string>("text", nullable: false),
                    Latitude = table.Column<double>("double precision", nullable: false),
                    Longitude = table.Column<double>("double precision", nullable: false),
                    VendorType = table.Column<string>("text", nullable: true),
                    Popularity = table.Column<int>("integer", nullable: false),
                    LastClickDate = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    Image = table.Column<byte[]>("bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.ID);
                    table.ForeignKey(
                        "FK_Vendors_AspNetUsers_UserID",
                        x => x.UserID,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Contacts",
                table => new
                {
                    ID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserRead = table.Column<bool>("boolean", nullable: false),
                    ReceiverRead = table.Column<bool>("boolean", nullable: false),
                    UserID = table.Column<string>("text", nullable: false),
                    ReceiverID = table.Column<int>("integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                    table.ForeignKey(
                        "FK_Contacts_AspNetUsers_UserID",
                        x => x.UserID,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Contacts_Vendors_ReceiverID",
                        x => x.ReceiverID,
                        "Vendors",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Followers",
                table => new
                {
                    UserID = table.Column<string>("text", nullable: false),
                    VendorID = table.Column<int>("integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => new { x.UserID, x.VendorID });
                    table.ForeignKey(
                        "FK_Followers_AspNetUsers_UserID",
                        x => x.UserID,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Followers_Vendors_VendorID",
                        x => x.VendorID,
                        "Vendors",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Posts",
                table => new
                {
                    PostID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VendorID = table.Column<int>("integer", nullable: false),
                    Date = table.Column<string>("text", nullable: true),
                    Text = table.Column<string>("text", nullable: true),
                    Image = table.Column<byte[]>("bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostID);
                    table.ForeignKey(
                        "FK_Posts_Vendors_VendorID",
                        x => x.VendorID,
                        "Vendors",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Reviews",
                table => new
                {
                    ID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VendorID = table.Column<int>("integer", nullable: false),
                    CommentID = table.Column<int>("integer", nullable: false),
                    SenderUsername = table.Column<string>("text", nullable: false),
                    Text = table.Column<string>("text", nullable: false),
                    Stars = table.Column<int>("integer", nullable: false),
                    Date = table.Column<string>("text", nullable: false),
                    Reply = table.Column<string>("text", nullable: true),
                    ReplySender = table.Column<string>("text", nullable: true),
                    ReplyDate = table.Column<string>("text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ID);
                    table.ForeignKey(
                        "FK_Reviews_Vendors_VendorID",
                        x => x.VendorID,
                        "Vendors",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "VendorWorkHours",
                table => new
                {
                    ID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsWorking = table.Column<bool>("boolean", nullable: false),
                    VendorID = table.Column<int>("integer", nullable: false),
                    Day = table.Column<int>("integer", nullable: false),
                    OpenTime = table.Column<TimeSpan>("interval", nullable: false),
                    CloseTime = table.Column<TimeSpan>("interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorWorkHours", x => x.ID);
                    table.ForeignKey(
                        "FK_VendorWorkHours_Vendors_VendorID",
                        x => x.VendorID,
                        "Vendors",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    ID = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Text = table.Column<string>("text", nullable: false),
                    Date = table.Column<string>("text", nullable: true),
                    IsUserSender = table.Column<bool>("boolean", nullable: false),
                    ContactID = table.Column<int>("integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        "FK_Messages_Contacts_ContactID",
                        x => x.ContactID,
                        "Contacts",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Notifications",
                table => new
                {
                    NotiId = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<string>("text", nullable: true),
                    VendorId = table.Column<int>("integer", nullable: false),
                    Url = table.Column<string>("text", nullable: true),
                    CreatedDate = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    NotiBody = table.Column<string>("text", nullable: true),
                    ReviewID = table.Column<int>("integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotiId);
                    table.ForeignKey(
                        "FK_Notifications_Reviews_ReviewID",
                        x => x.ReviewID,
                        "Reviews",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Contacts_ReceiverID",
                "Contacts",
                "ReceiverID");

            migrationBuilder.CreateIndex(
                "IX_Contacts_UserID",
                "Contacts",
                "UserID");

            migrationBuilder.CreateIndex(
                "IX_Followers_VendorID",
                "Followers",
                "VendorID");

            migrationBuilder.CreateIndex(
                "IX_Messages_ContactID",
                "Messages",
                "ContactID");

            migrationBuilder.CreateIndex(
                "IX_Notifications_ReviewID",
                "Notifications",
                "ReviewID");

            migrationBuilder.CreateIndex(
                "IX_Posts_VendorID",
                "Posts",
                "VendorID");

            migrationBuilder.CreateIndex(
                "IX_Reviews_VendorID",
                "Reviews",
                "VendorID");

            migrationBuilder.CreateIndex(
                "IX_Vendors_UserID",
                "Vendors",
                "UserID");

            migrationBuilder.CreateIndex(
                "IX_VendorWorkHours_VendorID",
                "VendorWorkHours",
                "VendorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "Followers");

            migrationBuilder.DropTable(
                "Messages");

            migrationBuilder.DropTable(
                "Notifications");

            migrationBuilder.DropTable(
                "Posts");

            migrationBuilder.DropTable(
                "VendorWorkHours");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "Contacts");

            migrationBuilder.DropTable(
                "Reviews");

            migrationBuilder.DropTable(
                "Vendors");

            migrationBuilder.DropTable(
                "AspNetUsers");
        }
    }
}
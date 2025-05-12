using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillShare.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCollaborationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSkill_AspNetUsers_TeachersId",
                table: "ApplicationUserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSkill_Skills_TeachingSkillsId",
                table: "ApplicationUserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSkill1_AspNetUsers_LearnersId",
                table: "ApplicationUserSkill1");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserSkill1_Skills_LearningSkillsId",
                table: "ApplicationUserSkill1");

            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_AspNetUsers_ReceiverId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_AspNetUsers_RequesterId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Collaborations_CollaborationId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Collaborations_CollaborationId1",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CollaborationId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CollaborationId1",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserSkill1",
                table: "ApplicationUserSkill1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserSkill",
                table: "ApplicationUserSkill");

            migrationBuilder.DropColumn(
                name: "CollaborationId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CollaborationId1",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "ApplicationUserSkill1",
                newName: "UserLearningSkills");

            migrationBuilder.RenameTable(
                name: "ApplicationUserSkill",
                newName: "UserTeachingSkills");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Collaborations",
                newName: "CompletedAt");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "Collaborations",
                newName: "TargetId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Collaborations",
                newName: "InitiatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Collaborations_RequesterId",
                table: "Collaborations",
                newName: "IX_Collaborations_TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Collaborations_ReceiverId",
                table: "Collaborations",
                newName: "IX_Collaborations_InitiatorId");

            migrationBuilder.RenameColumn(
                name: "LearnersId",
                table: "UserLearningSkills",
                newName: "ApplicationUser1Id");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserSkill1_LearningSkillsId",
                table: "UserLearningSkills",
                newName: "IX_UserLearningSkills_LearningSkillsId");

            migrationBuilder.RenameColumn(
                name: "TeachersId",
                table: "UserTeachingSkills",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserSkill_TeachingSkillsId",
                table: "UserTeachingSkills",
                newName: "IX_UserTeachingSkills_TeachingSkillsId");

            migrationBuilder.AddColumn<string>(
                name: "InitiatorFeedback",
                table: "Collaborations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InitiatorRating",
                table: "Collaborations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Collaborations",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetFeedback",
                table: "Collaborations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetRating",
                table: "Collaborations",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLearningSkills",
                table: "UserLearningSkills",
                columns: new[] { "ApplicationUser1Id", "LearningSkillsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTeachingSkills",
                table: "UserTeachingSkills",
                columns: new[] { "ApplicationUserId", "TeachingSkillsId" });

            migrationBuilder.CreateTable(
                name: "CollaborationInitiatorLearningSkills",
                columns: table => new
                {
                    Collaboration1Id = table.Column<int>(type: "int", nullable: false),
                    InitiatorLearningSkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationInitiatorLearningSkills", x => new { x.Collaboration1Id, x.InitiatorLearningSkillsId });
                    table.ForeignKey(
                        name: "FK_CollaborationInitiatorLearningSkills_Collaborations_Collaboration1Id",
                        column: x => x.Collaboration1Id,
                        principalTable: "Collaborations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationInitiatorLearningSkills_Skills_InitiatorLearningSkillsId",
                        column: x => x.InitiatorLearningSkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollaborationInitiatorTeachingSkills",
                columns: table => new
                {
                    CollaborationId = table.Column<int>(type: "int", nullable: false),
                    InitiatorTeachingSkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationInitiatorTeachingSkills", x => new { x.CollaborationId, x.InitiatorTeachingSkillsId });
                    table.ForeignKey(
                        name: "FK_CollaborationInitiatorTeachingSkills_Collaborations_CollaborationId",
                        column: x => x.CollaborationId,
                        principalTable: "Collaborations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationInitiatorTeachingSkills_Skills_InitiatorTeachingSkillsId",
                        column: x => x.InitiatorTeachingSkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationInitiatorLearningSkills_InitiatorLearningSkillsId",
                table: "CollaborationInitiatorLearningSkills",
                column: "InitiatorLearningSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationInitiatorTeachingSkills_InitiatorTeachingSkillsId",
                table: "CollaborationInitiatorTeachingSkills",
                column: "InitiatorTeachingSkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_AspNetUsers_InitiatorId",
                table: "Collaborations",
                column: "InitiatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_AspNetUsers_TargetId",
                table: "Collaborations",
                column: "TargetId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLearningSkills_AspNetUsers_ApplicationUser1Id",
                table: "UserLearningSkills",
                column: "ApplicationUser1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLearningSkills_Skills_LearningSkillsId",
                table: "UserLearningSkills",
                column: "LearningSkillsId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeachingSkills_AspNetUsers_ApplicationUserId",
                table: "UserTeachingSkills",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeachingSkills_Skills_TeachingSkillsId",
                table: "UserTeachingSkills",
                column: "TeachingSkillsId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_AspNetUsers_InitiatorId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_AspNetUsers_TargetId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLearningSkills_AspNetUsers_ApplicationUser1Id",
                table: "UserLearningSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLearningSkills_Skills_LearningSkillsId",
                table: "UserLearningSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeachingSkills_AspNetUsers_ApplicationUserId",
                table: "UserTeachingSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeachingSkills_Skills_TeachingSkillsId",
                table: "UserTeachingSkills");

            migrationBuilder.DropTable(
                name: "CollaborationInitiatorLearningSkills");

            migrationBuilder.DropTable(
                name: "CollaborationInitiatorTeachingSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTeachingSkills",
                table: "UserTeachingSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLearningSkills",
                table: "UserLearningSkills");

            migrationBuilder.DropColumn(
                name: "InitiatorFeedback",
                table: "Collaborations");

            migrationBuilder.DropColumn(
                name: "InitiatorRating",
                table: "Collaborations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Collaborations");

            migrationBuilder.DropColumn(
                name: "TargetFeedback",
                table: "Collaborations");

            migrationBuilder.DropColumn(
                name: "TargetRating",
                table: "Collaborations");

            migrationBuilder.RenameTable(
                name: "UserTeachingSkills",
                newName: "ApplicationUserSkill");

            migrationBuilder.RenameTable(
                name: "UserLearningSkills",
                newName: "ApplicationUserSkill1");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "Collaborations",
                newName: "RequesterId");

            migrationBuilder.RenameColumn(
                name: "InitiatorId",
                table: "Collaborations",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "Collaborations",
                newName: "UpdatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Collaborations_TargetId",
                table: "Collaborations",
                newName: "IX_Collaborations_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Collaborations_InitiatorId",
                table: "Collaborations",
                newName: "IX_Collaborations_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "ApplicationUserSkill",
                newName: "TeachersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTeachingSkills_TeachingSkillsId",
                table: "ApplicationUserSkill",
                newName: "IX_ApplicationUserSkill_TeachingSkillsId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUser1Id",
                table: "ApplicationUserSkill1",
                newName: "LearnersId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLearningSkills_LearningSkillsId",
                table: "ApplicationUserSkill1",
                newName: "IX_ApplicationUserSkill1_LearningSkillsId");

            migrationBuilder.AddColumn<int>(
                name: "CollaborationId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollaborationId1",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserSkill",
                table: "ApplicationUserSkill",
                columns: new[] { "TeachersId", "TeachingSkillsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserSkill1",
                table: "ApplicationUserSkill1",
                columns: new[] { "LearnersId", "LearningSkillsId" });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CollaborationId",
                table: "Skills",
                column: "CollaborationId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CollaborationId1",
                table: "Skills",
                column: "CollaborationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSkill_AspNetUsers_TeachersId",
                table: "ApplicationUserSkill",
                column: "TeachersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSkill_Skills_TeachingSkillsId",
                table: "ApplicationUserSkill",
                column: "TeachingSkillsId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSkill1_AspNetUsers_LearnersId",
                table: "ApplicationUserSkill1",
                column: "LearnersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserSkill1_Skills_LearningSkillsId",
                table: "ApplicationUserSkill1",
                column: "LearningSkillsId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_AspNetUsers_ReceiverId",
                table: "Collaborations",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_AspNetUsers_RequesterId",
                table: "Collaborations",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Collaborations_CollaborationId",
                table: "Skills",
                column: "CollaborationId",
                principalTable: "Collaborations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Collaborations_CollaborationId1",
                table: "Skills",
                column: "CollaborationId1",
                principalTable: "Collaborations",
                principalColumn: "Id");
        }
    }
}

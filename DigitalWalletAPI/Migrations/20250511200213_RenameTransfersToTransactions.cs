using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameTransfersToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_ReceiverWalletId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_SenderWalletId",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers");

            migrationBuilder.RenameTable(
                name: "Transfers",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_SenderWalletId",
                table: "Transactions",
                newName: "IX_Transactions_SenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_ReceiverWalletId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transfers");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderWalletId",
                table: "Transfers",
                newName: "IX_Transfers_SenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transfers",
                newName: "IX_Transfers_ReceiverWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_ReceiverWalletId",
                table: "Transfers",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_SenderWalletId",
                table: "Transfers",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

# Mini Account Management System
Mini-AMS is a simple, role-based account management system built with ASP.NET Core Razor Pages and SQL Server.

## Key Features
- Role-based Authentication: Admin, Accountant, and Viewer roles with custom permissions.
- User & Role Management: Register, login, and assign roles to users.
- Chart of Accounts: Create, update, delete, and view accounts in a parent/child hierarchy.
- Voucher Entry: Enter Journal, Payment, and Receipt vouchers with multi-line debit/credit entries.
- Stored Procedures Only: All database operations use stored procedures (no LINQ).

# Getting Started
### 1. Clone the repository:
`git clone https://github.com/jaforiq/Mini-AMS.git
cd Mini-AMS-Qtec`

### 2. Configure the database:
Update the DefaultConnection string in Mini-AMS/appsettings.json to point to your SQL Server instance.

### 3. Apply database migrations:
`dotnet ef database update`

### 4. Create required stored procedures and table types:
Run the SQL scripts in the Scripts/ folder (e.g., sp_ManageChartOfAccounts.sql, sp_SaveVoucher.sql) in your SQL Server database.

### 5. Run the application:
`dotnet run --project Mini-AMS`

### 6. Access the app:
Open your browser and go to https://localhost:<port>
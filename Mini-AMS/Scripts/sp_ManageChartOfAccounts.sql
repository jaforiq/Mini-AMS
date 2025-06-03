
CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10), 
    @Id INT = NULL,
    @Name NVARCHAR(100) = NULL,
    @ParentId INT = NULL,
    @Type NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'CREATE'
    BEGIN
        INSERT INTO ChartOfAccounts (Name, ParentId, [Type])
        VALUES (@Name, @ParentId, @Type);
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE ChartOfAccounts
        SET Name = @Name, ParentId = @ParentId, [Type] = @Type
        WHERE Id = @Id;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM ChartOfAccounts WHERE Id = @Id;
    END
END

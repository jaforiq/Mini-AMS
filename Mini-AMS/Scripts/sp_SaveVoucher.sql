CREATE TYPE dbo.VoucherLineType AS TABLE
(
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2),
    Narration NVARCHAR(255)
);


CREATE PROCEDURE sp_SaveVoucher
    @Date DATE,
    @ReferenceNo NVARCHAR(50),
    @VoucherType NVARCHAR(20),
    @Lines dbo.VoucherLineType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    
    INSERT INTO VoucherHeaders ([Date], ReferenceNo, VoucherType)
    VALUES (@Date, @ReferenceNo, @VoucherType);

    DECLARE @VoucherHeaderId INT = SCOPE_IDENTITY();

    
    INSERT INTO VoucherLines (VoucherHeaderId, AccountId, Debit, Credit, Narration)
    SELECT
        @VoucherHeaderId,
        AccountId,
        Debit,
        Credit,
        Narration
    FROM @Lines;
END

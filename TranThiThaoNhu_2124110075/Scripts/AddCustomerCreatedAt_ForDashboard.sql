/* Chay mot lan tren database Daily Fresh.
   Khach hang da ton tai truoc khi bo sung CreatedAt duoc de NULL vi khong co lich su ngay dang ky.
   Khach hang tao moi tu nay se duoc CustomersController gan CreatedAt = DateTime.Now.
*/
IF COL_LENGTH('dbo.Customers', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Customers ADD CreatedAt datetime2 NULL;
END
ELSE
BEGIN
    DECLARE @DefaultConstraintName sysname;
    SELECT @DefaultConstraintName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t ON t.object_id = c.object_id
    INNER JOIN sys.schemas sc ON sc.schema_id = t.schema_id
    WHERE sc.name = 'dbo' AND t.name = 'Customers' AND c.name = 'CreatedAt';

    IF @DefaultConstraintName IS NOT NULL
    BEGIN
        EXEC('ALTER TABLE dbo.Customers DROP CONSTRAINT [' + @DefaultConstraintName + ']');
        -- Cac ban ghi da ton tai bi gan nham ngay hien tai boi script cu: xoa ngay gia.
        UPDATE dbo.Customers SET CreatedAt = NULL;
    END

    ALTER TABLE dbo.Customers ALTER COLUMN CreatedAt datetime2 NULL;
END;

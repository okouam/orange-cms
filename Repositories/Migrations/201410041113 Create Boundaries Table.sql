CREATE TABLE Boundaries (
	[Id] BIGINT IDENTITY(1,1), 
	[Name] NVARCHAR(200) NOT NULL,
	[Shape] GEOGRAPHY,
	CONSTRAINT PK_Boundaries PRIMARY KEY ([Id])
)

GO 

CREATE SPATIAL INDEX idx_Boundaries_Shape 
  ON Boundaries (Shape)

GO

CREATE PROC [dbo].[InsertBoundary] 
    @Shape GEOGRAPHY, 
    @Name NVARCHAR(200)
AS 
    INSERT INTO [Boundaries]
		([Shape], [Name]) 
    VALUES
		(@Shape, @Name);

	DECLARE @Id AS BIGINT;
	SET @Id = SCOPE_IDENTITY();

	DELETE FROM dbo.BoundaryCustomer WHERE BoundaryId = @Id;
		
	INSERT INTO dbo.BoundaryCustomer 
		(CustomerId, BoundaryId)
	SELECT 
		Customers.Id, Boundaries.Id 
		FROM dbo.Customers 
		JOIN dbo.Boundaries 
			ON Customers.Coordinates.STIntersects(Boundaries.Shape) = 0
	WHERE Boundaries.Id = @Id;	

	SELECT @Id AS Id 

GO

CREATE PROC [dbo].[DeleteBoundary] 
    @Id BIGINT
AS 
	DELETE FROM [BoundaryCustomer] WHERE BoundaryId = @Id;
	DELETE FROM [Boundaries] WHERE Id = @Id;

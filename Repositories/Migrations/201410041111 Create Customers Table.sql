CREATE TABLE Customers (
	[Id] BIGINT IDENTITY(1,1), 
	[Coordinates] GEOGRAPHY NULL,
	[ImageUrl] NVARCHAR(200) NULL,
	[Telephone] NVARCHAR(200) NOT NULL,
	[Speed] NVARCHAR(200) NULL,
	[ExpiryDate] DATE NULL,
	[NeverExpires] BIT NULL,
	[Name] NVARCHAR(200),
	[Login] NVARCHAR(200),
	[Status] NVARCHAR(200),
	[Formula] NVARCHAR(200) NULL,
	CONSTRAINT PK_Customers PRIMARY KEY ([Id]),
    CONSTRAINT UC_Telephone UNIQUE([Telephone]) 
)

GO

CREATE SPATIAL INDEX idx_Customers_Coordinates
  ON Customers ([Coordinates])
  
GO

CREATE PROC [dbo].[InsertCustomer] 
    @Coordinates GEOGRAPHY, 
    @ImageUrl NVARCHAR(200), 
    @Telephone NVARCHAR(200), 
    @Speed NVARCHAR(200), 
    @ExpiryDate DATE, 
    @NeverExpires BIT, 
    @Name NVARCHAR(200), 
    @Login NVARCHAR(200), 
    @Status NVARCHAR(200), 
    @Formula NVARCHAR(200)
AS 
    INSERT INTO [Customers]
		(Coordinates, ImageUrl, Telephone, Speed, ExpiryDate, [NeverExpires], [Name], [Login], [Status], [Formula]) 
    VALUES
		(@Coordinates, @ImageUrl, @Telephone, @Speed, @ExpiryDate, @NeverExpires, @Name, @Login, @Status, @Formula);

	DECLARE @Id AS BIGINT;
	SET @Id = SCOPE_IDENTITY();

	EXEC [dbo].[UpdateCustomerBoundaries] @Id
	
	SELECT @Id AS Id 

GO

CREATE PROC [dbo].[DeleteCustomer] 
    @Id BIGINT
AS 
	DELETE FROM [BoundaryCustomer] WHERE CustomerId = @Id;
	DELETE FROM [Customers] WHERE Id = @Id;

GO

CREATE PROC [dbo].[UpdateCustomer] 
	@Id BIGINT,
    @Coordinates GEOGRAPHY, 
    @ImageUrl NVARCHAR(200), 
    @Telephone NVARCHAR(200), 
    @Speed NVARCHAR(200), 
    @ExpiryDate DATE, 
    @NeverExpires BIT, 
    @Name NVARCHAR(200), 
    @Login NVARCHAR(200), 
    @Status NVARCHAR(200), 
    @Formula NVARCHAR(200)
AS 
    UPDATE [Customers] 
	SET 
		Coordinates = @Coordinates, 
		ImageUrl = @ImageUrl, 
		Telephone = @Telephone, 
		Speed = @Speed, 
		ExpiryDate = @ExpiryDate, 
		[NeverExpires] = @NeverExpires, 
		[Name] = @Name, 
		[Login] = @Login, 
		[Status] = @Status, 
		[Formula] = @Formula 
	WHERE Id = @Id

	EXEC [dbo].[UpdateCustomerBoundaries] @Id

GO

CREATE PROC [dbo].[UpdateCustomerBoundaries]
    @Id BIGINT
AS

	DELETE FROM dbo.BoundaryCustomer WHERE CustomerId = @Id;

	INSERT INTO dbo.BoundaryCustomer 
		(CustomerId, BoundaryId)
	SELECT 
		Customers.Id, Boundaries.Id 
		FROM dbo.Customers 
		JOIN dbo.Boundaries 
			ON Customers.Coordinates.STIntersects(Boundaries.Shape) = 0
	WHERE Customers.Id = @Id;
 
CREATE TABLE Customers (
	[Id] BIGINT IDENTITY(1,1), 
	[Coordinates] GEOGRAPHY NULL,
	[ImageUrl] VARCHAR(200) NULL,
	[Telephone] VARCHAR(200) NOT NULL,
	[Speed] VARCHAR(200) NULL,
	[ExpiryDate] DATE NULL,
	[NeverExpires] BIT NULL,
	[Name] VARCHAR(200),
	[Login] VARCHAR(200),
	[Status] VARCHAR(200),
	[Formula] VARCHAR(200) NULL,
	CONSTRAINT PK_Customers PRIMARY KEY ([Id]),
    CONSTRAINT UC_Telephone UNIQUE([Telephone]) 
)

GO

CREATE SPATIAL INDEX idx_Customers_Coordinates
  ON Customers ([Coordinates])
  
GO
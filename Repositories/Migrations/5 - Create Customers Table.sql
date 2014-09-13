CREATE TABLE Customers (
	[Id] BIGINT IDENTITY(1,1), 
	[Coordinates] GEOGRAPHY NULL,
	[ImageUrl] VARCHAR(MAX) NULL,
	[Telephone] VARCHAR(200) NOT NULL,
	[Speed] VARCHAR(MAX) NULL,
	[ExpiryDate] DATE NULL,
	[NeverExpires] BIT NULL,
	[Name] VARCHAR(MAX),
	[Login] VARCHAR(MAX),
	[Status] VARCHAR(MAX),
	[Formula] VARCHAR(MAX) NULL,
	CONSTRAINT PK_Customers PRIMARY KEY ([Id]),
    CONSTRAINT UC_Telephone UNIQUE([Telephone]) 
)
CREATE TABLE Customers (
	[Id] BIGINT IDENTITY(1,1), 
	[Name] VARCHAR(MAX) NOT NULL,
	[CreatedById] BIGINT NOT NULL,
	[ClientId] BIGINT NOT NULL,
	[Longitude] DECIMAL NOT NULL,
	[Latitude] DECIMAL NOT NULL,
	[Telephone] VARCHAR(MAX) NOT NULL,
	CONSTRAINT PK_Customers PRIMARY KEY ([Id]),
	CONSTRAINT FK_Customers_Users FOREIGN KEY([CreatedById]) REFERENCES Users([Id]),
	CONSTRAINT FK_Customers_Clients FOREIGN KEY([ClientId]) REFERENCES Clients([Id])
)

CREATE INDEX IDX_Customers_ClientId ON Customers (ClientId)
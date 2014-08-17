CREATE TABLE Categories (
	[Id] BIGINT IDENTITY(1,1), 
	[Name] VARCHAR(MAX) NOT NULL,
	[ClientId] BIGINT NOT NULL,
	CONSTRAINT PK_Categories PRIMARY KEY ([Id]),
	CONSTRAINT FK_Categories_Clients FOREIGN KEY([ClientId]) REFERENCES Clients([Id])
)

CREATE INDEX IDX_Categories_ClientId ON Categories (ClientId)
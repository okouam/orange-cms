﻿CREATE TABLE Clients (
	[Id] BIGINT IDENTITY(1,1), 
	[Name] VARCHAR(MAX) NOT NULL,
	[ContactId] BIGINT NULL,
	CONSTRAINT PK_Clients PRIMARY KEY ([Id]),
	CONSTRAINT FK_Clients_Contacts FOREIGN KEY([ContactId]) REFERENCES Contacts([Id])
)
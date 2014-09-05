﻿CREATE TABLE Users (
	[Id] BIGINT IDENTITY(1,1), 
	[Email] VARCHAR(MAX) NOT NULL,
	[Password] VARCHAR(MAX) NOT NULL,
	[Role] VARCHAR(MAX) NOT NULL,
	[UserName] VARCHAR(MAX) NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY ([Id])
)
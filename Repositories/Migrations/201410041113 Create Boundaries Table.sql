CREATE TABLE Boundaries (
	[Id] BIGINT IDENTITY(1,1), 
	[Name] VARCHAR(200) NOT NULL,
	[Shape] GEOGRAPHY,
	CONSTRAINT PK_Boundaries PRIMARY KEY ([Id])
)

GO 

CREATE SPATIAL INDEX idx_Boundaries_Shape 
  ON Boundaries (Shape)

GO
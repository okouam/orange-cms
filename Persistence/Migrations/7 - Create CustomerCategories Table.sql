CREATE TABLE CustomerCategories (
	[CustomerId] BIGINT, 
	[CategoryId] BIGINT, 
	CONSTRAINT PK_CustomersToCategories PRIMARY KEY ([CustomerId], [CategoryId]),
	CONSTRAINT FK_CustomersToCategories_Customers FOREIGN KEY([CustomerId]) REFERENCES Customers([Id]),
	CONSTRAINT FK_CustomersToCategories_Categories FOREIGN KEY([CategoryId]) REFERENCES Categories([Id])
)
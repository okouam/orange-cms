CREATE TABLE BoundaryCustomer (
	[CustomerId] BIGINT FOREIGN KEY (BoundaryId) REFERENCES Boundaries(Id), 
	[BoundaryId] BIGINT FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
	PRIMARY KEY ([CustomerId], [BoundaryId])	
)

GO

CREATE INDEX IDX_BoundaryCustomer_CustomerId ON BoundaryCustomer(CustomerId, BoundaryId)

GO

CREATE INDEX IDX_BoundaryCustomer_BoundaryId ON BoundaryCustomer(BoundaryId, CustomerId)

GO

CREATE PROC [dbo].[DoNothingCustomerBoundary] 
    @CustomerId BIGINT, 
    @BoundaryId BIGINT
AS 
  -- do nothing, handled when inserting boundaries or customers
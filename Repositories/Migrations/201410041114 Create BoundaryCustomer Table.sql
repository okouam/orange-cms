CREATE TABLE BoundaryCustomer (
	[CustomerId] BIGINT, 
	[BoundaryId] BIGINT,
)

GO

CREATE INDEX IDX_BoundaryCustomer_CustomerId ON BoundaryCustomer(CustomerId, BoundaryId)

GO

CREATE INDEX IDX_BoundaryCustomer_BoundaryId ON BoundaryCustomer(BoundaryId, CustomerId)
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spAddRenter
	@PropertyId INT,
	@NumberOfOccupants INT,
	@LastName Varchar(50),
	@FirstName Varchar(50),
	@PrimaryPhoneNumber Varchar(50),
	@StartDate DateTime,
	@EndDate DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO Renter(PropertyId, NumberOfOccupants, LastName, FirstName, PrimaryPhoneNumber, StartDate, EndDate)
	Values(@PropertyId, @NumberOfOccupants, @LastName, @FirstName, @PrimaryPhoneNumber, @StartDate, @EndDate)
END
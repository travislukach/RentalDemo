-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spAddProperty 
	-- Add the parameters for the stored procedure here
	@name varchar(50),
	@rate money,
	@maxOccupants int,
	@location varchar(50),
	@nonStandardFeat text
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	INSERT INTO Property (PropertyName, DailyRate, MaximumOccupants, Location, NonStandardFeatures)
	VALUES(@name, @rate, @maxOccupants, @location, @nonStandardFeat)
	END TRY
	BEGIN CATCH
		SELECT   
        ERROR_NUMBER() AS ErrorNumber,
		ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

END
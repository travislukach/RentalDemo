-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spSearchPropertiesByRate
	@rate MONEY
AS
BEGIN
	SELECT * FROM Property WHERE DailyRate <= @rate
END
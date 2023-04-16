-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spSearchPropertiesByMaxOccupants]
	@occupants INT
AS
BEGIN
	SELECT * FROM Property WHERE MaximumOccupants >= @occupants
END
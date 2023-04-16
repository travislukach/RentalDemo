-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spSearchPropertiesByName
	@name VARCHAR(50)
AS
BEGIN
	SELECT * FROM Property WHERE PropertyName LIKE '%'+@name+'%'  
END
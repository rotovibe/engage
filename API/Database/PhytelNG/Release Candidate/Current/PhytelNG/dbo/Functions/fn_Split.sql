﻿CREATE FUNCTION [dbo].[fn_Split]
(
	@text varchar(max)
	, @delimiter varchar(20) = ' '
)

RETURNS @Strings TABLE
(    
	position int IDENTITY PRIMARY KEY
	, value varchar(max)   
)

AS

BEGIN

	DECLARE @index int 
	SET @index = -1 

	WHILE (LEN(@text) > 0) 
	BEGIN  

		SET @index = CHARINDEX(@delimiter , @text)  

		IF (@index = 0) AND (LEN(@text) > 0)  
		BEGIN   
			INSERT INTO @Strings(value) 
			VALUES (@text)

			BREAK
		END  

		IF (@index > 1)  
		BEGIN   
			INSERT INTO @Strings(value)
			VALUES (LEFT(@text, @index - 1))   

			SET @text = RIGHT(@text, (LEN(@text) - @index))  
		END  
		ELSE 
			SET @text = RIGHT(@text, (LEN(@text) - @index)) 
	
	END

	RETURN

END

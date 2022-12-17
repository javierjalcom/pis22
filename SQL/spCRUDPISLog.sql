/*
Highlight and execute the following statement to drop the procedure
before executing the create statement.

DROP PROCEDURE spCRUDPISLog
 
*/
CREATE PROCEDURE spCRUDPISLog   @intMode     int
                               ,@intVisit     numeric(16)
                               ,@strMessage  varchar(150)
                               ,@strUser      varchar(18)
                                       
                                       
  
AS  
    BEGIN 

       -- si el modo es consula
          IF ( @intMode =1)
          BEGIN
  		        
		          DECLARE @lintnumevent numeric(16)
		          
		          SET @lintnumevent = 0
		          
		          SELECT @lintnumevent =  MAX(tblclsPISLog.intEventId)
		          FROM tblclsPISLog
		          
		          SELECT @lintnumevent =  ISNULL(@lintnumevent,0)
		           
		          SELECT @lintnumevent = @lintnumevent +1 
		          
		          INSERT INTO tblclsPISLog
		          	(intEventId, intVisitId, strMessage, strUser, dtmCreationStamp)
		          VALUES 
		          	(@lintnumevent,@intVisit , @strMessage, @strUser, GETDATE() )



		          
          END 
    END 
    

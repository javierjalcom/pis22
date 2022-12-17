/*
Highlight and execute the following statement to drop the procedure
before executing the create statement.

DROP PROCEDURE spGetPISVisitInfo

*/
CREATE PROCEDURE spGetPISVisitInfo  @intVisitId          udtIdentifier
                                    --    ,@dtmAppointDate      DATETIME
                                    --    ,@intUserId           numeric
                                    -- ,@strUser             varchar(20)
  
AS  
    BEGIN 
    
		
		DECLARE @intZoneCounter int  -- contador de zona 
	    DECLARE @dtmSingleDate     Datetime
	    DECLARE @dtmStartDateTime  Datetime
	    DECLARE @lintCount int

	    
       -- CREAR UNA TABLA DE HORARIOS
          CREATE TABLE  #VisitResult
		  (                
		    strVisitCodigo        varchar(20),
		    intVisitItem         int null,
		    idTipoOperacion      int null,		    
		    strBL                varchar(20) null,
	        strPedimento         varchar(20) null,
	        idTipo               int null,
   		    intMotivo            int null,	 
   		    idRecintoOrigen      int null,
		    idRecintoDestino     int null,		       		           
   		    strimo               varchar(30) null,		    
   		    strBuque             varchar(30) null,
  		    idSolicitante        numeric null,
		    idAgenteAduanal      numeric null,
		    idTipoProducto       int null,
		    idManiobra           int null,
		    idTipoDespacho       int null,
		    noContenedor         varchar(30) null,
		    idTipoContenedor     varchar(10) null,
		    idEstadoContenedor   int null ,
		    strium                  varchar(30) null,
		    strfechaInicio       varchar(20) null,
		    idHoraInicia         int null,
		    strfechaFin         varchar(20) null,
		    idHoraFinaliza       int null,
		    inttotalTractos      int null,
		    inttotalManiobras    int null,
		    strFechaItem         varchar(20) null,
		    idHoraItem           int null,
		    noManiobras          int null

		  )
    
      -- por default salida de contenedores 
		  INSERT INTO #VisitResult ( 	strVisitCodigo  , intVisitItem ,idTipoOperacion  ,	strBL ,strPedimento  ,
		                               idTipo           ,intMotivo     ,idRecintoOrigen  ,idRecintoDestino ,		       		           
		                               strimo           ,strBuque      ,  idSolicitante  ,idAgenteAduanal  ,
		                               idTipoProducto   ,idManiobra    ,  idTipoDespacho  ,noContenedor   ,
		                               idTipoContenedor ,idEstadoContenedor  ,strium      ,strfechaInicio  ,
		                               idHoraInicia   ,strfechaFin         ,idHoraFinaliza ,inttotalTractos ,
		                               inttotalManiobras   ,strFechaItem   ,idHoraItem    ,noManiobras 
		                            )

		  SELECT
		   CONVERT(VARCHAR(20),tblclsVisitContainer.intVisitId ),tblclsVisitContainer.intVisitItemId , 
		              (
		                CASE WHEN tblclsFiscalMovement.strFiscalMovementIdentifier='IMPO' THEN 1
		                     WHEN tblclsFiscalMovement.strFiscalMovementIdentifier='EXPO' THEN 2
		                 ELSE 0
		                 END 		                
		              ) --idTipoOperacion
			              , --BL
			              (
			                SELECT MAX(tblclsDocument.strDocumentFolio)
			                FROM tblclsContainerInventoryDoc
			                 INNER JOIN tblclsDocumentType ON tblclsDocumentType.intDocumentTypeId = tblclsDocument.intDocumentTypeId
			                 INNER JOIN tblclsDocument ON tblclsDocument.intDocumentId  =tblclsContainerInventoryDoc.intDocumentId
			               WHERE tblclsContainerInventoryDoc.intContainerUniversalId = tblclsContainerInventory.intContainerUniversalId
			               AND tblclsDocumentType.strDocumentTypeIdentifier = 'BL') 
		            		  , -- pedimento
				               (
				                 SELECT MAX(tblclsDocument.strDocumentFolio)
				                 FROM tblclsContainerInventoryDoc
				                   INNER JOIN tblclsDocumentType ON tblclsDocumentType.intDocumentTypeId = tblclsDocument.intDocumentTypeId
				                   INNER JOIN tblclsDocument ON tblclsDocument.intDocumentId  =tblclsContainerInventoryDoc.intDocumentId
				                 WHERE tblclsContainerInventoryDoc.intContainerUniversalId = tblclsContainerInventory.intContainerUniversalId
				                 AND tblclsDocumentType.strDocumentTypeIdentifier = 'PIMPO'
				               )
				     --,idTipo           ,intMotivo     ,idRecintoOrigen  ,idRecintoDestino ,		       		           
				     ,1,   (
				              CASE WHEN ISNULL(tblclsVisit.strDeliveryType,'')='TIT' THEN 2
				               ELSE 1
				               END
				           ) -- intMotivo
				                 , 10971 , 10971 --,idTipo ,intMotivo ,idRecintoOrigen,idRecintoDestino
				     --strimo           ,strBuque      ,  idSolicitante  ,idAgenteAduanal  ,
				     ,'' , tblclsVessel.strVesselName , DELENTY.intPISCompanyId , DELENTY.intPISCompanyId
				    --idTipoProducto   ,idManiobra    ,  idTipoDespacho  ,noContenedor   ,
				    , 1 , 1, 1,tblclsVisitContainer.strContainerId 
		            --idTipoContenedor ,idEstadoContenedor  ,strium      ,strfechaInicio  ,
		            , CONVERT(VARCHAR(5),tblclsContainerISOCode.intPISID), 1 , CONVERT(VARCHAR(10),tblclsVisitContainer.intContainerUniversalId)
		             ,str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  --CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) --
		            --idHoraInicia   ,strfechaFin         ,idHoraFinaliza ,inttotalTractos ,
		            ,	DATEPART(dd, tblclsVisit.dtmAppointmentDate) 
		                   ,  str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  -- CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) 
		                          , DATEPART(dd, tblclsVisit.dtmAppointmentDate), 1
		            --inttotalManiobras   ,strFechaItem   ,idHoraItem    ,noManiobras 
		            ,1 , str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  -- CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) 
		            , DATEPART(dd, tblclsVisit.dtmAppointmentDate), 1
		            
		  from tblclsVisitContainer
		   inner join tblclsVisit on tblclsVisit.intVisitId = tblclsVisitContainer.intVisitId
		   INNER JOIN tblclsContainerInventory ON tblclsContainerInventory.intContainerUniversalId = tblclsVisitContainer.intContainerUniversalId
		   INNER JOIN tblclsContainer ON tblclsContainer.strContainerId =tblclsVisitContainer.strContainerId
		   inner join tblclsService on tblclsService.intServiceId = tblclsVisitContainer.intServiceId
		   
		   INNER JOIN tblclsContainerISOCode ON tblclsContainerISOCode.intContISOCodeId = tblclsContainer.intContISOCodeId
		   INNER JOIN tblclsContainerType ON tblclsContainerType.intContainerTypeId = tblclsContainerISOCode.intContainerTypeId
		   INNER JOIN tblclsContainerSize  ON tblclsContainerSize.intContainerSizeId = tblclsContainerISOCode.intContainerSizeId
		  
		   INNER JOIN tblclsContainerDeliveryDetail  ON  tblclsContainerDeliveryDetail.intContainerUniversalId = tblclsVisitContainer.intContainerUniversalId
		                                             AND  tblclsContainerDeliveryDetail.intVisitId= tblclsVisitContainer.intVisitId
		                                             AND tblclsContainerDeliveryDetail.intContainerDeliveryId = tblclsVisitContainer.intServiceOrderId
		                                             
		   INNER JOIN tblclsContainerDelivery ON    tblclsContainerDelivery.intContainerDeliveryId = tblclsContainerDeliveryDetail.intContainerDeliveryId
		   
		   INNER JOIN tblclsFiscalMovement  on tblclsContainerInventory.intFiscalMovementId = tblclsFiscalMovement.intFiscalMovementId
		   
		   LEFT JOIN tblclsVesselVoyage ON tblclsVesselVoyage.intVesselVoyageId = tblclsContainerInventory.intContainerInvVesselVoyageId
		   LEFT JOIN tblclsVessel ON tblclsVessel.intVesselId = tblclsVesselVoyage.intVesselId
		   LEFT JOIN tblclsCompanyEntity DELENTY ON DELENTY.intCompanyEntityId = tblclsContainerDelivery.intContDelRequiredById
		                                         AND DELENTY.intCustomerTypeId = tblclsContainerDelivery.intContDelRequiredTypeId
		   LEFT JOIN tblclsCompany DELCOMP ON DELCOMP.intCompanyId = DELENTY.intCompanyId
		   
		   LEFT JOIN tblclsCompanyEntity INVENTY ON INVENTY.intCompanyEntityId = tblclsContainerInventory.intContRecepRequiredById
		                                         AND INVENTY.intCustomerTypeId = tblclsContainerInventory.intContRecepRequiredTypeId
		                                         
		   LEFT JOIN tblclsCompany INVCOMP ON INVENTY.intCompanyId = INVCOMP.intCompanyId
		   
		  where tblclsVisit.intVisitId  =@intVisitId
		  
      -------
      -- contar los registros de la tabla temporal
      SELECT @lintCount  =COUNT(#VisitResult.strVisitCodigo)
      FROM #VisitResult
      
      SET @lintCount  = ISNULL(@lintCount,0)
      
       -- si la tabla es vacia ,         -- buscarla como ingreso de carga general
       IF ( @lintCount =0)
       BEGIN -- @lintCount
          
         

				  INSERT INTO #VisitResult ( 	strVisitCodigo  , intVisitItem ,idTipoOperacion  ,	strBL ,strPedimento  ,
				                               idTipo           ,intMotivo     ,idRecintoOrigen  ,idRecintoDestino ,		       		           
				                               strimo           ,strBuque      ,  idSolicitante  ,idAgenteAduanal  ,
				                               idTipoProducto   ,idManiobra    ,  idTipoDespacho  ,noContenedor   ,
				                               idTipoContenedor ,idEstadoContenedor  ,strium      ,strfechaInicio  ,
				                               idHoraInicia   ,strfechaFin         ,idHoraFinaliza ,inttotalTractos ,
				                               inttotalManiobras   ,strFechaItem   ,idHoraItem    ,noManiobras 
				                           )
				                           
				                   
				  SELECT
				   CONVERT(VARCHAR(20),tblclsVisitGeneralCargo.intVisitId ),tblclsVisitGeneralCargo.intServiceOrderDetailId , 
				              (
				                CASE WHEN tblclsFiscalMovement.strFiscalMovementIdentifier='IMPO' THEN 1
				                     WHEN tblclsFiscalMovement.strFiscalMovementIdentifier='EXPO' THEN 2
				                 ELSE 0
				                 END 		                
				              ) --idTipoOperacion
					           , --BL 
					            (
					                SELECT MAX(tblclsDocument.strDocumentFolio)
					                FROM tblclsGCInventoryDocument
					                  INNER JOIN tblclsDocument ON tblclsDocument.intDocumentId  =tblclsGCInventoryDocument.intDocumentId
					                  INNER JOIN tblclsDocumentType ON tblclsDocumentType.intDocumentTypeId = tblclsDocument.intDocumentTypeId
					                WHERE tblclsGCInventoryDocument.intGeneralCargoUniversalId = tblclsGCInventoryDocument.intGeneralCargoUniversalId
					                AND tblclsDocumentType.strDocumentTypeIdentifier = 'BL') 
				            		  , -- pedimento
						               (
						                 SELECT MAX(tblclsDocument.strDocumentFolio)
						                 FROM tblclsGCInventoryDocument
		             				       INNER JOIN tblclsDocument ON tblclsDocument.intDocumentId  = tblclsGCInventoryDocument.intDocumentId             				       
						                   INNER JOIN tblclsDocumentType ON tblclsDocumentType.intDocumentTypeId = tblclsDocument.intDocumentTypeId
						                 AND tblclsDocumentType.strDocumentTypeIdentifier = 'PIMPO'
						               )
						           				               
						     --,idTipo           ,intMotivo     ,idRecintoOrigen  ,idRecintoDestino ,		       		           
						     ,1,   (
						              CASE WHEN ISNULL(tblclsVisit.strDeliveryType,'')='TIT' THEN 2
						               ELSE 1
						               END
						           ) -- intMotivo
						                 , 10971 , 10971 --,idTipo ,intMotivo ,idRecintoOrigen,idRecintoDestino
						     --strimo           ,strBuque      ,  idSolicitante  ,idAgenteAduanal  ,
						     ,'' , tblclsVessel.strVesselName , DELENTY.intPISCompanyId , DELENTY.intPISCompanyId
						    --idTipoProducto   ,idManiobra    ,  idTipoDespacho  ,noContenedor   ,
						    , 2 , 1, 1,'' 
				            --idTipoContenedor ,idEstadoContenedor  ,strium      ,strfechaInicio  ,
				            ,'', 1 , CONVERT(VARCHAR(10),  tblclsVisitGeneralCargo.intGeneralCargoUniversalId)
				             ,str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  --CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) --
				            --idHoraInicia   ,strfechaFin         ,idHoraFinaliza ,inttotalTractos ,
				            ,	DATEPART(dd, tblclsVisit.dtmAppointmentDate) 
				                   ,  str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  -- CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) 
				                          , DATEPART(dd, tblclsVisit.dtmAppointmentDate), 1
				            --inttotalManiobras   ,strFechaItem   ,idHoraItem    ,noManiobras 
				            ,1 , str_replace( convert( varchar(16), tblclsVisit.dtmAppointmentDate, 111 ), '/', '-')  -- CONVERT(VARCHAR(16), tblclsVisit.dtmAppointmentDate) 
				            , DATEPART(dd, tblclsVisit.dtmAppointmentDate), 1
				            
				  FROM tblclsVisitGeneralCargo
				   inner join tblclsVisit on tblclsVisit.intVisitId = tblclsVisitGeneralCargo.intVisitId
				   INNER JOIN tblclsGeneralCargoInventory ON tblclsGeneralCargoInventory.intGeneralCargoUniversalId = tblclsVisitGeneralCargo.intGeneralCargoUniversalId
				   INNER JOIN tblclsGCInventoryItem       ON   tblclsGCInventoryItem.intGeneralCargoUniversalId = tblclsGeneralCargoInventory.intGeneralCargoUniversalId
				                                          AND  tblclsGCInventoryItem.intGCInventoryItemId = tblclsVisitGeneralCargo.intGCInventoryItemId
				                                          
				   inner join tblclsService on tblclsService.intServiceId = tblclsVisitGeneralCargo.intServiceId
				   INNER JOIN tblclsGeneralCargoDelivery ON   tblclsGeneralCargoDelivery.intGeneralCargoDeliveryId =tblclsVisitGeneralCargo.intServiceOrderId
				                                         AND  tblclsGeneralCargoDelivery.intServiceId = tblclsVisitGeneralCargo.intServiceId
				                                         
				   INNER JOIN tblclsGCDeliveryDetail ON  tblclsGCDeliveryDetail.intGeneralCargoDeliveryId = tblclsVisitGeneralCargo.intServiceOrderId
				                                     AND tblclsGCDeliveryDetail.intGeneralCargoUniversalId  = tblclsVisitGeneralCargo.intGeneralCargoUniversalId
				                                     AND tblclsGCDeliveryDetail.intGCInventoryItemId = tblclsVisitGeneralCargo.intGCInventoryItemId
				                                     AND tblclsGCDeliveryDetail.intGCDeliveryDetailId = tblclsVisitGeneralCargo.intServiceOrderDetailId
				   
				   INNER JOIN tblclsFiscalMovement  on tblclsGCInventoryItem.intFiscalMovementId = tblclsFiscalMovement.intFiscalMovementId
				   
				   LEFT JOIN tblclsVesselVoyage ON tblclsVesselVoyage.intVesselVoyageId = tblclsGCInventoryItem.intVesselVoyageId
				   LEFT JOIN tblclsVessel ON tblclsVessel.intVesselId = tblclsVesselVoyage.intVesselId
				   LEFT JOIN tblclsCompanyEntity DELENTY ON DELENTY.intCompanyEntityId = tblclsGeneralCargoDelivery.intGCDeliveryRequiredById
				                                         AND DELENTY.intCustomerTypeId = tblclsGeneralCargoDelivery.intGCDeliveryRequiredById
				   LEFT JOIN tblclsCompany DELCOMP ON DELCOMP.intCompanyId = DELENTY.intCompanyId
				   
				  where tblclsVisit.intVisitId  =@intVisitId   
          
         
       END  -- @lintCount =0
	     
       
        
         
        -- si la tabla es vacia 
        -- buscarla como salida de carga general 
      
      --- si es tabla vacia , buscarla como ingreso de contenedores 
      ----
      -------
      SELECT strVisitCodigo  ,
             intVisitItem ,
             idTipoOperacion  ,	
             isnull(strBL,'')as 'strBL' ,
             isnull(strPedimento,'')  as 'strPedimento',
             idTipo           ,
             intMotivo     ,
             idRecintoOrigen  ,
             idRecintoDestino ,
             strimo           ,
             strBuque      ,  
             idSolicitante  ,
             idAgenteAduanal  ,
             idTipoProducto   ,
             idManiobra    ,  
             idTipoDespacho  ,
             noContenedor   ,
             idTipoContenedor ,
             idEstadoContenedor  ,
             strium      ,
             strfechaInicio  ,
             idHoraInicia   ,
             strfechaFin         ,
             idHoraFinaliza ,
             inttotalTractos ,
             inttotalManiobras   ,
             strFechaItem   ,
             idHoraItem    ,
             noManiobras 

      FROM #VisitResult
      
		  -- tblclsService.strServiceIdentifier = 'ENTLL'      
		  
		  
   END 
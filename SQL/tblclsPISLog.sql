
-- drop table     tblclsPISLog

    CREATE TABLE tblclsPISLog
    (
     intEventId  numeric(16) NOT  NULL,
     intVisitId numeric(16) NULL,
     strMessage varchar(25) NULL,
     strUser varchar(16) NULL,
     dtmCreationStamp datetime NULL,
     PRIMARY KEY (intEventId)
    )

-- select * from tblclsPISLog


--alter table tblclsPISLog  modify strMessage varchar(150) null  

ALTER TABLE tblclsPISLog ADD strPISDate varchar(30) NULL

ALTER TABLE tblclsPISLog ADD intPISHour int NULL

ALTER TABLE 
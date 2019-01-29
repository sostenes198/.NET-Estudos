USE [master]
GO 

CREATE DATABASE [ESTUDOS]
GO
    
USE [ESTUDOS]
GO     
    
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO    

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IDEMPOTENCY_CONSUMER]') AND type in (N'U'))
DROP TABLE [dbo].[IDEMPOTENCY_CONSUMER]
GO

create table [dbo].[IDEMPOTENCY_CONSUMER]
(
    INSTANCE_ID     varchar(100) not null,
    IDEMPOTENCY_KEY varchar(100) not null,
    STATE           varchar(50)  not null,
    TIMESTAMP       datetime2    not null,
    constraint IDEMPOTENCY_CONSUMER_pk
        primary key (INSTANCE_ID, IDEMPOTENCY_KEY)
)
go

INSERT INTO [dbo].[IDEMPOTENCY_CONSUMER] (INSTANCE_ID, IDEMPOTENCY_KEY, STATE, TIMESTAMP) VALUES    
    ('INSTANCE-1', '4435b01c-8f24-4ee4-9f79-f9ba672a5d62', 'Committed', '2022-04-09'),
    ('INSTANCE-1', 'a7aef543-ad71-4b26-9062-ed1646501f3e', 'Committed', '2022-04-09'),
    ('INSTANCE-1', '912562e1-2685-427a-8fca-3a23f9fc5fd1', 'Committed', '2022-04-09'),
    ('INSTANCE-2', '01cbac19-4070-4694-b338-705ae78dad8f', 'Committed', '2022-04-09'),
    ('INSTANCE-2', 'f025cbe8-bfa9-4c1a-b3e1-ec357eb4de3d', 'Committed', '2022-04-09'),    
    ('INSTANCE-2', 'b22377cb-1b06-4b99-b474-9768b6bee993', 'Committed', '2022-04-09')
GO    




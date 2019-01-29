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

-----------------------------------------------------PRODUCT --------------------------------------------------------------------------

CREATE TABLE [dbo].[PRODUCT]
(
    ID              INT IDENTITY,
    PRODUCT_KEY     VARCHAR(50) UNIQUE NOT NULL,
    NAME            VARCHAR(50)        NOT NULL,
    DESCRIPTION     VARCHAR(200),
    ACTIVE          BIT                NOT NULL,
    USER_INCLUSION  VARCHAR(100)       NOT NULL,
    DATE_INCLUSION  DATETIME2          NOT NULL,
    USER_ALTERATION VARCHAR(100)       NOT NULL,
    DATE_ALTERATION DATETIME2          NOT NULL,
    CONSTRAINT PK_PRODUCT_ID PRIMARY KEY (ID),
)
GO

INSERT INTO [SQL_FACIAL_BIOMETRICS].[dbo].[PRODUCT] (PRODUCT_KEY, NAME, DESCRIPTION, ACTIVE, USER_INCLUSION, DATE_INCLUSION, USER_ALTERATION, DATE_ALTERATION)
    VALUES ('base-integration-test', 'Base Integration Test', 'ProductId To Integration Tests', 1, 'integration-test', '2021-10-26', 'integration-test', '2021-10-26')
GO

---------------------------------------------------------------------------------------------------------------------------------------
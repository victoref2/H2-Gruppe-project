USE [master];
GO
CREATE LOGIN [AddsUsers_User] WITH PASSWORD = 'Password123!';
GO

USE [AutoAuctionDB];
GO
CREATE USER [AddsUsers_User] FOR LOGIN [AddsUsers_User];
GO

GRANT EXECUTE TO [AddsUsers_User];
GO

-- Deny SELECT on tables
DENY SELECT ON SCHEMA::dbo TO [AddsUsers_User];
-- Deny INSERT on tables
DENY INSERT ON SCHEMA::dbo TO [AddsUsers_User];
-- Deny UPDATE on tables
DENY UPDATE ON SCHEMA::dbo TO [AddsUsers_User];
-- Deny DELETE on tables
DENY DELETE ON SCHEMA::dbo TO [AddsUsers_User];
GO

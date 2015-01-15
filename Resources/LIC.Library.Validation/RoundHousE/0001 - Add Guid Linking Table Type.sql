-- TODO:
--	Replace --[schemaname]-- with your schema name (e.g. milkproduction)

BEGIN TRANSACTION

/* create table type to be used as parameter when doing large event data retrieval */
CREATE TYPE --[schemaname]--.[GuidLinkingTableType] AS TABLE(
	[value] [uniqueidentifier] NULL
)
GO

/* Descriptions */
declare @v sql_variant 

set @v = N'This type is used to define a set of Guids which can then be joined onto to improve the performance of a bulk query see: http://stackoverflow.com/questions/17437802/selecting-multiple-rows-by-id-is-there-a-faster-way-than-where-in'
exec sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'--[schemaname]--', N'TYPE', N'GuidLinkingTableType'
GO

COMMIT TRANSACTION
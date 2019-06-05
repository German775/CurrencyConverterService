delete from Rates
delete from Currencies
DBCC CHECKIDENT ('Rates', RESEED)
DBCC CHECKIDENT ('Currencies', RESEED)

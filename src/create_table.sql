IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_NAME = 'Toggles'))
BEGIN
    CREATE TABLE [Toggles] (
        [Id] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_Toggles] PRIMARY KEY ([Id])
    );
END;
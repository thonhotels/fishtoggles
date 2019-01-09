# fishtoggles
Tiny tiny library for feature toggles in sql server

### Install
dotnet add package fishtoggles

### Usage
Add a call to UseFeatureToggles. Pass in commandline args and a logging delegate. Both can be empty.

```c#
public static IWebHost BuildWebHost(string[] args)
{
    return
        WebHost.CreateDefaultBuilder(args)
        .UseFeatureToggles(args, (ex, msg) => Log.Warning(ex, msg))
    .
    .
    .
}
        
```

If you don't want ability to read config from command line args and/or don't want any logging just pass in and empty array and/or no-op delegate like this: 
```c#
WebHost.CreateDefaultBuilder(args)
        .UseFeatureToggles(new string[0], (x, y) => {})
```

### Database
Run the following script in your database:
```sql
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
```

Insert named feature toggles in the toggles table:

```sql
INSERT INTO Toggles
VALUES ('FeatureMyFeature', 'true') --an enabled feature toggle named: FeatureMyFeature
```

Check the state of a feature toggle in code like this:
```c#
public void SomeMethod(IConfiguration configuration)
{
    if (configuration["FeatureMyFeature"])
    {
        //feature is enabled
    }
}
```


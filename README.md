# EnvForU
A .Net alternative to appSettings.  Environment should live in Environment Variables

```csharp
 var env = new Settings(); 
```
Brings in all the settings from .env in the current project to the dictionary

Settings in the .env should be in the following format

``` 
setting1=value1
setting2=value2
```

All settings are case insensitive but by convention should be UPPERCASE to match the convention of environment variables.

## Utilizing this project should allow you to product an atomic application that can easily be used with BeanStalk, OpenShift and other environments that require Environment Variables for config, but most importantly can help you maintain a truely atomic version of the application when using git deployment strategies.

           
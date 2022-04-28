# poc-serilog-level-dinamico

## Objetivo
Alterar o level do log dinamicamente

## Solução

implementar um serviço que alterar o ```LoggingLevelSwitch``` do Logger e determinar que ele seja controlado pelo serviço:


``` 
Serilog.Log.Logger = new LoggerConfiguration()
                        .Destructure.ByTransforming<HttpRequest>(r => new { Path = r.Path, Body = r.Body, Method = r.Method })
                        .MinimumLevel.ControlledBy(_loggingService.GetLoggingLevelSwitch())
                        .WriteTo.Switch(wt => wt.Console(levelSwitch: _loggingService.GetLoggingLevelSwitch()), services)
                        .ReadFrom.Configuration(configuration)
                        .CreateBootstrapLogger(); 
```

## Como trocar ?

Apenas chamar o serviço e passar o level desejado. Exemplo:

```
_logLevelSwitch.SetLoggingLevel((LogEventLevel)Enum.Parse(typeof(LogEventLevel), levelDesejado, true));
```

## Opções de level

* Verbose       
* Debug        
* Information        
* Warning        
* Error
* Fatal


## Resultado

![image](https://user-images.githubusercontent.com/8622005/165852188-5fca14ec-feef-4f46-80de-34e362aeb83a.png)


![image](https://user-images.githubusercontent.com/8622005/165852391-6a3ad9bb-5788-4ca2-98b8-25968bb094c9.png)




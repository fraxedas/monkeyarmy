Monkey army
==========  
Some monkeys to wreck your infrastructure  

Available on nuget: https://www.nuget.org/packages/monkeyarmy  
```Batchfile
    Install-Package monkeyarmy
```

Sample
======
```c#
    Monkey _monkey;
    _monkey = new ServiceMonkey()
              .WithServiceNameContaining("MSDTC")
              .IfServiceIsRunning()
              .StopService();
    _monkey.Wreck();
```

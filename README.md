Monkey army
==========
Some monkeys to wreck your infrastructure  

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

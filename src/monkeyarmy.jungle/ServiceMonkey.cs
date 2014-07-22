using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace monkeyarmy.jungle
{
    public class ServiceMonkey : Monkey
    {
        public ServiceMonkey()
        {
            Selectors = new List<Func<ServiceController, bool>>();
            Actions = new List<Action<ServiceController>>();
        }

        public IList<Func<ServiceController, bool>> Selectors { get; set; }
        public IList<Action<ServiceController>> Actions { get; set; }

        public ServiceMonkey WithServiceNameStartingWith(string name)
        {
            Selectors.Add(controller => controller.ServiceName.StartsWith(name));
            return this;
        }

        public ServiceMonkey WithServiceWithName(string name)
        {
            Selectors.Add(controller => controller.ServiceName.Equals(name));
            return this;
        }

        public ServiceMonkey WithServiceNameContaining(string name)
        {
            Selectors.Add(controller => controller.ServiceName.Contains(name));
            return this;
        }

        public ServiceMonkey IfServiceIsRunning()
        {
            Selectors.Add(controller => controller.Status == ServiceControllerStatus.Running);
            return this;
        }

        public ServiceMonkey IfServiceIsStopped()
        {
            Selectors.Add(controller => controller.Status == ServiceControllerStatus.Stopped);
            return this;
        }

        public ServiceMonkey StartService()
        {
            Actions.Add(controller =>
                            {
                                controller.Start();
                                controller.WaitForStatus(ServiceControllerStatus.Running);
                            });
            return this;
        }

        public ServiceMonkey StopService()
        {
            Actions.Add(controller =>
            {
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);
            });
            return this;
        }

        public override void Wreck()
        {
            var services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (!Selectors.All(x => x(service))) continue;
                Console.Out.WriteLine("{0} is {1}", service.ServiceName, service.Status);
                foreach (var action in Actions)
                {
                    action(service);
                }
            }
        }
    }
}
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
        }

        public IList<Func<ServiceController, bool>> Selectors { get; set; }
        public IEnumerable<ServiceController> Services { get; set; }

        public ServiceMonkey ServiceNameStartsWith(string name)
        {
            Selectors.Add(controller => controller.ServiceName.StartsWith(name));
            return this;
        }

        public ServiceMonkey ServiceWithName(string name)
        {
            Selectors.Add(controller => controller.ServiceName.Equals(name));
            return this;
        }

        public ServiceMonkey ServiceNameContains(string name)
        {
            Selectors.Add(controller => controller.ServiceName.Contains(name));
            return this;
        }

        public override void Wreck()
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (Selectors.All(x => x(service)))
                {
                    Console.Out.WriteLine("service = {0}", service.ServiceName);
                    Wreck(service);
                }
            }
        }

        private void Wreck(ServiceController service)
        {
            switch (service.Status)
            {
                case ServiceControllerStatus.Running:
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    break;
                case ServiceControllerStatus.Stopped:
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    break;
            }
        }
    }
}
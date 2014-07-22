using System.Linq;
using System.ServiceProcess;
using NUnit.Framework;
using monkeyarmy.jungle;

namespace monkeyarmy.test
{
    [TestFixture]
    public class ServiceMonkeyTests
    {
        private ServiceMonkey _monkey;

        [Test]
        public void Test_werck_change_the_status_of_a_running_service_to_stopped()
        {
            _monkey = new ServiceMonkey()
                .WithServiceNameContaining("MSDTC")
                .IfServiceIsRunning();

            _monkey.Wreck();

            var service = ServiceController.GetServices().First(x => x.ServiceName == "MSDTC");
            Assert.That(service.Status == ServiceControllerStatus.Stopped);
        }

        [Test]
        public void Test_werck_change_the_status_of_a_stopped_service_to_running()
        {
            _monkey = new ServiceMonkey()
                .WithServiceNameContaining("MSDTC")
                .IfServiceIsStopped();

            _monkey.Wreck();

            var service = ServiceController.GetServices().First(x => x.ServiceName == "MSDTC");
            Assert.That(service.Status == ServiceControllerStatus.Running);
        }
    }
}

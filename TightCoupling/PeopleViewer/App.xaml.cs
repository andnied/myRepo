using Microsoft.Practices.Unity;
using PeopleViewer.Presentation;
using PersonRepository.Caching;
using PersonRepository.CSV;
using PersonRepository.Interface;
using PersonRepository.Service;
using System.Windows;

namespace PeopleViewer
{
    public partial class App : Application
    {
        private static IUnityContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ComposeContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private static void ComposeContainer()
        {
            Container = new UnityContainer();
            Container.RegisterType<IPersonRepository, ServiceRepository>(new ContainerControlledLifetimeManager());
        }

        private static void ComposeObjects()
        {
            Current.MainWindow = Container.Resolve<PeopleViewerWindow>();
            Current.MainWindow.Title = "Loose Coupling App";
        }
    }
}

using System.Windows;
using Calculator.Log;

namespace Calculator
{ 
    public partial class App 
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Logger.Log("start app");
        }
    }
}

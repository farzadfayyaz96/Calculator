using System.Windows;
using Calculator.Log;

namespace Calculator
{ 
    public partial class App 
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Logger.CloseFile();
        }
    }
}

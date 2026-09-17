using System.Windows;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Service
{
    public static class CatchExeption
    {
        public static async Task ExecuteWithCatchAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (ScopedExeption ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());             
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());           
            }
        }
    }
}

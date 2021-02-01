using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.Implement_Diagnostics_In_An_Application
{
    public class Debug_Assert
    {
        public static void Test()
        {
            var customerName = "Rob";
            Debug.Assert(!string.IsNullOrWhiteSpace(customerName));

            customerName = string.Empty;
            Debug.Assert(!string.IsNullOrWhiteSpace(customerName));
        }
    }
}
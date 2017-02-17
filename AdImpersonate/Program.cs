using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdImpersonate
{
    class Program
    {
        static void Main(string[] args)
        {
            string domain = "";
            string username = "";
            string password = "";

            Console.WriteLine("Impersonated User: " + WindowsIdentity.GetCurrent().Name);

            using (ADImpersonate impersonate = new ADImpersonate(domain, username, password))
            {
                Console.WriteLine("Impersonated User: " + WindowsIdentity.GetCurrent().Name);
                
                //Do your work as the impersonated user
                string[] files = Directory.GetFiles("C:\\");              
            }

            //user is put back
            Console.WriteLine("Impersonated User: " + WindowsIdentity.GetCurrent().Name);
        }
    }
}

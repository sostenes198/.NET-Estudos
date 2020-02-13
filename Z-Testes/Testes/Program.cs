using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Testes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BaseClass bc = new BaseClass();  
            DerivedClass dc = new DerivedClass();  
            BaseClass bcdc = new DerivedClass();  
            
            bc.Method1();  
            bc.Method2();  
            dc.Method1();  
            dc.Method2();  
            bcdc.Method1();  
            bcdc.Method2();
        }
        
        class BaseClass  
        {  
            public virtual void Method1()  
            {  
                Console.WriteLine("Base - Method1");  
            }  
            
            public void Method2()  
            {  
                Console.WriteLine("Base - Method2");  
            } 
        }  
          
        class DerivedClass : BaseClass  
        {  
            public override void Method1()  
            {  
                Console.WriteLine("Derived - Method1");  
            }  
            
            public new void Method2()  
            {  
                Console.WriteLine("Derived - Method2");  
            }  
            
            public void Method3()  
            {  
                Console.WriteLine("Derived - Method3");  
            }  
        }  
    }
}
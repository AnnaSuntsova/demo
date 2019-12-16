using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCSample
{
	public class CustomerBLL
	{
        public string Message { get; set; }

        public CustomerBLL()
        {
            Message = "Hello world!";
        }         

        public void WelcomeWorld()
        {
            Console.WriteLine(Message);
        }
    }
}

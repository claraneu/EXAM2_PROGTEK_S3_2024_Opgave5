using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM2_Opgave4_TCPApp_TEK3_2024
{
    public class Message
    {
        public string Method { get; set; }
        public int Num1 { get; set; }
        public int Num2 { get; set; }

        public Message(string method, int num1, int num2)
        { 
            Method = method;
            Num1 = num1;
            Num2 = num2;
        
        }

        public Message()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server_Fix
{
    class Program
    {
        private static int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        private static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];

            recv = s.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize,0);
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.ASCII.GetBytes("exit ");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }           
            return data;
        }

        //private static int ReceiveVarData1(Socket s)
        //{
        //    int total = 0;
        //    int recv;
        //    byte[] datasize = new byte[4];

        //    recv = s.Receive(datasize, 0, 4, 0);
        //    int size = BitConverter.ToInt32(datasize, 0);
        //    int dataleft = size;
        //    byte[] data = new byte[size];


        //    while (total < size)
        //    {
        //        recv = s.Receive(data, total, dataleft, 0);
        //        if (recv == 0)
        //        {
        //            data = Encoding.ASCII.GetBytes("exit ");
        //            break;
        //        }
        //        total += recv;
        //        dataleft -= recv;
        //    }

        //    int a = 0;
        //    foreach (byte byteValue in data)
        //    {
        //        a = Convert.ToInt32(data);
        //    }
        //    return a;
        //}

        public static void Main()
        {
            byte[] data = new byte[1024];
            byte[] data1 = new byte[1024];
            byte[] data2 = new byte[1024];
            byte[] data3 = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);
            Console.WriteLine("Waiting for a client...");

            Socket client = newsock.Accept();
            IPEndPoint newclient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            newclient.Address, newclient.Port);

            string welcome = "CALCULATOR CLIENT-SERVER DIAGRAM!";
            data = Encoding.ASCII.GetBytes(welcome);
            int sent = SendVarData(client, data);

            string phepToan;
            int result=0;
            int a = 0,  b=0;
            //char c = '\0';
            while (true)
            {
                //data = ReceiveVarData(client);
                
                //Console.WriteLine("Client: " + Encoding.ASCII.GetString(data));
                
                //input1 = "Nhap vao so a: ";

                //Console.SetCursorPosition(0, Console.CursorTop - 1);

                //Console.WriteLine("You: " + input);

                sent = SendVarData(client, Encoding.ASCII.GetBytes("Nhap vao so a: "));
                data1 = ReceiveVarData(client); //Giá trị a
                
                //Console.WriteLine("Client: " + Encoding.ASCII.GetString(data));
                sent = SendVarData(client, Encoding.ASCII.GetBytes("Nhap vao so b: "));
                data2 = ReceiveVarData(client); //Giá trị b
                //Console.WriteLine(data2.ToString());
                //b = Convert.ToInt32(data2);
                sent = SendVarData(client, Encoding.ASCII.GetBytes("Cho biet phep tinh can dung la | + | - | * | / |: "));
                data3 = ReceiveVarData(client); //Biểu thức phép toán
                phepToan = Encoding.ASCII.GetString(data3);
                //a = Convert.ToString(Encoding.ASCII.GetString(data1));
                if (phepToan=="+")
                {
                    foreach (int byteValue in data1)
                    {                       
                       a=(byteValue);
                                                
                    }
                    char ca = (char)a;
                    string a1 = ca.ToString();
                   
                    foreach (byte byteValue in data2)
                    {
                        b = byteValue;
                    }
                    char cb = (char)b;
                    string b1 = cb.ToString();
                    result = Convert.ToInt32(a1) + Convert.ToInt32(b1);
                    //Console.WriteLine(result);
                    //for (int i = 0; i < data1.Length; i++)
                    //{
                    //    a = BitConverter.ToInt32(data1[i],0);

                    //}

                    //for (int i = 0; i < data2.Length; i++)
                    //{
                    //    b = Convert.ToChar(data2[i]);
                    //}
                    //result = Convert.a+b;

                    //result = (Int32)(Convert.ToInt16(data1) + Convert.ToInt16(data2));
                    //result = Convert.ToInt32(data1) + Convert.ToInt32(data2);
                    sent = SendVarData(client, Encoding.ASCII.GetBytes("Ket qua phep tinh: "+Convert.ToString(result)));
                }
                if (phepToan == "-")
                {
                    result = (Int32)(Convert.ToInt16(data1) - Convert.ToInt16(data2));
                    //result = Convert.ToInt32(data1) - Convert.ToInt32(data2);
                    sent = SendVarData(client, Encoding.ASCII.GetBytes(Convert.ToString(result)));
                }
                if (phepToan == "*")
                {
                    result = (Int32)(Convert.ToInt16(data1) * Convert.ToInt16(data2));
                    //result = Convert.ToInt32(data1) * Convert.ToInt32(data2);
                    sent = SendVarData(client, Encoding.ASCII.GetBytes(Convert.ToString(result)));
                }
                if (phepToan == "/")
                {
                    result = (Int32)(Convert.ToInt16(data1) / Convert.ToInt16(data2));
                    //result = Convert.ToInt32(data1) / Convert.ToInt32(data2);
                    sent = SendVarData(client, Encoding.ASCII.GetBytes(Convert.ToString(result)));
                }
            }
           // Console.WriteLine("Disconnected from {0}", newclient.Address);
            //client.Close();
            //newsock.Close();
            Console.ReadLine();
        }
    }
}

using EvalClient.EvalServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EvalClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var channel = new EvalServiceClient("BasicHttpBinding_IEvalService"))
            {
                try
                {
                    channel.SubmitEval(new EvalServiceLibrary.Eval()
                    {
                        Message = "Test msg",
                        Submitter = "Tester1"
                    });
                    channel.SubmitEval(new EvalServiceLibrary.Eval()
                    {
                        Message = "Test msg2",
                        Submitter = "Tester2"
                    });
                    
                    var task = channel.GetEvalsAsync();
                    Console.WriteLine("Waiting...");
                    Console.WriteLine(task.Result.Count());
                    Console.ReadKey();
                }
                catch (FaultException e)
                {
                    Console.WriteLine(e.ToString());
                    channel.Abort();
                }
                catch (CommunicationException e)
                {
                    Console.WriteLine(e.ToString());
                    channel.Abort();
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine(e.ToString());
                    channel.Abort();
                }
            }
        }
    }
}

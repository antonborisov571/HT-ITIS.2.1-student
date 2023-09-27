using System.Diagnostics;
using Hw3.Mutex;

Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} starts");
using var wm = new WithMutex();
Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} acquires mutex");
Console.WriteLine(wm.Increment());
Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} releases mutex");

//var delay = 1000;

//try
//{
//    var semaphore = Semaphore.OpenExisting("semaphore");
//    semaphore.WaitOne(delay + 100);
//    Thread.Sleep(delay);
//    semaphore.Release();
//}
//catch
//{
//    var semaphore = new Semaphore(1, 1, "semaphore");
//    semaphore.WaitOne();
//    Thread.Sleep(delay);
//    semaphore.Release();
//}
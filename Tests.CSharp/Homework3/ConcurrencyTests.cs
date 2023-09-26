using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Hw3.Mutex;
using Tests.RunLogic.Attributes;
using Xunit.Abstractions;

namespace Tests.CSharp.Homework3;

public class ConcurrencyTests
{
    private readonly ITestOutputHelper _toh;

    public ConcurrencyTests(ITestOutputHelper toh)
    {
        _toh = toh;
    }

    [Homework(Homeworks.HomeWork3)]
    public void SingleThread_NoRaces()
    {
        var expected = Concurrency.Increment(1, 1000);
        Assert.Equal(expected, Concurrency.Index);
    }

    [Homework(Homeworks.HomeWork3)]
    public void FiveThreads_100Iterations_RaceIsHardToReproduce()
    {
        var expected = Concurrency.Increment(5, 100);
        Assert.Equal(expected, Concurrency.Index);
    }

    [Homework(Homeworks.HomeWork3)]
    public void EightThreads_100KIterations_RaceIsReproduced()
    {
        var expected = Concurrency.Increment(8, 100_000);
        Assert.NotEqual(expected, Concurrency.Index);
        _toh.WriteLine($"Expected: {expected}; Actual: {Concurrency.Index}");
    }

    [Homework(Homeworks.HomeWork3)]
    public void EightThreads_100KIterations_WithLock_NoRaces()
    {
        var expected = Concurrency.IncrementWithLock(8, 100_000);
        Assert.Equal(expected, Concurrency.Index);
        _toh.WriteLine($"Expected: {expected}; Actual: {Concurrency.Index}");
    }

    [Homework(Homeworks.HomeWork3)]
    public void EightThreads_100KIterations_LockIsSyntaxSugarForMonitor_NoRaces()
    {
        var expected = Concurrency.IncrementWithLock(8, 100_000);
        Assert.Equal(expected, Concurrency.Index);
        _toh.WriteLine($"Expected: {expected}; Actual: {Concurrency.Index}");
    }

    [Homework(Homeworks.HomeWork3)]
    public void EightThreads_100KIterations_WithInterlocked_NoRaces()
    {
        var expected = Concurrency.IncrementWithInterlocked(8, 100_000);
        Assert.Equal(expected, Concurrency.Index);
        _toh.WriteLine($"Expected: {expected}; Actual: {Concurrency.Index}");
    }

    [Homework(Homeworks.HomeWork3)]
    public void EightThreads_100KIterations_InterlockedIsFasterThanLock_Or_IsIt()
    {
        var isM1Mac = OperatingSystem.IsMacOS() &&
                      RuntimeInformation.ProcessArchitecture == Architecture.Arm64;

        var elapsedWithLock = StopWatcher.Stopwatch(EightThreads_100KIterations_WithLock_NoRaces);
        var elapsedWithInterlocked = StopWatcher.Stopwatch(EightThreads_100KIterations_WithInterlocked_NoRaces);

        _toh.WriteLine($"Lock: {elapsedWithLock}; Interlocked: {elapsedWithInterlocked}");

        // see: https://godbolt.org/z/1TzWMz4aj
        if (isM1Mac)
            Assert.True(elapsedWithLock < elapsedWithInterlocked);
        else
            Assert.True(elapsedWithLock > elapsedWithInterlocked);
    }

    [Homework(Homeworks.HomeWork3)]
    public void Semaphore()
    {
        // TODO: homework+
        var delay = 1000;
        var semaphore = new Semaphore(2, 2);
        var listTasks = new Task[4];

        for (int i = 0; i < 4; i++)
        {
            listTasks[i] = new Task(() =>
            {
                semaphore.WaitOne(delay + 100);
                Thread.Sleep(delay);
                semaphore.Release();
            });
        }
        var sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < 4; i++)
        {
            listTasks[i].Start();
        }
        Task.WaitAll(listTasks);

        Assert.True(sw.Elapsed.TotalMilliseconds >= delay * 2);
    }

    [Homework(Homeworks.HomeWork3)]
    public async Task SemaphoreSlimWithTasks()
    {
        var expected = await Concurrency.IncrementAsync(8, 100_000);
        Assert.Equal(expected, Concurrency.Index);
    }

    [Homework(Homeworks.HomeWork3)]
    public void NamedSemaphore_InterprocessCommunication()
    {
        // TODO: homework+
        // https://learn.microsoft.com/en-us/dotnet/standard/threading/semaphore-and-semaphoreslim#named-semaphores
        // see mutex as example
        var delay = 1000;
        var p1 = new Task(() =>
        {
            var semaphore = new Semaphore(1, 1, "semaphore");
            semaphore.WaitOne();
            Thread.Sleep(delay);
            semaphore.Release();
        });

        var p2 = new Task(() =>
        {
            Thread.Sleep(100);
            var semaphore = System.Threading.Semaphore.OpenExisting("semaphore");
            semaphore.WaitOne(delay + 100);
            Thread.Sleep(delay);
            semaphore.Release();
        });

        var sw = new Stopwatch();
        sw.Start();
        p1.Start();
        p2.Start();
        Task.WaitAll(p1, p2);

        Assert.True(sw.Elapsed.TotalMilliseconds >= delay * 2);
    }

    [Homework(Homeworks.HomeWork3)]
    public void ConcurrentDictionary_100KIterations_WithInterlocked_NoRaces()
    {
        var expected = Concurrency.IncrementWithConcurrentDictionary(8, 100_000);
        Assert.Equal(expected, Concurrency.Index);
    }

    [Homework(Homeworks.HomeWork3)]
    public async Task Mutex()
    {
        var p1 = new Process
        {
            StartInfo = GetProcessStartInfo()
        };
        var p2 = new Process
        {
            StartInfo = GetProcessStartInfo()
        };

        var sw = new Stopwatch();
        sw.Start();
        p1.Start();
        p2.Start();
        await p1.WaitForExitAsync();
        await p2.WaitForExitAsync();
        p1.WaitForExit();
        var val = await p1.StandardOutput.ReadToEndAsync();
        _toh.WriteLine(val);

        p2.WaitForExit();
        val = await p2.StandardOutput.ReadToEndAsync();
        sw.Stop();
        _toh.WriteLine(val);

        Assert.True(sw.Elapsed.TotalMilliseconds >= WithMutex.Delay * 2);
    }

    private static ProcessStartInfo GetProcessStartInfo()
    {
        return new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run --project ../../../../Homework3/Hw3.Mutex/Hw3.Mutex.csproj",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
    }
}
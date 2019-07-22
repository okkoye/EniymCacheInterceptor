using BenchmarkDotNet.Running;
using System;

namespace EniymCacheInterceptor.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<EniymCacheInterceptorTest>();
            Console.ReadLine();
        }
    }
}
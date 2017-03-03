using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace a8
{
    class Program
    {
        private static byte[] _bytes;

        static void Main(string[] args)
        {
            _bytes = new byte[2048*2048];
            
            var stopwatch = new Stopwatch();

            var cnt = 1000;

            stopwatch.Start();
            for (int i = 0; i < cnt; i++)
            {
                Reader();
            }
            stopwatch.Stop();
            Console.WriteLine(nameof(Reader) + " " + stopwatch.ElapsedMilliseconds);


            stopwatch.Restart();
            for (int i = 0; i < cnt; i++)
            {
                For();
            }
            stopwatch.Stop();
            Console.WriteLine(nameof(For) + " " + stopwatch.ElapsedMilliseconds);

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte For()
        {
            var span = new Span<byte>(_bytes);
            byte ch = 0;
            for (int i = 0; i < span.Length; i++)
            {
                ch = span[i];
            }
            return ch;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte Reader()
        {
            var reader = new SpanReader(new Span<byte>(_bytes));
            byte ch = 0;
            do
            {
                ch = reader.Take();
            }
            while (!reader.End);
            return ch;
        }
    }


    public struct SpanReader
    {
        public bool End;
        public Span<byte> Data;
        public int Index;

        public SpanReader(Span<byte> data) : this()
        {
            Data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte Take()
        {
            var result = Data[Index];
            Index++;
            if (Index >= Data.Length)
            {
                End = true;
            }
            return result;
        }
    }
}

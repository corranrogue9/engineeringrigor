namespace ConsoleApp2
{
    using System.Collections.Concurrent;

    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cache = new ConcurrentDictionary<string, AtomicCacheEntry<string, Task<string>>>();
            Console.WriteLine(await HandleRequest(cache, "someinputvalue"));
        }

        static async Task<string> HandleRequest(ConcurrentDictionary<string, AtomicCacheEntry<string, Task<string>>> cache, string input)
        {
            var entry = new AtomicCacheEntry<string, Task<string>>(() => GetDataAsync());
            var retrievedEntry = cache.GetOrAdd("input", (_) => entry);
            return await retrievedEntry.Value;
        }

        static async Task<string> GetDataAsync()
        {
            return await Task.FromResult("ASdf");
        }
    }

    class AtomicCacheEntry<TKey, TValue>
    {
        private readonly Func<TValue> add;

        private TValue value;

        private int @lock;

        public AtomicCacheEntry(Func<TValue> add)
        {
            this.add = add;

            @lock = 0;
        }

        public TValue Value
        {
            get
            {
                if (Interlocked.Exchange(ref this.@lock, 1) == 0)
                {
                    this.value = this.add();
                    this.@lock = 2;
                }

                while (this.@lock != 2)
                {
                }

                return this.value;
            }
        }
    }
}
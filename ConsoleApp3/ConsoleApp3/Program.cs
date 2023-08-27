using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata;
using static ConsoleApp3.ListyExtensions;

namespace ConsoleApp3
{
    public static class Extensions
    {
        public static IEnumerable<Property> GetAllProperties(Schema schema)
        {
            return schema.GetAllProperties();
        }

        public static IEnumerable<Property> GetEffectiveProperties(this IEnumerable<Schema> self)
        {
            foreach (var schema in self)
            {
                foreach (var property in schema.GetAllProperties())
                {
                    yield return property;
                }
            }
        }

        public static IEnumerable<T> ToLinkedList<T>(this T self, Func<T, T> nextNodeSelector, Func<T, bool> terminalNodePredicate)
        {
            while (!terminalNodePredicate(self))
            {
                yield return self;
                self = nextNodeSelector(self);
            }
        }

        public static IEnumerable<T> ToLinkedList<T>(this T self, Func<T, T?> nextNodeSelector)
        {
            var newSelf = self;
            do
            {
                yield return newSelf;
            } while ((newSelf = nextNodeSelector(newSelf)) != null);
        }

        public static IEnumerable<Schema> TraverseUp(this Schema self)
        {
            while (self != null)
            {
                yield return self;
                self = self.BaseResourceSchema;
            }
        }
    }

    public sealed class Schema
    {
        private readonly List<Property> properties;

        public Schema(IEnumerable<Property> properties)
        {
            this.properties = properties.ToList();
            this.BaseResourceSchema = null;
        }

        public IReadOnlyCollection<Property> GetAllProperties()
        {
            return properties;
        }

        public Schema? BaseResourceSchema { get; }
    }

    public sealed class Property
    {
        public Property(EdmTypeKind syntax, string? name)
        {
            this.Syntax = syntax;
            this.Name = name;
        }

        public EdmTypeKind Syntax { get; }

        public string? Name { get; }
    }

    public enum EdmTypeKind
    {
        EntityReference,
        Other,
    }

    public static class Container
    {
        public static string DoWork2(Schema entitySchema)
        {
            var toWrite = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema, schema => schema == null)
                .SelectMany(schema => schema.GetAllProperties())
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork3(Schema entitySchema)
        {
            var toWrite = entitySchema
                .TraverseUp()
                .SelectMany(schema => schema.GetAllProperties())
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork4(Schema entitySchema)
        {
            var toWrite = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema)
                .SelectMany(schema => schema.GetAllProperties())
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork5(Schema entitySchema)
        {
            string samplePropertyString = string.Empty;
            var schemas = entitySchema
                .TraverseUp();
            foreach (var schema in schemas)
            {
                var properties = schema.GetAllProperties();
                if (properties.Count >= 1)
                {
                    int count = 0;
                    foreach (var property in properties)
                    {
                        if (property.Syntax != EdmTypeKind.EntityReference)
                        {
                            if (samplePropertyString == string.Empty)
                            {
                                samplePropertyString = property.Name;
                            }
                            else
                            {
                                samplePropertyString += "," + property.Name;
                            }

                            count++;
                        }

                        if (count == 2)
                        {
                            break;
                        }
                    }

                    if (count > 0)
                    {
                        break;
                    }
                }
            }

            return samplePropertyString;
        }

        public static string DoWork6(Schema entitySchema)
        {
            string samplePropertyString = string.Empty;
            var schemas = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema);
            foreach (var schema in schemas)
            {
                var properties = schema.GetAllProperties();
                if (properties.Count >= 1)
                {
                    int count = 0;
                    foreach (var property in properties)
                    {
                        if (property.Syntax != EdmTypeKind.EntityReference)
                        {
                            if (samplePropertyString == string.Empty)
                            {
                                samplePropertyString = property.Name;
                            }
                            else
                            {
                                samplePropertyString += "," + property.Name;
                            }

                            count++;
                        }

                        if (count == 2)
                        {
                            break;
                        }
                    }

                    if (count > 0)
                    {
                        break;
                    }
                }
            }

            return samplePropertyString;
        }

        public static string DoWork7(Schema entitySchema)
        {
            var toWrite = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema)
                .SelectMany(Extensions.GetAllProperties)
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork8(Schema entitySchema)
        {
            var toWrite = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema)
                .GetEffectiveProperties()
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork9(Schema entitySchema)
        {
            var schemas = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema);
            var toWrite = new List<string>(2);
            foreach (var schema in schemas)
            {
                var properties = schema
                    .GetAllProperties()
                    .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                    .Select(property => property.Name)
                    .Take(2);
                toWrite.AddRange(properties);
                if (toWrite.Count == 2)
                {
                    break;
                }
            }

            var samplePropertyString = string.Join(",", toWrite);
            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }

        public static string DoWork10(Schema entitySchema)
        {
            string samplePropertyString = string.Empty;
            var schemas = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema);
            foreach (var schema in schemas)
            {
                var properties = schema
                    .GetAllProperties()
                    .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                    .Select(property => property.Name)
                    .Take(2);
                int count = 0;
                foreach (var property in properties)
                {
                    if (samplePropertyString == string.Empty)
                    {
                        samplePropertyString = property;
                    }
                    else
                    {
                        samplePropertyString += "," + property;
                    }

                    count++;

                    if (count == 2)
                    {
                        break;
                    }
                }

                if (count > 0)
                {
                    break;
                }
            }

            return samplePropertyString;
        }

        public static string DoWork11(Schema entitySchema)
        {
            string samplePropertyString = string.Empty;
            var properties = entitySchema
                .ToLinkedList(schema => schema.BaseResourceSchema)
                .SelectMany(schema => schema.GetAllProperties())
                .Where(property => property.Syntax != EdmTypeKind.EntityReference)
                .Select(property => property.Name)
                .Take(2);
            foreach (var property in properties)
            {
                if (samplePropertyString == string.Empty)
                {
                    samplePropertyString = property;
                }
                else
                {
                    samplePropertyString += "," + property;
                }
            }

            return samplePropertyString;
        }

        public static string DoWork(Schema entitySchema)
        {
            string samplePropertyString = string.Empty;
            while (entitySchema != null)
            {
                var properties = entitySchema.GetAllProperties();
                if (properties.Count >= 1)
                {
                    int count = 0;
                    foreach (var property in properties)
                    {
                        if (property.Syntax != EdmTypeKind.EntityReference)
                        {
                            if (samplePropertyString == string.Empty)
                            {
                                samplePropertyString = property.Name;
                            }
                            else
                            {
                                samplePropertyString += "," + property.Name;
                            }

                            count++;
                        }

                        if (count == 2)
                        {
                            break;
                        }
                    }

                    if (count > 0)
                    {
                        break;
                    }
                }

                entitySchema = entitySchema.BaseResourceSchema;
            }

            ////Console.WriteLine(samplePropertyString);
            return samplePropertyString;
        }
    }

    public static class ListyExtensions
    {
        public static void DoWork(IEnumerable<string> data)
        {
            foreach (var element in data.ToListy())
            {
            }
        }

        public static ListyEnumerable<T> ToListy<T>(this IEnumerable<T> self)
        {
            return new ListyEnumerable<T>(self);
        }

        public struct ListyEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> possibleList;

            public ListyEnumerable(IEnumerable<T> possibleList)
            {
                this.possibleList = possibleList;
            }

            public ListyEnumerator<T> GetEnumerator()
            {
                return new ListyEnumerator<T>(this.possibleList);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return this.possibleList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.possibleList.GetEnumerator();
            }
        }

        public struct ListyEnumerator<T> : IEnumerator<T>
        {
            private List<T>.Enumerator list;

            private IEnumerator<T>? enumerable;

            public ListyEnumerator(IEnumerable<T> possibleList)
            {
                if (possibleList is List<T> list)
                {
                    this.list = list.GetEnumerator();
                }
                else
                {
                    this.enumerable = possibleList.GetEnumerator();
                }
            }

            public T Current
            {
                get
                {
                    if (this.enumerable == null)
                    {
                        return this.list.Current;
                    }
                    else
                    {
                        return this.enumerable.Current;
                    }
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                if (this.enumerable == null)
                {
                    this.list.Dispose();
                }
                else
                {
                    this.enumerable.Dispose();
                }
            }

            public bool MoveNext()
            {
                if (this.enumerable == null)
                {
                    return this.list.MoveNext();
                }
                else
                {
                    return this.enumerable.MoveNext();
                }
            }

            public void Reset()
            {
                if (this.enumerable != null)
                {
                    this.enumerable.Reset();
                }
            }
        }

        public static ListyEnumerable2<T> ToListy2<T>(this IEnumerable<T> self)
        {
            return new ListyEnumerable2<T>(self);
        }

        public struct ListyEnumerable2<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> possibleList;

            public ListyEnumerable2(IEnumerable<T> possibleList)
            {
                this.possibleList = possibleList;
            }

            public ListyEnumerator2<T> GetEnumerator()
            {
                if (this.possibleList is List<T> list)
                {
                    return new ListyEnumerator2<T>(list);
                }

                return new ListyEnumerator2<T>(this.possibleList);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return this.possibleList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.possibleList.GetEnumerator();
            }
        }

        public struct ListyEnumerator2<T> : IEnumerator<T>
        {
            private List<T>.Enumerator list;

            private IEnumerator<T> enumerable;

            public ListyEnumerator2(List<T> list)
            {
                this.list = list.GetEnumerator();
            }

            public ListyEnumerator2(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable.GetEnumerator();
            }

            public T Current
            {
                get
                {
                    return this.list.Current;
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                this.list.Dispose();
            }

            public bool MoveNext()
            {
                return this.list.MoveNext();
            }

            public void Reset()
            {
                if (this.enumerable != null)
                {
                    this.enumerable.Reset();
                }
            }
        }
    }

    internal class Program
    {
        static void DoWork(IEnumerable<string> data)
        {
            var count = 0;
            foreach (var element in data)
            {
                ++count;
            }
        }

        static void DoWork2(IEnumerable<string> data)
        {
            var list = data as List<string>;
            var count = 0;
            foreach (var element in list)
            {
                ++count;
            }
        }

        public struct TestEnumerator : IEnumerator<string>
        {
            private readonly IEnumerable<string> list;

            private int index;

            private readonly int version;

            private string? current;

            public TestEnumerator(IEnumerable<string> list)
            {
                this.list = list;
                this.index = -1;
                this.version = 5;
                this.current = default;
            }

            public string Current
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.current;
                        default:
                            throw new Exception();
                    }

                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                ++this.index;
                this.current = "asdf";
                return this.index == 0;
            }

            public void Reset()
            {
            }
        }

        static void DoWork3(IEnumerable<string> data)
        {
            var count = 0;
            var enumerator = new TestEnumerator(data);
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        static void DoWork4(IEnumerable<string> data)
        {
            var count = 0;
            foreach (var element in data.ToListy())
            {
                ++count;
            }
        }

        public struct TestEnumerator2 : IEnumerator<string>
        {
            private int index = -1;

            public TestEnumerator2()
            {
            }

            public string Current
            {
                get
                {
                    if (this.index != 0)
                    {
                        throw new Exception();
                    }

                    return "asdf";
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return ++this.index == 0;
            }

            public void Reset()
            {
            }
        }

        static void DoWork5(IEnumerable<string> data)
        {
            var count = 0;
            var enumerator = new TestEnumerator2();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        static void DoWork6(IEnumerable<string> data)
        {
            var count = 0;
            foreach (var element in data.ToListy2())
            {
                ++count;
            }
        }

        public sealed class TestEnumeratorButAClass : IEnumerator<string>
        {
            private readonly IEnumerable<string> list;

            private int index;

            private readonly int version;

            private string? current;

            public TestEnumeratorButAClass(IEnumerable<string> list)
            {
                this.list = list;
                this.index = -1;
                this.version = 5;
                this.current = default;
            }

            public string Current
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.current;
                        default:
                            throw new Exception();
                    }

                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                ++this.index;
                this.current = "asdf";
                return this.index == 0;
            }

            public void Reset()
            {
            }
        }

        static void DoWork7(IEnumerable<string> data)
        {
            var count = 0;
            var enumerator = new TestEnumeratorButAClass(data);
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        static void DoWork8(IEnumerable<string> data)
        {
            var count = 0;
            using (var enumerator = new TestEnumeratorButAClass(data))
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
        }

        public sealed class ListEnumeratorButAClass : IEnumerator<string>
        {
            private readonly List<string> list;

            private int index;

            private readonly int version;

            private string? current;

            public ListEnumeratorButAClass(List<string> list)
            {
                this.list = list;
                this.index = 0;
                this.version = 5;
                this.current = default;
            }

            public string Current
            {
                get
                {
                    if (this.index == 0 || this.index == this.list.Count + 1)
                    {
                        throw new Exception();
                    }

                    return this.current;
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.index < this.list.Count)
                {
                    this.current = this.list[this.index];
                    ++this.index;
                    return true;
                }
                
                return false;
            }

            public void Reset()
            {
            }
        }

        static void DoWork9(IEnumerable<string> data)
        {
            var list = data as List<string>;
            var count = 0;
            using (var enumerator = new ListEnumeratorButAClass(list))
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
        }

        public sealed class ListEnumeratorButBoxed : IEnumerator<string>
        {
            private readonly List<string>.Enumerator list;

            public ListEnumeratorButBoxed(List<string> list)
            {
                this.list = list.GetEnumerator();
            }

            public string Current
            {
                get
                {
                    return this.list.Current;
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return this.list.MoveNext();
            }

            public void Reset()
            {
            }
        }

        static void DoWork10(IEnumerable<string> data)
        {
            var list = data as List<string>;
            var count = 0;
            using (var enumerator = new ListEnumeratorButBoxed(list))
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    ++count;
                }
            }
        }

        static void Test()
        {
            var data = new List<string>
            {
                "asdf",
            };

            int iterations = 1000000;
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork2(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork3(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork4(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork5(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork6(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork7(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork8(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork9(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                DoWork9(data);
            }

            Console.WriteLine(stopwatch.ElapsedTicks);
            stopwatch = Stopwatch.StartNew();

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Test();

            //// TODO change font to 16
            var schema = new Schema(new[]
            {
                new Property(EdmTypeKind.Other, "asdf"),
            });

            int iterations = 1000000;
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork(schema);
            }

            var doWorkElapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWorkElapsed);
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork2(schema);
            }

            var doWork2Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork2Elapsed);
            Console.WriteLine($"2:1 {doWork2Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork3(schema);
            }

            var doWork3Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork3Elapsed);
            Console.WriteLine($"3:1 {doWork3Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork4(schema);
            }

            var doWork4Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork4Elapsed);
            Console.WriteLine($"4:1 {doWork4Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork5(schema);
            }

            var doWork5Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork5Elapsed);
            Console.WriteLine($"5:1 {doWork5Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork6(schema);
            }

            var doWork6Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork6Elapsed);
            Console.WriteLine($"6:1 {doWork6Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork7(schema);
            }

            var doWork7Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork7Elapsed);
            Console.WriteLine($"7:1 {doWork7Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork8(schema);
            }

            var doWork8Elapsed = stopwatch.ElapsedTicks;
            Console.WriteLine(doWork8Elapsed);
            Console.WriteLine($"8:1 {doWork8Elapsed / (double)doWorkElapsed}");
            Console.WriteLine();
            stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork9(schema);
            }

            stopwatch = Write(stopwatch, doWorkElapsed, 9);

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork10(schema);
            }

            stopwatch = Write(stopwatch, doWorkElapsed, 10);

            for (int i = 0; i < iterations; ++i)
            {
                Container.DoWork11(schema);
            }

            stopwatch = Write(stopwatch, doWorkElapsed, 11);
        }

        private static Stopwatch Write(Stopwatch stopwatch, long baseTicks, int variantIndex)
        {
            var variantTicks = stopwatch.ElapsedTicks;
            Console.WriteLine(variantTicks);
            Console.WriteLine($"{variantIndex}:1 {variantTicks / (double)baseTicks}");
            Console.WriteLine();
            return Stopwatch.StartNew();
        }
    }
}
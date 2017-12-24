using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WoaW.RnD.MMF
{
    public class InMemDb
    {
        public static void Save(object data)
        {
            //https://msdn.microsoft.com/en-us/library/c5sbs8z9(v=vs.110).aspx
            //https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter(v=vs.110).aspx
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/binary-serialization
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/serialization-guidelines
            FileStream fs = new FileStream("DataFile.dat", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

        }
        public static void Update()
        {
            long offset = 0; // 256 megabytes
            long length = 1024; // 512 megabytes

            // Create the memory-mapped file.
            var relativePath = @"..\..\..\ExtremelyLargeImage.data";
            var path = Path.GetFullPath(relativePath);
            using (var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open, "ImgA"))
            {
                // Create a random access view, from the 256th megabyte (the offset)
                // to the 768th megabyte (the offset plus length).
                using (var accessor = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.ReadWriteExecute))
                {
                    int colorSize = Marshal.SizeOf(typeof(MyColor));
                    MyColor color;

                    // Make changes to the view.
                    for (long i = 0; i < length; i += colorSize)
                    {
                        accessor.Read(i, out color);
                        color.Brighten(10);
                        accessor.Write(i, ref color);
                    }
                }
            }
        }
        public static void Write()
        {
            //https://blogs.msdn.microsoft.com/salvapatuel/2009/06/08/working-with-memory-mapped-files-in-net-4/

            var list = new List<MyColor>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(new MyColor());
            }
            int colorSize = Marshal.SizeOf(typeof(MyColor));

            long offset = 0;
            long length = colorSize * list.Count;

            // Create the memory-mapped file.
            var relativePath = @"..\..\..\ExtremelyLargeImage.data";
            var path = Path.GetFullPath(relativePath);
            using (var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.CreateNew, "ImgA", length))
            {
                // Create a random access view, from the 256th megabyte (the offset)
                // to the 768th megabyte (the offset plus length).
                using (var accessor = mmf.CreateViewAccessor(offset, length))
                {
                    //for (int i = 0; i < length; i += colorSize)
                    for (int i = 0; i < list.Count; ++i)
                    {
                        var color = list[i];
                        var addres = i * colorSize;
                        color.Brighten(10);
                        accessor.Write(addres, ref color);
                    }
                    accessor.Flush();
                }
            }
        }


        #region properties
        public string ConnectionString { get; set; }

        private int _recordSize { get; set; }
        private int _pageSize { get; set; }
        private string _path { get; set; }
        private string _fileName { get; set; }
        public Dictionary<Type, List<object>> _data;
        public LinkedList<ItemRecord> _records;
        #endregion

        #region constructors
        public InMemDb()
        {
            _data = new Dictionary<Type, List<object>>();
            _records = new LinkedList<ItemRecord>();

            _pageSize = 1024 * 1024; //(mb)
            _recordSize = Marshal.SizeOf(typeof(ItemRecord));
        }
        #endregion


        #region public API
        public void Connect()
        {
            ParceConnectionString();
        }
        public List<T> Get<T>()
        {
            List<object> setTemp = null;
            _data.TryGetValue(typeof(T), out setTemp);
            var set = setTemp.OfType<T>().ToList();
            return set;
        }

        public T Read<T>(string id)
        {
            List<object> setTemp = null;
            _data.TryGetValue(typeof(T), out setTemp);
            var set = setTemp.OfType<T>().ToList();
            return set[0];
        }
        public void Add<T>(T data)
        {
            var t = typeof(T);
            //var size = Marshal.SizeOf(data);
            var size = ConvertObjectToByteArray<T>(data);

            _records.AddLast(new ItemRecord()
            {
                //TypeName = t.AssemblyQualifiedName,
                Offset = _records.Count * _recordSize,
                PageNum = 1,
                Length = size.Length
            });
            var set = GetSet<T>();
            set.Add(data);
        }
        public void Update<T>(object data)
        {
            var type = data.GetType();
            GetSet<T>();
        }
        public void Delete(object data)
        {
        }
        #endregion

        #region implementation
        private List<T> GetSet<T>(bool i)
        {
            List<object> setTemp = null;
            var type = typeof(T);
            var r = _data.TryGetValue(type, out setTemp);
            if (r == false)
            {
                setTemp = new List<object>();
                _data.Add(typeof(T), setTemp);
            }
            var c = setTemp.OfType<T>();
            var set = new List<T>(c);
            return set;
        }
        private List<object> GetSet<T>()
        {
            var type = typeof(T);
            List<object> setTemp = null;
            var r = _data.TryGetValue(type, out setTemp);
            if (r == false)
            {
                setTemp = new List<object>();
                _data.Add(type, setTemp);
            }
            return setTemp;
        }
        public List<object> Load()
        {
            using (var mmf = MemoryMappedFile.CreateFromFile(_path, FileMode.Open ))
            {
                using (var accessor = mmf.CreateViewAccessor())
                {
                    byte[] memoryBytes = new byte[accessor.Capacity];
                    accessor.ReadArray(0, memoryBytes, 0, memoryBytes.Length);
                    _data = ConvertByteArrayToObject<Dictionary<Type, List<object>>>(memoryBytes);
                }
            }
            return null;
        }

        public void Save<T>()
        {
            var buffer = ConvertObjectToByteArray(_data);
            using (var mmf = MemoryMappedFile.CreateFromFile(_path, FileMode.OpenOrCreate, "ImgA", buffer.Length))
            {
                using (var accessor = mmf.CreateViewAccessor(0, buffer.Length, MemoryMappedFileAccess.ReadWrite))
                {
                    accessor.WriteArray(0, buffer, 0, buffer.Length);
                    accessor.Flush();
                }
            }
        }
        public void Save<T>(bool a)
        {
            //var length = 1024 * 1024 + 10;
            var set = GetSet<T>();

            var length = 0;
            //foreach (var item in set)
            //{
            //    length += Marshal.SizeOf(item);
            //}
            foreach (var item in _records)
            {
                length += item.Length;
            }

            using (var mmf = MemoryMappedFile.CreateFromFile(_path, FileMode.OpenOrCreate, "ImgA", length))
            {
                using (var accessor = mmf.CreateViewAccessor(0, length, MemoryMappedFileAccess.ReadWrite))
                {
                    for (int i = 0; i < set.Count; ++i)
                    {
                        var item = (T)set[i];
                        var buffer = ConvertObjectToByteArray(item);
                        int position = i * buffer.Length;
                        accessor.WriteArray(position, buffer, 0, buffer.Length);
                    }
                    accessor.Flush();
                }
            }
        }
        private void ParceConnectionString()
        {
            _path = Path.GetFullPath(ConnectionString);
        }

        //https://stackoverflow.com/questions/25153868/using-a-byte-array-to-get-a-dynamic-class-from-a-memory-map-file
        private byte[] ConvertObjectToByteArray<T>(T sourceObj)
        {
            byte[] byteArray = null;

            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, sourceObj);
                byteArray = stream.ToArray();
            }

            return byteArray;
        }
        private T ConvertByteArrayToObject<T>(byte[] sourceBytes)
        {
            object newObject = null;

            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(sourceBytes))
            {
                newObject = formatter.Deserialize(stream);
            }

            return (T)newObject;
        }

        #endregion
    }
}

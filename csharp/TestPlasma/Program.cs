using Apache.Arrow;
using Apache.Arrow.Ipc;
using Apache.Arrow.Plasma;
using Microsoft.Data.Analysis;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TestPlasma
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: TestPlasma {socketName} [create|read] [{ObjectIdHex}]");
                return;
            }

            string socketName = args[0];
            var client = new PlasmaClient(socketName);

            if (args[1] == "create")
            {
                ObjectId newId;
                string idString;
                if (args.Length == 3)
                {
                    newId = ObjectId.Create(args[2]);
                    idString = args[2];
                }
                else
                {
                    Span<byte> objectIdBuffer = stackalloc byte[20];
                    for (int i = 0; i < objectIdBuffer.Length; i++)
                    {
                        objectIdBuffer[i] = (byte)'a';
                    }
                    newId = ObjectId.Create(objectIdBuffer);
                    idString = BytesToHex(objectIdBuffer);
                }

                CreatePlasmaObject(client, newId);
                Console.WriteLine($"New object created. Id: {idString}");
                return;
            }

            if (args[1] == "read")
            {
                if (args.Length == 3)
                {
                    string objectIdHex = args[2];

                    ReadPlasmaObject(client, objectIdHex);
                    return;
                }
            }

            Console.WriteLine("Usage: TestPlasma {socketName} [create|read] [{ObjectIdHex}]");
        }

        private static void CreatePlasmaObject(PlasmaClient client, ObjectId objectId)
        {
            DataFrame df = DataFrame.LoadCsv("housing.csv");
            df.Columns.Remove("ocean_proximity");

            RecordBatch batch = df.ToArrowRecordBatches().First();

            MemoryStream stream = new MemoryStream();
            ArrowStreamWriter writer = new ArrowStreamWriter(stream, batch.Schema, leaveOpen: true);
            writer.WriteRecordBatchAsync(batch).Wait();

            stream.Position = 0;

            CreatedObject createdObject = client.CreateObject(objectId, stream.Length);
            ObjectStoreData createdData = createdObject.GetData();

            stream.GetBuffer()
                .AsSpan(0, (int)stream.Length)
                .CopyTo(createdData.Memory.Span);

            createdObject.Seal();
        }

        private static string BytesToHex(Span<byte> bytes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        private static void ReadPlasmaObject(PlasmaClient client, string objectIdHex)
        {
            var objId = ObjectId.Create(objectIdHex);

            var referredObject = client.ReferObject(objId, -1);
            Console.WriteLine(referredObject);

            using (ObjectStoreData data = referredObject.GetData())
            {
                Console.WriteLine(data.Memory.Length);
                ArrowStreamReader reader = new ArrowStreamReader(data.Memory);
                RecordBatch batch;
                while ((batch = reader.ReadNextRecordBatch()) != null)
                {
                    Console.WriteLine("Length: " + batch.Length);
                    Console.Write("Columns: ");
                    for (int i = 0; i < batch.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            Console.Write(", ");
                        }
                        Console.Write(batch.Schema.GetFieldByIndex(i).Name);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting");

            var myByte = new ByteArrayCreator();

            await myByte.WaitForDispose();

            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1);
                Thread.Sleep(6000);

                for(int i = 0; i < 15; i++)
                {
                    _ = Task.Factory.StartNew(async () =>
                    {
                        var myByte = new ByteArrayCreator();

                        await myByte.WaitForDispose();
                    });
                }
            });

            Console.ReadLine();
        }
    }

    public class ByteArrayCreator : IDisposable
    {
        private byte[] byteArray;
        private bool disposed;

        public ByteArrayCreator()
        {
            // Create a byte array of size 100 MB
            byteArray = new byte[100 * 1024 * 1024];

            // Fill the byte array with some data
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = 1;
            }

            Console.WriteLine("Byte array created and filled with data.");
        }

        public async Task WaitForDispose()
        {
            Console.WriteLine("Creating ByteArray");

            // Wait for 30 seconds
            await Task.Delay(TimeSpan.FromSeconds(5));

            Console.WriteLine("30 seconds have passed. The byte array can now be disposed of.");

            Dispose();
        }

        ~ByteArrayCreator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed objects
                byteArray = null;
            }

            // Dispose unmanaged objects
            disposed = true;
        }
    }


}

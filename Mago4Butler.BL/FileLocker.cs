using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    internal class FileLocker : IFileLocker
    {
        public ILockToken CreateLockToken(string pathToLock)
        {
            if (string.IsNullOrWhiteSpace(pathToLock))
            {
                throw new ArgumentNullException("pathToLock");
            }
            if (!File.Exists(pathToLock))
            {
                throw new ArgumentException(pathToLock + " does not exist");
            }
            return new LockToken(pathToLock);
        }
    }

    internal class LockToken : ILockToken
    {
        Stream readLockStream;

        public LockToken(string pathToLock)
        {
            this.readLockStream = File.OpenRead(pathToLock);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                if (readLockStream != null)
                {
                    readLockStream.Dispose();
                }
            }
        }
    }
}

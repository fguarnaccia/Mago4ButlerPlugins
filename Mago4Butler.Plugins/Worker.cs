using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public interface IWorker<T>
    {
        void OnRequestReceived(T currentRequest);
    }

    public static class WorkerExtensions
    {
        static Dictionary<object, object> instances = new Dictionary<object, object>();

        public static void Start<T>(this IWorker<T> @this)
        {
            var workerInstance = new WorkerInstance<T>(@this);
            workerInstance.Start();

            instances.Add(@this, workerInstance);
        }
        public static void Enqueue<T>(this IWorker<T> @this, T request)
        {
            object workerInstance = null;
            if (instances.TryGetValue(@this, out workerInstance))
            {
                (workerInstance as WorkerInstance<T>).Enqueue(request);
            }
        }
    }

    internal class WorkerInstance<T>
    {
        readonly object lockTicket = new object();
        Queue<T> requests = new Queue<T>();
        IWorker<T> worker;

        public WorkerInstance(IWorker<T> worker)
        {
            this.worker = worker;
        }

        internal void Start()
        {
            var thread = new Thread(() => WorkerThread());
            thread.IsBackground = true;
            thread.Start();
        }

        internal void Enqueue(T request)
        {
            lock (this.lockTicket)
            {
                this.requests.Enqueue(request);
            }
        }
        private T Dequeue()
        {
            lock (this.lockTicket)
            {
                if (this.requests.Count > 0)
                {
                    return this.requests.Dequeue();
                }

                return default(T);
            }
        }

        private void WorkerThread()
        {
            while (true)
            {
                var currentRequest = this.Dequeue();

                while (currentRequest == null)
                {
                    Thread.Sleep(1000);
                    currentRequest = this.Dequeue();
                }

                worker.OnRequestReceived(currentRequest);
                Thread.Sleep(1000);
            }
        }
    }
}

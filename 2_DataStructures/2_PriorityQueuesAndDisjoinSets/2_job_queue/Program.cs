using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_job_queue
{
    public class QItem
    {
        public int TaskId;
        public long TimeStart;
        public long TimeBusy;
        public int ThreadId;

        public long TimeEnd
        {
            get { return TimeStart + TimeBusy; }
        }
        public QItem(int taskId, long timeStart, long timeBusy, int threadId)
        {
            TaskId = taskId;
            TimeStart = timeStart;
            TimeBusy = timeBusy;
            ThreadId = threadId;
        }
    }

    public class PriorityQ
    {
        private readonly List<QItem> queue = new List<QItem>();

        private void Swap(int a, int b)
        {
            var buf = queue[a];
            queue[a] = queue[b];
            queue[b] = buf;
        }

        private void SiftUp(int index)
        {
            var nextI = index;
            do
            {
                index = nextI;
                var p = (index - 1) / 2;
                if (p >= 0 && (queue[index].TimeEnd < queue[p].TimeEnd ||
                    (queue[index].TimeEnd == queue[p].TimeEnd && queue[index].ThreadId < queue[p].ThreadId)))
                {
                    nextI = p;
                    Swap(index, nextI);
                }
            } while (nextI != index);
        }

        private void SiftDown(int index)
        {
            var nextI = index;
            do
            {
                index = nextI;
                var c1 = 2 * index + 1;
                var c2 = 2 * index + 2;

                if ((c1 < queue.Count && queue[index].TimeEnd >= queue[c1].TimeEnd) ||
                    (c2 < queue.Count && queue[index].TimeEnd >= queue[c2].TimeEnd))
                {
                    if (c2 >= queue.Count)
                    {
                        if (queue[c1].TimeEnd < queue[index].TimeEnd
                        || queue[c1].ThreadId < queue[index].ThreadId)
                        {
                            nextI = c1;
                        }
                    }
                    else
                    {
                        if (queue[c1].TimeEnd < queue[c2].TimeEnd)
                        {
                            nextI = c1;
                        }
                        else if (queue[c1].TimeEnd > queue[c2].TimeEnd)
                        {
                            nextI = c2;
                        }
                        else
                        {
                            nextI = queue[c1].ThreadId < queue[c2].ThreadId ? c1 : c2;
                        }
                    }
                    if (index != nextI) Swap(index, nextI);
                }
            } while (nextI != index);
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public void Enqueue(QItem item)
        {
            queue.Add(item);
            SiftUp(queue.Count - 1);
        }

        public QItem Dequeue()
        {
            var result = queue[0];
            queue[0] = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);
            SiftDown(0);
            return result;
        }
    }

    public static class JobQueue
    {
        public static string[] Solve(string line1, string line2)
        {
            var threadCount = int.Parse(line1.Split(' ')[0]);
            var tasks = line2.Split(' ').Select(x => long.Parse(x)).ToArray();
            var queue = new PriorityQ();
            var threadId = 0;
            var result = new string[tasks.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                if (threadId < threadCount)
                {
                    queue.Enqueue(new QItem(i, 0, tasks[i], threadId));
                    threadId++;
                }
                else
                {
                    var processed = queue.Dequeue();
                    result[processed.TaskId] = $"{processed.ThreadId} {processed.TimeStart}";
                    queue.Enqueue(new QItem(i, processed.TimeStart + processed.TimeBusy, tasks[i], processed.ThreadId));
                }
            }
            while (queue.Count > 0)
            {
                var processed = queue.Dequeue();
                result[processed.TaskId] = $"{processed.ThreadId} {processed.TimeStart}";
            }
            return result;
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            var result = Solve(line1, line2);
            foreach (var line in result)
            {
                Console.WriteLine(line);
            }
        }
    }
}

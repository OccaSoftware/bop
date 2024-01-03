namespace OccaSoftware.Bop.Runtime
{
    public class PoolStatistics
    {
        public int PoolSize { get; }
        public int PoolActiveCount { get; }
        public int PoolInactiveCount { get; }

        public PoolStatistics(Pooler pooler)
        {
            PoolActiveCount = GetPoolActiveCount(pooler);
            PoolInactiveCount = GetPoolInactiveCount(pooler);
            PoolSize = PoolActiveCount + PoolInactiveCount;
        }

        private int GetPoolActiveCount(Pooler pooler)
        {
            int c = 0;
            for (int a = 0; a < pooler.Pool.Count; a++)
            {
                if (pooler.Pool[a].IsActive() && pooler.Pool[a].GetObject() != null)
                {
                    c++;
                }
            }

            return c;
        }

        private int GetPoolInactiveCount(Pooler pooler)
        {
            int c = 0;
            for (int a = 0; a < pooler.Pool.Count; a++)
            {
                if (!pooler.Pool[a].IsActive() && pooler.Pool[a].GetObject() != null)
                {
                    c++;
                }
            }

            return c;
        }
    }
}

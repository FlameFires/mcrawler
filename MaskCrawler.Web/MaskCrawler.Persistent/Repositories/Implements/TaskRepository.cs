using MaskCrawler.Models.Domain;
using MaskCrawler.Persistent.Infrastructure;

namespace MaskCrawler.Persistent.Repositories.Implements
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(IDatabaseAdapter database) : base(database)
        {
        }
    }
}

using Dapper.FluentMap.Mapping;

using MaskCrawler.Models.Domain;

namespace MaskCrawler.Models.Maps
{
    public class TaskMap : EntityMap<TaskEntity>
    {
        public TaskMap()
        {
            //ToTable("task");

            Map(p => p.TaskId)
                //.IsKey()
                .ToColumn("tid");

            Map(p => p.TaskParentId)
                .ToColumn("tpid");

            Map(p => p.TaskName)
                .ToColumn("name");

            Map(p => p.TaskDescribe)
                .ToColumn("describe");

            Map(p => p.ResponseType)
                .ToColumn("resType");
        }
    }
}

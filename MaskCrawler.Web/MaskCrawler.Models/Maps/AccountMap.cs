using Dapper.FluentMap.Mapping;

using MaskCrawler.Models.Domain;

namespace MaskCrawler.Models.Maps
{
    public class AccountMap : EntityMap<AccountEntity>
    {
        public AccountMap()
        {
            // 这个配置不会生效,因为没有使用 FluentMap进行查询
            // 内部使用的是 SimpleCRUD


            Map(p => p.Gid)
                .ToColumn("id");

            // Map property 'Name' to column 'strName'.
            Map(p => p.Name)
                .ToColumn("name");

            // Map property 'Name' to column 'strName'.
            Map(p => p.Pwd)
                .ToColumn("password");
        }
    }
}

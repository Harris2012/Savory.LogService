using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.LogService.Repository.Entity
{
    public class LogEventEntity
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int AppId { get; set; }

        public int LogTypeId { get; set; }

        public string Source { get; set; }

        public DateTime CreateTime { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}

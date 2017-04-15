using System.Collections.Generic;

namespace AW.Application.DtoDefault
{
    public class HeaderEntity
    {
        public string Field { get; set; }

        public string Title { get; set; }

        public string DisplayName { get; set; }

        public bool Visible { get; set; }

        public bool IsFirst { get; set; }

        public bool EnableSorting { get; set; }

        public string Type { get; set; }

        public string Width { get; set; }

        public string BaseUrl { get; set; }

        public Dictionary<string, string> Filter { get; set; }

        public HeaderEntity(string field)
        {
            Field = field;
        }
    }
}
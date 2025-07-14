using System.Collections.Generic;

namespace BackendProjects.DataExporter
{
    public interface IDataExporter
    {
        public string Export<T>(IEnumerable<T> data);
    }
}

using System.Collections.Generic;

namespace ASBMessageTool.Model;

public interface IServiceBusMessageApplicationPropertiesHolder
{
    void RemoveEmptyProperties();
    IList<string> GetDuplicatedApplicationProperties();
}

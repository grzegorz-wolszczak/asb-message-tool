using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using Core.Maybe;

namespace ASBMessageTool.SendingMessages.Code;

public interface ISenderSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusMessageSendData settings, CancellationToken token);
}

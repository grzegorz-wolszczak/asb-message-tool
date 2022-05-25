using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using Core.Maybe;

namespace ASBMessageTool.ReceivingMessages.Code;

public interface IReceiverSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusReceiverSettings settings, CancellationToken token);
}

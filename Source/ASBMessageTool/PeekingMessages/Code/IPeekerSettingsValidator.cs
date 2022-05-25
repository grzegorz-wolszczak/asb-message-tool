using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using Core.Maybe;

namespace ASBMessageTool.PeekingMessages.Code;

public interface IPeekerSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusPeekerSettings settings, CancellationToken token);
}

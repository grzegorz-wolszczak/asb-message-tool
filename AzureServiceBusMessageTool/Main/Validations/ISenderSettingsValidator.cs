using System.Threading;
using System.Threading.Tasks;
using Core.Maybe;
using Main.ViewModels.Configs.Senders;

namespace Main.Validations;

public interface ISenderSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusMessageSendData settings, CancellationToken token);
}

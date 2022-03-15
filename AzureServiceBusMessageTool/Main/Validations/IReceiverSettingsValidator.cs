using System.Threading;
using System.Threading.Tasks;
using Core.Maybe;
using Main.ViewModels.Configs.Receivers;

namespace Main.Validations;

public interface IReceiverSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusReceiverSettings settings, CancellationToken token);
}

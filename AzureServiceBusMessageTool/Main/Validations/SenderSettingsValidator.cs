using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;
using Main.ViewModels.Configs.Senders;

namespace Main.Validations;

public class SenderSettingsValidator : ISenderSettingsValidator
{
    public async Task<Maybe<ValidationErrorResult>> Validate(
        ServiceBusMessageSendData settings,
        CancellationToken token)
    {
        var sbClient = new ServiceBusAdministrationClient(settings.ConnectionString);
        var queueOrTopicName = settings.QueueOrTopicName;
        var topicValidationResult = await ServiceBusValidations.ValidateTopic(sbClient, queueOrTopicName, token);
        if (topicValidationResult.IsNothing())
        {
            return topicValidationResult;
        }

        return await ServiceBusValidations.ValidateQueue(sbClient, queueOrTopicName, token);
    }
}

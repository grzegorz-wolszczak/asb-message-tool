using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;

namespace ASBMessageTool.SendingMessages;

public class SenderSettingsValidator : ISenderSettingsValidator
{
    public async Task<Maybe<ValidationErrorResult>> Validate(
        ServiceBusMessageSendData settings,
        CancellationToken token)
    {
        var sbClient = new ServiceBusAdministrationClient(settings.ConnectionString);
        var queueOrTopicName = settings.QueueOrTopicName;
        return await ServiceBusValidations.ValidateQueueOrTopicName($"config name: {settings.ConfigName}", // context 
            sbClient, queueOrTopicName, token);
    }
}

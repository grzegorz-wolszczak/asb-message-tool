using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;
using Main.ViewModels.Configs.Senders;

namespace Main.Validations;

public class SenderSettingsValidator : ISenderSettingsValidator
{
    public async Task<Maybe<ValidationErrorResult>> Validate(ServiceBusMessageSendData settings, CancellationToken token)
    {
        var sbClient = new ServiceBusAdministrationClient(settings.ConnectionString);
        if (string.IsNullOrWhiteSpace(settings.TopicName))
        {
            return new ValidationErrorResult("Topic name is null or whitespace").ToMaybe();
        }
        try
        {
            await sbClient.GetTopicAsync(settings.TopicName, token);
        }
        catch (ServiceBusException e)
        {
            if (e.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
            {
                return new ValidationErrorResult($"Topic '{settings.TopicName}' does not exist").ToMaybe();
            }

            return new ValidationErrorResult($"ServiceBus exception happened: {e}").ToMaybe();
        }
        
        return Maybe<ValidationErrorResult>.Nothing;
    }
}

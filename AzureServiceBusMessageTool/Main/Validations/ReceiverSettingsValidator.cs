using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;
using Main.Application.Logging;
using Main.ViewModels.Configs.Receivers;

namespace Main.Validations;

public class ReceiverSettingsValidator : IReceiverSettingsValidator
{
    private readonly IServiceBusHelperLogger _logger;

    public ReceiverSettingsValidator(IServiceBusHelperLogger logger)
    {
        _logger = logger;
    }

    public async Task<Maybe<ValidationErrorResult>> Validate(ServiceBusReceiverSettings settings, CancellationToken token)
    {
        var sbClient = new ServiceBusAdministrationClient(settings.ConnectionString);
        if (string.IsNullOrWhiteSpace(settings.TopicName))
        {
            return new ValidationErrorResult("Topic name is null or whitespace").ToMaybe();
        }

        if (string.IsNullOrWhiteSpace(settings.SubscriptionName))
        {
            return new ValidationErrorResult("Subscription is null or whitespace").ToMaybe();
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

        var subscriptionExists = await sbClient.SubscriptionExistsAsync(settings.TopicName, settings.SubscriptionName, token);
        if (false == subscriptionExists)
        {
            return new ValidationErrorResult($"Subscription '{settings.SubscriptionName}' in topic '{settings.TopicName}' does not exist").ToMaybe();
        }

        return Maybe<ValidationErrorResult>.Nothing;
    }
}

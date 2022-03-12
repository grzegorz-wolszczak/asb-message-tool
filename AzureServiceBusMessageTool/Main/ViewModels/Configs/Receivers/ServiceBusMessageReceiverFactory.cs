using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;
using Main.Application.Logging;

namespace Main.ViewModels.Configs.Receivers;

public class ServiceBusMessageReceiverFactory
{
    private readonly IServiceBusHelperLogger _logger;

    public ServiceBusMessageReceiverFactory(IServiceBusHelperLogger logger)
    {
        _logger = logger;
    }

    public IServiceBusMessageReceiver Create()
    {
        var receiverSettingsValidator = new ReceiversSettingsValidator(_logger);
        return new ServiceBusMessageReceiver(_logger, receiverSettingsValidator);
    }
}

public class ReceiversSettingsValidator : IReceiverSettingsValidator
{
    private readonly IServiceBusHelperLogger _logger;

    public ReceiversSettingsValidator(IServiceBusHelperLogger logger)
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

public interface IReceiverSettingsValidator
{
    Task<Maybe<ValidationErrorResult>> Validate(ServiceBusReceiverSettings settings, CancellationToken token);
}

public record ValidationErrorResult(string ErrorMsg);

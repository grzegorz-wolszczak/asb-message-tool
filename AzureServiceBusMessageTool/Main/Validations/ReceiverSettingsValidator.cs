using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;
using Main.Application.Logging;
using Main.ExceptionHandling;
using Main.Models;
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
        if (settings.ReceiverDataSourceType == ReceiverDataSourceType.Topic)
        {
            return await ValidateTopicWithSubscriptionConfiguration(sbClient, settings.TopicName, settings.SubscriptionName, token);
        }

        if (settings.ReceiverDataSourceType == ReceiverDataSourceType.Queue)
        {
            return await ValidateQueueConfiguration(settings, sbClient, token);
        }

        throw new AsbMessageToolException($"Internal error: unhandled enum value '{settings.ReceiverDataSourceType}'");
    }

    private async Task<Maybe<ValidationErrorResult>> ValidateQueueConfiguration(ServiceBusReceiverSettings settings,
        ServiceBusAdministrationClient sbClient,
        CancellationToken token)
    {
        return await ServiceBusValidations.ValidateQueue(sbClient, settings.ReceiverQueueName, token);
    }


    private static async Task<Maybe<ValidationErrorResult>> ValidateTopicWithSubscriptionConfiguration(ServiceBusAdministrationClient sbClient,
        string topicName,
        string subscriptionName,
        CancellationToken token)
    {
        var topicValidationResult = await ServiceBusValidations.ValidateTopic(sbClient, topicName, token);
        if (topicValidationResult.HasValue)
        {
            return topicValidationResult;
        }

        return await ServiceBusValidations.ValidateSubscription(sbClient, topicName, subscriptionName, token);
    }
}

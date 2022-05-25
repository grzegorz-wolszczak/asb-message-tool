using System;
using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Model;
using ASBMessageTool.ReceivingMessages.Code;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerSettingsValidator : IPeekerSettingsValidator
{
    private readonly IServiceBusHelperLogger _logger;

    public PeekerSettingsValidator(IServiceBusHelperLogger logger)
    {
        _logger = logger;
        
    }


    
    public async Task<Maybe<ValidationErrorResult>> Validate(ServiceBusPeekerSettings settings, CancellationToken token)
    {
        var sbClient = new ServiceBusAdministrationClient(settings.ConnectionString);
        if (settings.ReceiverDataSourceType == ReceiverDataSourceType.Topic)
        {
            return await ValidateTopicWithSubscriptionConfiguration(
                $"config name: {settings.ConfigName}" , // context
                sbClient, settings.TopicName, settings.SubscriptionName, token);
        }

        if (settings.ReceiverDataSourceType == ReceiverDataSourceType.Queue)
        {
            return await ValidateQueueConfiguration(settings, sbClient, token);
        }

        throw new AsbMessageToolException($"Internal error: unhandled enum value '{settings.ReceiverDataSourceType}'");
    }

    private async Task<Maybe<ValidationErrorResult>> ValidateQueueConfiguration(
        ServiceBusPeekerSettings settings,
        ServiceBusAdministrationClient sbClient,
        CancellationToken token)
    {
        return await ServiceBusValidations.ValidateQueue($"config name: {settings.ConfigName}", // context 
            sbClient, settings.ReceiverQueueName, token);
    }


    private static async Task<Maybe<ValidationErrorResult>> ValidateTopicWithSubscriptionConfiguration(
        string context,
        ServiceBusAdministrationClient sbClient,
        string topicName,
        string subscriptionName,
        CancellationToken token)
    {
        var topicValidationResult = await ServiceBusValidations.ValidateTopic(context, sbClient, topicName, token);
        if (topicValidationResult.HasValue)
        {
            return topicValidationResult;
        }

        return await ServiceBusValidations.ValidateSubscription(context, sbClient, topicName, subscriptionName, token);
    }
}

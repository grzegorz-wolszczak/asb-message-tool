using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;

namespace Main.Validations;

public static class ServiceBusValidations
{
    public static async Task<Maybe<ValidationErrorResult>> ValidateTopic(
        ServiceBusAdministrationClient sbClient,
        string topicName,
        CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(topicName))
        {
            return new ValidationErrorResult("Topic name is null or whitespace").ToMaybe();
        }

        try
        {
            if (!await sbClient.TopicExistsAsync(topicName, token))
            {
                // topic is not found but
                // check if there is a queue with that name
                var optionalInfo = "";
                if (await sbClient.QueueExistsAsync(topicName, token))
                {
                    optionalInfo = " (but queue with that name exists!)";
                }

                return new ValidationErrorResult($"Topic '{topicName}' does not exist{optionalInfo}.").ToMaybe();
            }
        }
        catch (ServiceBusException e)
        {
            return new ValidationErrorResult($"ServiceBus exception happened (invalid connection string?): {e}").ToMaybe();
        }

        return Maybe<ValidationErrorResult>.Nothing;
    }

    public static async Task<Maybe<ValidationErrorResult>> ValidateSubscription(
        ServiceBusAdministrationClient sbClient,
        string topicName,
        string subscriptionName,
        CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(subscriptionName))
        {
            return new ValidationErrorResult("Subscription is null or whitespace").ToMaybe();
        }

        if (!await sbClient.SubscriptionExistsAsync(topicName, subscriptionName, token))
        {
            return new ValidationErrorResult($"Subscription '{subscriptionName}' for topic '{topicName}' does not exist").ToMaybe();
        }

        return Maybe<ValidationErrorResult>.Nothing;
    }

    public static async Task<Maybe<ValidationErrorResult>> ValidateQueue(
        ServiceBusAdministrationClient sbClient,
        string queueName,
        CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            return new ValidationErrorResult("Queue name is null or whitespace").ToMaybe();
        }

        try
        {
            if (await sbClient.QueueExistsAsync(queueName, token))
            {
                return Maybe<ValidationErrorResult>.Nothing;
            }

            // queue is not found but
            // check if there is a topic with that name
            var optionalTopicInfo = "";
            if (await sbClient.TopicExistsAsync(queueName, token))
            {
                optionalTopicInfo = " (but topic with that name exists!)";
            }

            return new ValidationErrorResult($"Queue '{queueName}' does not exist{optionalTopicInfo}.").ToMaybe();
        }
        catch (Exception e)
        {
            return new ValidationErrorResult($"ServiceBus exception happened (invalid connection string?): {e}").ToMaybe();
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Core.Maybe;

namespace ASBMessageTool.Application;

public static class ServiceBusValidations
{
    public static async Task<Maybe<ValidationErrorResult>> ValidateTopic(
        string context, // e.g. config name
        ServiceBusAdministrationClient sbClient,
        string topicName,
        CancellationToken token)
    {
        var errorMessagePrefix = $"{context}: While validating topic name '{topicName}'";
        if (string.IsNullOrWhiteSpace(topicName))
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: topic name is null or whitespace").ToMaybe();
        }

        try
        {
            if (!await sbClient.TopicExistsAsync(topicName, token))
            {
                // topic is not found but
                // check if there is a queue with that name
                var optionalInfo = string.Empty;
                if (await sbClient.QueueExistsAsync(topicName, token))
                {
                    optionalInfo = " (but queue with that name exists!)";
                }

                return new ValidationErrorResult($"{errorMessagePrefix}: topic does not exist{optionalInfo}.").ToMaybe();
            }
        }
        catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
        {
            throw;
        }
        catch (ServiceBusException e)
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: exception happened (invalid connection string?): {e}").ToMaybe();
        }

        return Maybe<ValidationErrorResult>.Nothing;
    }

    public static async Task<Maybe<ValidationErrorResult>> ValidateSubscription(
        string context,
        ServiceBusAdministrationClient sbClient,
        string topicName,
        string subscriptionName,
        CancellationToken token)
    {
        var errorMessagePrefix = $"{context}: while validating subscription name '{subscriptionName}' for topic name '{topicName}'";
        if (string.IsNullOrWhiteSpace(subscriptionName))
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: subscription name is null or whitespace").ToMaybe();
        }

        if (!await sbClient.SubscriptionExistsAsync(topicName, subscriptionName, token))
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: subscription '{subscriptionName}' for topic '{topicName}' does not exist").ToMaybe();
        }

        return Maybe<ValidationErrorResult>.Nothing;
    }

    public static async Task<Maybe<ValidationErrorResult>> ValidateQueue(
        string context,
        ServiceBusAdministrationClient sbClient,
        string queueName,
        CancellationToken token)
    {
        var errorMessagePrefix = $"{context}: while validating queue name '{queueName}'";
        if (string.IsNullOrWhiteSpace(queueName))
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: queue name is null or whitespace").ToMaybe();
        }

        try
        {
            if (await sbClient.QueueExistsAsync(queueName, token))
            {
                return Maybe<ValidationErrorResult>.Nothing;
            }

            // queue is not found but
            // check if there is a topic with that name
            var optionalTopicInfo = string.Empty;
            if (await sbClient.TopicExistsAsync(queueName, token))
            {
                optionalTopicInfo = " (but topic with that name exists!)";
            }

            return new ValidationErrorResult($"{errorMessagePrefix}: queue does not exist{optionalTopicInfo}.").ToMaybe();
        }
        catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: exception happened (invalid connection string?): {e}").ToMaybe();
        }
    }

    public static async Task<Maybe<ValidationErrorResult>> ValidateQueueOrTopicName(
        string context,
        ServiceBusAdministrationClient sbClient,
        string queueOrTopicName,
        CancellationToken token)
    {
        var errorMessagePrefix = $"{context}: while validating queue/topic name '{queueOrTopicName}'";
        if (string.IsNullOrWhiteSpace(queueOrTopicName))
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: queue/topic name is null or whitespace").ToMaybe();
        }

        try
        {
            var queueExists = await sbClient.QueueExistsAsync(queueOrTopicName, token);
            var topicExists = await sbClient.TopicExistsAsync(queueOrTopicName, token);
            if (queueExists || topicExists)
            {
                return Maybe<ValidationErrorResult>.Nothing;
            }

            return new ValidationErrorResult($"{errorMessagePrefix}: neither queue nor topic with that name exist").ToMaybe();
        }
        catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            return new ValidationErrorResult($"{errorMessagePrefix}: exception happened (invalid connection string?): {e}")
                .ToMaybe();
        }
    }
}

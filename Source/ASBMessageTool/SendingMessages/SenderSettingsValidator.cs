using System;
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
        var connectionString = settings.ConnectionString;
        try
        {
            var sbClient = new ServiceBusAdministrationClient(connectionString);
            var queueOrTopicName = settings.QueueOrTopicName;

            return await ServiceBusValidations.ValidateQueueOrTopicName($"config name: '{settings.ConfigName}'",  
                sbClient, queueOrTopicName, token);
        }
        catch (Exception e)
        {
            var errorMsg = $"While validating config name: '{settings.ConfigName}', exception happened:\n{e}";
            return new ValidationErrorResult(errorMsg).ToMaybe();
        }
        
    }
}

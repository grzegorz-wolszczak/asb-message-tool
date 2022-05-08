using System;
using System.Linq;
using ASBMessageTool.Application;
using ASBMessageTool.Model;

namespace ASBMessageTool.ReceivingMessages.Gui;

public static class MessageApplicationPropertiesVerifications
{
    public static void WarnUserOnDuplicatesExistence(
        IServiceBusMessageApplicationPropertiesHolder viewModel,
        Action onNoDuplicatesAction)
    {
        if (viewModel is null)
        {
            return;
        }
        
        viewModel.RemoveEmptyProperties();
        var duplicatedPropertyNames = viewModel.GetDuplicatedApplicationProperties();
        if (duplicatedPropertyNames.Count > 0)
        {
            // todo: this code is duplicated may times // refactoring
            var duplicatedNames = string.Join(", ", duplicatedPropertyNames.Select(e => $"\"{e}\""));
            var errorMessage = "Property names must be unique.\n" +
                                               $"Found following duplicate names:\n\n{duplicatedNames}" +
                                               "\n\nRemove duplicates or rename them.";
            UserInteractions.ShowErrorDialog("Duplicate property names found!", errorMessage);
            return;
        }

        onNoDuplicatesAction();
    }
}

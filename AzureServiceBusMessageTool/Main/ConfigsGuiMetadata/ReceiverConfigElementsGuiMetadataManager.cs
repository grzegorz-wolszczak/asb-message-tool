using System.Collections.Generic;
using Main.ExceptionHandling;
using Main.ViewModels.Configs.Receivers;

namespace Main.ConfigsGuiMetadata
{
   public class ReceiverConfigElementsGuiMetadataManager : IReceiverConfigWindowDetacher
   {
      private readonly Dictionary<ReceiverConfigViewModelWrapper, ReceiverConfigElementGuiRepresentation> _dict = new();

      public void Add(ReceiverConfigViewModelWrapper newConfig)
      {
         var newMetadata = new ReceiverConfigElementGuiRepresentation();
         _dict.Add(newConfig, newMetadata);
      }

      public void Remove(ReceiverConfigViewModelWrapper currentSelectedItem)
      {
         _dict.Remove(currentSelectedItem);
      }


      public void Delete(ReceiverConfigViewModelWrapper currentSelectedItem)
      {
         _dict.TryGetValue(currentSelectedItem, out ReceiverConfigElementGuiRepresentation metadata);
         if (metadata == null)
         {
            throw new AsbMessageToolException("Could not find metadata for Receiver config");
         }

         metadata.CloseWindowOnElementDelete();
         Remove(currentSelectedItem);
      }

      public void DetachFromPanel(ReceiverConfigViewModelWrapper currentSelectedItem)
      {
         _dict.TryGetValue(currentSelectedItem, out ReceiverConfigElementGuiRepresentation metadata);
         if (metadata == null)
         {
            throw new AsbMessageToolException("Could not find metadata for Receiver config");
         }

         metadata.ShowCorrespondingElementWindow(currentSelectedItem);
         currentSelectedItem.CurrentSelectedConfigModelItem.IsEmbeddedInsideRightPanel = false;
      }
   }
}

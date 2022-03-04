using System;
using System.Collections.Generic;
using Main.ViewModels.Configs.Senders;

namespace Main.ConfigsGuiMetadata;

public class SenderConfigElementsGuiMetadataManager : ISenderConfigWindowDetacher
{
   private readonly Dictionary<SenderConfigViewModelWrapper, SenderConfigElementGuiRepresentation> _dict = new();

   public void Add(SenderConfigViewModelWrapper newConfig)
   {
      var newMetadata = new SenderConfigElementGuiRepresentation();
      _dict.Add(newConfig, newMetadata);
   }

   public void Remove(SenderConfigViewModelWrapper currentSelectedItem)
   {
      _dict.Remove(currentSelectedItem);
   }

   public void DetachFromPanel(SenderConfigViewModelWrapper currentSelectedItem)
   {
      _dict.TryGetValue(currentSelectedItem, out SenderConfigElementGuiRepresentation metadata);
      if (metadata == null)
      {
         throw new ApplicationException("Could not find metadata for sender config");
      }

      metadata.ShowCorrespondingElementWindow(currentSelectedItem);
      currentSelectedItem.CurrentSelectedConfigModelItem.IsEmbeddedInsideRightPanel = false;

   }

   public void Delete(SenderConfigViewModelWrapper currentSelectedItem)
   {
      _dict.TryGetValue(currentSelectedItem, out SenderConfigElementGuiRepresentation metadata);
      if (metadata == null)
      {
         throw new ApplicationException("Could not find metadata for sender config");
      }

      metadata.CloseWindowOnElementDelete();
      Remove(currentSelectedItem);
   }
}

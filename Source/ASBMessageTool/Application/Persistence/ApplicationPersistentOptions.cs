using System.Collections.Generic;
using System.Linq;
using ASBMessageTool.Model;
using ASBMessageTool.ReceivingMessages;
using ASBMessageTool.SendingMessages;

namespace ASBMessageTool.Application.Persistence;

public class ApplicationPersistentOptions
{
    private readonly MainViewModel _mainViewModel;
    private readonly SendersConfigs _sendersConfigs;
    private readonly ReceiversConfigsViewModel _receiversConfigsViewModel;
    private readonly LeftRightTabsSyncViewModel _leftRightTabsSyncViewModel;

    public ApplicationPersistentOptions(MainViewModel mainViewModel,
        SendersConfigs sendersConfigs,
        ReceiversConfigsViewModel receiversConfigsViewModel, 
        LeftRightTabsSyncViewModel leftRightTabsSyncViewModel)
    {
        _mainViewModel = mainViewModel;
        _sendersConfigs = sendersConfigs;
        _receiversConfigsViewModel = receiversConfigsViewModel;
        _leftRightTabsSyncViewModel = leftRightTabsSyncViewModel;
    }

    public MainWindowSettings GetMainWindowSettings()
    {
        return new MainWindowSettings
        {
            ShouldScrollToEndOnLogContentChange = _mainViewModel.ShouldScrollToEndOnLogContentChange,
            ShouldWordWrapLogContent = _mainViewModel.ShouldWordWrapLogContent,
            LogTextBoxFontSize = _mainViewModel.LogTextBoxFontSize,
            SelectedLeftPanelTabIndex = _leftRightTabsSyncViewModel.SelectedLeftPanelTabIndex
        };
    }

    public void ReadMainWindowSettings(MainWindowSettings mainWindowSettings)
    {
        if (mainWindowSettings == null) return;
        _mainViewModel.ShouldScrollToEndOnLogContentChange = mainWindowSettings.ShouldScrollToEndOnLogContentChange;
        _mainViewModel.ShouldWordWrapLogContent = mainWindowSettings.ShouldWordWrapLogContent;
        _mainViewModel.LogTextBoxFontSize = mainWindowSettings.LogTextBoxFontSize;
        _leftRightTabsSyncViewModel.SelectedLeftPanelTabIndex = mainWindowSettings.SelectedLeftPanelTabIndex; 
    }

    public List<ReceiverConfigModel> GetReceiversConfigToStore()
    {
        return _receiversConfigsViewModel.ReceiversConfigsVMs.Select(e => e.ModelItem).ToList();
    }

    public List<SenderConfigModel> GetSendersConfigToStore()
    {
        return _sendersConfigs.SendersConfigsVMs.Select(e => e.ModelItem).ToList();
    }

    public void ReadSendersConfigSettings(List<SenderConfigModel> configs)
    {
        if (configs is null) return;
        foreach (var item in configs)
        {
            _sendersConfigs.AddNewForModelItem(item);
        }
    }

    public void ReadReceiversConfigSettings(List<ReceiverConfigModel> configs)
    {
        if (configs is null) return;
        foreach (var item in configs)
        {
            _receiversConfigsViewModel.AddNewForModelItem(item);
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using Main.Models;
using Main.ViewModels;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.Application.Persistence;

public class ApplicationPersistentOptions
{
    private readonly MainViewModel _mainViewModel;
    private readonly ServiceBusSelectedConfigsViewModel _serviceBusConfigsViewModel;
    private readonly SendersSelectedConfigViewModel _sendersSelectedConfigViewModel;
    private readonly ReceiversSelectedConfigViewModel _receiversSelectedConfigViewModel;
    private readonly LeftPanelControlViewModel _leftPanelControlViewModel;

    public ApplicationPersistentOptions(
        MainViewModel mainViewModel,
        ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel,
        SendersSelectedConfigViewModel sendersSelectedConfigViewModel,
        ReceiversSelectedConfigViewModel receiversSelectedConfigViewModel,
        LeftPanelControlViewModel leftPanelControlViewModel)
    {
        _mainViewModel = mainViewModel;
        _serviceBusConfigsViewModel = serviceBusConfigsViewModel;
        _sendersSelectedConfigViewModel = sendersSelectedConfigViewModel;
        _receiversSelectedConfigViewModel = receiversSelectedConfigViewModel;
        _leftPanelControlViewModel = leftPanelControlViewModel;
    }

    public List<ServiceBusConfigModel> GetServiceBusConfigsToStore()
    {
        return _serviceBusConfigsViewModel.ServiceBusConfigs.Select(e => e).ToList();
    }

    public void ReadServiceBusConfigsFrom(List<ServiceBusConfigModel> settingsServiceBusConfigs)
    {
        if (settingsServiceBusConfigs == null) return;
        _serviceBusConfigsViewModel.AddConfigs(settingsServiceBusConfigs);
    }

    public List<SenderConfigModel> GetSendersConfigsToStore()
    {
        return _sendersSelectedConfigViewModel.SendersConfigs.Select(e => e.Item).ToList();
    }

    public void ReadSendersConfigsFrom(List<SenderConfigModel> settingsSendersConfig)
    {
        if (settingsSendersConfig == null) return;
        _sendersSelectedConfigViewModel.AddConfigs(settingsSendersConfig);
    }

    public List<ReceiverConfigModel> GetReceiversConfigsToStore()
    {
        return _receiversSelectedConfigViewModel.ReceiversConfigs.Select(e => e.Item).ToList();
    }

    public void ReadReceiversConfigFrom(List<ReceiverConfigModel> settingsReceiversConfig)
    {
        if (settingsReceiversConfig == null) return;
        _receiversSelectedConfigViewModel.AddConfigs(settingsReceiversConfig);
    }

    public MainWindowSettings GetMainWindowSettings()
    {
        return new MainWindowSettings
        {
            ShouldScrollToEndOnLogContentChange = _mainViewModel.ShouldScrollToEndOnLogContentChange,
            ShouldWordWrapLogContent = _mainViewModel.ShouldWordWrapLogContent,
            LogTextBoxFontSize = _mainViewModel.LogTextBoxFontSize,
            SelectedLeftPanelTabIndex = _leftPanelControlViewModel.SelectedLeftPanelTabIndex
        };
    }

    public void ReadMainWindowSettings(MainWindowSettings mainWindowSettings)
    {
        if (mainWindowSettings == null) return;
        _mainViewModel.ShouldScrollToEndOnLogContentChange = mainWindowSettings.ShouldScrollToEndOnLogContentChange;
        _mainViewModel.ShouldWordWrapLogContent = mainWindowSettings.ShouldWordWrapLogContent;
        _mainViewModel.LogTextBoxFontSize = mainWindowSettings.LogTextBoxFontSize;
        _leftPanelControlViewModel.SelectedLeftPanelTabIndex = mainWindowSettings.SelectedLeftPanelTabIndex;
    }
}

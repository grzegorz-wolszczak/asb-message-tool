using ASBMessageTool.Application.Config;

namespace ASBMessageTool.Application.Persistence;

public class MainWindowSettings
{
    public bool ShouldScrollToEndOnLogContentChange { get; set; }
    public bool ShouldWordWrapLogContent { get; set; }
    public int LogTextBoxFontSize { get; set; } = AppDefaults.DefaultTextBoxFontSize;
    
    public int SelectedLeftPanelTabIndex { get; set; }
}

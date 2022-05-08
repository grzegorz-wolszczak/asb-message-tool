using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ASBMessageTool.Application;
using ASBMessageTool.Model;
using ASBMessageTool.SendingMessages;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Numbers;
using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace UnitTests.ViewModel.Main;

public class MainViewModelTests
{
    private MainViewModel _mainViewModel;

    [SetUp]
    public void SetUp()
    {
        var aboutWindowProxy = Substitute.For<IAboutWindowProxy>();
        var senderConfigModels = new ObservableCollection<SenderConfigModel>();
        _mainViewModel = new MainViewModel(aboutWindowProxy);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataInteger))]
    public void ShouldChangeIntegerPropertyWhenChanged(
        Func<MainViewModel, int> propertyGetter,
        Action<MainViewModel, int> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };
        var newValue = Any.Integer();
        //
        // // Act
        propertySetter(_mainViewModel, newValue);

        // Assert
        propertiesChanged.Should().Contain(propertyName);
        propertyGetter(_mainViewModel).Should().Be(newValue);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataInteger))]
    public void ShouldNotChangeIntegerPropertyIfNotChanged(
        Func<MainViewModel, int> propertyGetter,
        Action<MainViewModel, int> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        var value = Any.Integer();
        propertySetter(_mainViewModel, value);
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };

        // Act
        propertySetter(_mainViewModel, value);

        // Assert
        propertiesChanged.Should().NotContain(propertyName);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataBoolean))]
    public void ShouldChangeBooleanPropertyWhenChanged(
        Func<MainViewModel, bool> propertyGetter,
        Action<MainViewModel, bool> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };
        var newValue = !propertyGetter(_mainViewModel);

        // Act
        propertySetter(_mainViewModel, newValue);

        // Assert
        propertiesChanged.Should().Contain(propertyName);
        propertyGetter(_mainViewModel).Should().Be(newValue);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataBoolean))]
    public void ShouldNotChangeBooleanPropertyIfNotChanged(
        Func<MainViewModel, bool> propertyGetter,
        Action<MainViewModel, bool> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        var value = Any.Boolean();
        propertySetter(_mainViewModel, value);
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };

        // Act
        propertySetter(_mainViewModel, value);

        // Assert
        propertiesChanged.Should().NotContain(propertyName);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataString))]
    public void ShouldChangeStringPropertyWhenChanged(
        Func<MainViewModel, string> propertyGetter,
        Action<MainViewModel, string> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };
        var newValue = Any.String();

        // Act
        propertySetter(_mainViewModel, newValue);

        // Assert
        propertiesChanged.Should().Contain(propertyName);
        propertyGetter(_mainViewModel).Should().Be(newValue);
    }


    [Test]
    [TestCaseSource(nameof(PropertiesDataString))]
    public void ShouldNotChangeStringPropertyIfNotChanged(
        Func<MainViewModel, string> propertyGetter,
        Action<MainViewModel, string> propertySetter,
        string propertyName)
    {
        // Arrange
        var propertiesChanged = new List<string>();
        var value = Any.String();
        propertySetter(_mainViewModel, value);
        _mainViewModel.PropertyChanged += (_, e) => { propertiesChanged.Add(e.PropertyName); };

        // Act
        propertySetter(_mainViewModel, value);

        // Assert
        propertiesChanged.Should().NotContain(propertyName);
    }

    private static IEnumerable<TestCaseData> PropertiesDataInteger
    {
        get
        {
            yield return new TestCaseData(
                (MainViewModel viewModel) => viewModel.LogTextBoxFontSize, // propertyGetter
                (MainViewModel viewModel, int value) => { viewModel.LogTextBoxFontSize = value; }, // propertySetter
                nameof(MainViewModel.LogTextBoxFontSize)
            );
        }
    }

    private static IList<TestCaseData> PropertiesDataBoolean()
    {
        return new List<TestCaseData>()
        {
            new(
                (MainViewModel viewModel) => viewModel.ShouldWordWrapLogContent, // propertyGetter
                (MainViewModel viewModel, bool value) => { viewModel.ShouldWordWrapLogContent = value; }, // propertySetter
                nameof(MainViewModel.ShouldWordWrapLogContent)
            ),

            new(
                (MainViewModel viewModel) => viewModel.ShouldScrollToEndOnLogContentChange, // propertyGetter
                (MainViewModel viewModel, bool value) => { viewModel.ShouldScrollToEndOnLogContentChange = value; }, // propertySetter
                nameof(MainViewModel.ShouldScrollToEndOnLogContentChange)
            ),
        };
    }

    private static IEnumerable<TestCaseData> PropertiesDataString
    {
        get
        {
            yield return new TestCaseData(
                (MainViewModel viewModel) => viewModel.LogContent, // propertyGetter
                (MainViewModel viewModel, string value) => { viewModel.LogContent = value; }, // propertySetter
                nameof(MainViewModel.LogContent)
            );
        }
    }
}

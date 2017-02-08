﻿namespace MVP.App.ViewModels
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using MVP.App.Events;
    using MVP.App.Views;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using WinUX.MvvmLight.Common.ViewModels;
    using WinUX.Xaml.Controls;

    public class AppShellPageViewModel : CoreViewModelBase
    {
        private bool isInitialized;

        private bool isPaneOpen;

        private bool isBusyMessageVisible;

        private string busyMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShellPageViewModel"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        public AppShellPageViewModel(IMessenger messenger)
            : base(messenger)
        {
            this.PrimaryMenuButtons = new ObservableCollection<AppMenuButton>();
            this.SecondaryMenuButtons = new ObservableCollection<AppMenuButton>();
            this.FlyoutMenuButtons = new ObservableCollection<FlyoutAppMenuButton>();

            this.Initialize();

            this.MessengerInstance.Register<RefreshDataCompleteMessage>(
                this,
                x => this.UpdateBusyIndicator(string.Empty, !x.IsComplete));
        }

        /// <summary>
        /// Gets the primary menu buttons displayed at the top of the app menu.
        /// </summary>
        public ObservableCollection<AppMenuButton> PrimaryMenuButtons { get; }

        /// <summary>
        /// Gets the secondary menu buttons displayed at the bottom of the app menu.
        /// </summary>
        public ObservableCollection<AppMenuButton> SecondaryMenuButtons { get; }

        /// <summary>
        /// Gets the flyout menu buttons displayed above the secondary menu buttons.
        /// </summary>
        public ObservableCollection<FlyoutAppMenuButton> FlyoutMenuButtons { get; }

        public bool IsPaneOpen
        {
            get
            {
                return this.isPaneOpen;
            }
            set
            {
                this.Set(() => this.IsPaneOpen, ref this.isPaneOpen, value);
            }
        }

        public string BusyMessage
        {
            get
            {
                return this.busyMessage;
            }
            set
            {
                this.Set(() => this.BusyMessage, ref this.busyMessage, value);
            }
        }

        public bool IsBusyMessageVisible
        {
            get
            {
                return this.isBusyMessageVisible;
            }
            set
            {
                this.Set(() => this.IsBusyMessageVisible, ref this.isBusyMessageVisible, value);
            }
        }

        public void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }

            this.isInitialized = true;

            this.PrimaryMenuButtons.Clear();
            this.SecondaryMenuButtons.Clear();
            this.FlyoutMenuButtons.Clear();

            this.PrimaryMenuButtons.Add(CreateHomeButton());
            this.SecondaryMenuButtons.Add(this.CreateRefreshButton());
        }

        private AppMenuButton CreateRefreshButton()
        {
            var btn = new AppMenuButton
                          {
                              ButtonType = AppMenuButtonType.Command,
                              Command = new RelayCommand(this.RefreshData),
                              Content = GenerateButtonContent(Symbol.Refresh, "Refresh"),
                              IsGrouped = false,
                              ToolTip = "Refresh data"
                          };

            return btn;
        }

        private void RefreshData()
        {
            this.MessengerInstance.Send(new RefreshDataMessage(RefreshDataMode.All));
            this.IsPaneOpen = false;

            this.UpdateBusyIndicator("Refreshing...");
        }

        public void UpdateBusyIndicator(string message, bool show = true)
        {
            if (show && !string.IsNullOrWhiteSpace(message))
            {
                this.BusyMessage = message;
                this.IsBusyMessageVisible = true;
            }
            else
            {
                this.IsBusyMessageVisible = false;
                this.BusyMessage = string.Empty;
            }
        }

        private static AppMenuButton CreateHomeButton()
        {
            var btn = new AppMenuButton
                          {
                              ClearNavigationStack = true,
                              Content = GenerateButtonContent(Symbol.Home, "Home"),
                              IsGrouped = true,
                              Page = typeof(MainPage),
                              ToolTip = "View profile"
                          };
            return btn;
        }

        private static UIElement GenerateButtonContent(Symbol symbol, string content)
        {
            var container = new StackPanel { Orientation = Orientation.Horizontal };
            var icon = new SymbolIcon(symbol) { Width = 48, Height = 48 };
            var label = new TextBlock
                            {
                                Margin = new Thickness(12, 0, 0, 0),
                                VerticalAlignment = VerticalAlignment.Center,
                                Text = content
                            };

            container.Children.Add(icon);
            container.Children.Add(label);

            return container;
        }
    }
}
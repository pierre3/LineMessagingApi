using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Line.Messaging.Liff;
using System.Windows.Input;
using Prism.Commands;
using Line.Messaging;
using System.Collections.ObjectModel;

namespace LiffManager
{
    public class MainWindowViewModel : BindableBase
    {
        private LiffClient _liffClient;
        private LineMessagingClient _lineClient;
        private string _liffClientAccessToken;
        private string _lineClientAccessToken;

        private ViewType _selectedViewType = ViewType.Compact;
        private string _siteUrl = "";
        private string _channelAccessToken = "";
        private string _userId;

        private IList<LiffApp> _liffApps;
        private LiffApp _selectedLiffApp;
        
        private LiffClient LiffClient
        {
            get
            {
                if (_liffClient == null || _liffClientAccessToken != _channelAccessToken)
                {
                    _liffClient = new LiffClient(_channelAccessToken);
                    _liffClientAccessToken = _channelAccessToken;
                }
                return _liffClient;
            }
        }

        private LineMessagingClient LineClient
        {
            get
            {
                if (_lineClient == null || _lineClientAccessToken != _channelAccessToken)
                {
                    _lineClient = new LineMessagingClient(_channelAccessToken);
                    _lineClientAccessToken = _channelAccessToken;
                }
                return _lineClient;
            }
        }

        public ViewType SelectedViewType
        {
            get => _selectedViewType;
            set
            {
                SetProperty(ref _selectedViewType, value);
            }
        }

        public ObservableCollection<string> Errors { get; } = new ObservableCollection<string>();

        public string SiteUrl { get => _siteUrl; set => SetProperty(ref _siteUrl, value); }

        public string ChannelAccessToken { get => _channelAccessToken; set => SetProperty(ref _channelAccessToken, value); }

        public string UserId { get => _userId; set => SetProperty(ref _userId, value); }

        public IList<LiffApp> LiffApps { get => _liffApps; set => SetProperty(ref _liffApps, value); }

        public LiffApp SelectedLiffApp { get => _selectedLiffApp; set => SetProperty(ref _selectedLiffApp, value); }

        public ICommand AddLiffAppCommand { get; }

        public ICommand PushLinkMessageCommand { get; }

        public ICommand ListLiffAppsCommand { get; }

        public ICommand DeleteLiffAppCommand { get; }

        public ICommand UpdateLiffAppCommand { get; }

        public MainWindowViewModel()
        {
            AddLiffAppCommand = new DelegateCommand(async () => await AddLiffAppAsync(),
                () => !string.IsNullOrWhiteSpace(ChannelAccessToken) && !string.IsNullOrWhiteSpace(SiteUrl))
                .ObservesProperty(() => ChannelAccessToken)
                .ObservesProperty(() => SiteUrl);

            PushLinkMessageCommand = new DelegateCommand(async () => await PushLinkMessagAsync(),
                () => !string.IsNullOrWhiteSpace(ChannelAccessToken) && !string.IsNullOrWhiteSpace(UserId) && SelectedLiffApp != null)
                .ObservesProperty(() => ChannelAccessToken)
                .ObservesProperty(()=> UserId)
                .ObservesProperty(() => SelectedLiffApp);

            ListLiffAppsCommand = new DelegateCommand(async () => await ListLiffAppAsync(),
                () => !string.IsNullOrWhiteSpace(ChannelAccessToken))
                .ObservesProperty(() => ChannelAccessToken);

            DeleteLiffAppCommand = new DelegateCommand(async () => await DeleteLiffAppAsync(),
                () => !string.IsNullOrWhiteSpace(ChannelAccessToken) && SelectedLiffApp != null)
                .ObservesProperty(() => ChannelAccessToken)
                .ObservesProperty(() => SelectedLiffApp);

            UpdateLiffAppCommand = new DelegateCommand(async () => await UpdateLffAppAsync(),
                () => !string.IsNullOrWhiteSpace(ChannelAccessToken) && !string.IsNullOrWhiteSpace(SiteUrl) && SelectedLiffApp != null)
                .ObservesProperty(() => ChannelAccessToken)
                .ObservesProperty(() => SiteUrl)
                .ObservesProperty(() => SelectedLiffApp);
        }

        private async Task AddLiffAppAsync()
        {
            try
            {
                await LiffClient.AddLiffAppAsync(SelectedViewType, SiteUrl);
                LiffApps = await _liffClient.GetAllLiffAppAsync();
            }
            catch (Exception e)
            {
                Errors.Add(e.ToString());
            }
        }

        private async Task PushLinkMessagAsync()
        {
            try
            {
                await LineClient.PushMessageAsync(_userId, $"line://app/{SelectedLiffApp.LiffId}");
            }
            catch (Exception e)
            {
                Errors.Add(e.ToString());
            }
        }

        private async Task ListLiffAppAsync()
        {
            try
            {
                LiffApps = await LiffClient.GetAllLiffAppAsync();
            }
            catch (Exception e)
            {
                Errors.Add(e.ToString());
            }
        }

        private async Task DeleteLiffAppAsync()
        {
            try
            {
                await LiffClient.DeleteLiffAppAsync(SelectedLiffApp.LiffId);
                LiffApps = await _liffClient.GetAllLiffAppAsync();
            }
            catch (Exception e)
            {
                Errors.Add(e.ToString());
            }
        }

        private async Task UpdateLffAppAsync()
        {
            try
            {
                await LiffClient.UpdateLiffAppAsync(SelectedLiffApp.LiffId, SelectedViewType, SiteUrl);
                LiffApps = await _liffClient.GetAllLiffAppAsync();
            }catch(Exception e)
            {
                Errors.Add(e.ToString());
            }
        }
    }
}


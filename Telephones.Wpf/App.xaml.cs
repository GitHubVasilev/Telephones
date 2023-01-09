using System.Windows;
using Prism.Ioc;
using Telephones.API.Client.ClientAPI;
using Telephones.API.Client.Interfaces;
using Telephones.Wpf.Views;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using DryIoc.Microsoft.DependencyInjection.Extension;
using Telephones.Wpf.Views.UserControls;
using Telephones.Wpf.ViewModels;
using AutoMapper;
using Telephones.Wpf.Infrastructure.Mapper;

namespace Telephones.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile<RecordMapperConfiguration>();
            });
            mapperConfiguration.AssertConfigurationIsValid();

            containerRegistry.GetContainer().RegisterServices(services =>
            {
                services.AddHttpClient();
                services.AddSingleton(mapperConfiguration.CreateMapper());
            });

            
            containerRegistry.Register<ITelephoneBookClientAPI, TelephoneBookClientAPI>();


            #region Dialogs
            containerRegistry.RegisterDialog<BrowserRecordUserControl, BrowserRecordViewModel>("BrowserRecord");
            containerRegistry.RegisterDialog<UpdateRecordUserControl, UpdateRecordViewModel>("UpdateRecord");
            containerRegistry.RegisterDialog<CreateRecordUserControl, CreateRecordViewModel>("CreateRecord");
            containerRegistry.RegisterDialog<NotificationDialogUserControl, NotificationDialogViewModel>("NotificationDialog");
            containerRegistry.RegisterDialog<YesOrNoDialogUserControl, YesOrNoDialogViewModel>("YesOrNoDialog");
            #endregion
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}

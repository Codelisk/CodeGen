using Attributes.MauiAttributes;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Maui.Base
{
    [BaseViewModel]
    public partial class MinBaseViewModel :
        BaseBaseVm,
        IInitialize,
        IInitializeAsync,
        INavigatedAware,
        IConfirmNavigation
    {
        public MinBaseViewModel(bool useValidation = false) : base()
        {
        }
        //protected override INavigationParameters AddBaseValuesToParametersForNavigationToRegion(INavigationParameters parameters)
        //{
        //    //var result = base.AddBaseValuesToParametersForNavigationToRegion(parameters);

        //    //result.Add(BaseNavigationParameterKeys.DestroyWithFromPageViewModel, this.DestroyWith);
        //    //return result;
        //    return default;
        //}
        protected virtual void SubscribeForEvents(INavigationParameters parameters) { }
        protected virtual void UnsubscribeForEvents() { }
        protected virtual void FirstSetup(INavigationParameters parameters) { }
        public virtual void SetUpReactive()
        {
        }
        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }
        [Reactive]
        public string Title { get; set; }
        public virtual void Initialize(INavigationParameters parameters)
        {
            FirstSetup(parameters);

            if (parameters != null)
            {
                if (parameters.TryGetValue("BaseNavigationParameterKeys.Title", out string title))
                {
                    Title = title;
                }
            }

            SetUpReactive();
            SubscribeForEvents(parameters);
        }
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            bool isNavigationModeBack = false;

            //try
            //{
            //    isNavigationModeBack = parameters.GetNavigationMode() == NavigationMode.Back;
            //}
            //catch (Exception)
            //{
            //    //NavigationModeBack not supported for example when navigating through TabbedPage
            //}

            if (isNavigationModeBack || parameters.ContainsKey("BaseNavigationParameterKeys.SetupReactiveAgainBool"))
            {
                SetUpReactive();
                SubscribeForEvents(parameters);
            }

        }

        //protected override void Deactivate()
        //{
        //    base.Deactivate();
        //    this.UnsubscribeForEvents();
        //}
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            //Deactivate();
            IsBusy = false;
        }

        public virtual void OnBackButtonPressed()
        {
            //NavigationService.GoBackAsync();
        }

        public virtual bool CanNavigate(INavigationParameters parameters)
        {
            //if (parameters.TryGetValue(BaseNavigationParameterKeys.PreventIsBusyWhenNavigatingBool, out bool preventIsBusy) && preventIsBusy)
            //{
            //    return true;
            //}

            return IsBusy = true;
        }

        #region Helper
        #endregion
    }
}


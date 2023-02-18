using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using iMed.Domain.Dtos.LargDtos;
using Mapster;

namespace iMed.App.ViewModels.Pages
{
    public class FlashCardCategoryPageViewModel : ViewModelBase<FlashCardCategoryPageDto>
    {
        public ICommand AddRateCommand { get; set; }
        public ICommand PurchaseCourseCommand { get; set; }
        public ICommand MyLienterBoxCommand { get; set; }
        public bool PurchaseVisible { get; set; }
        public bool NotPurchaseVisible { get; set; }
        public int RatesCount { get; set; }
        public FlashCardCategoryPageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
        {

        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            var flashCardCategory = parameters.GetValue<FlashCardCategorySDto>("FlashCardCategory");
            PageDto.FlashCardCategory = flashCardCategory.Adapt<FlashCardCategoryLDto>();
            try
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                //var rates = await RestWrapper.RateRestApi.GetCourseRate(PageDto.Course.CourseId, 1, UtilitiesWrapper.Instance.BearerToken);
                var page = await RestWrapper.PageRestApi.GetFlashCardCategoryPage(PageDto.FlashCardCategory.FlashCardCategoryId, UtilitiesWrapper.Instance.BearerToken);

                PageDto.IsPurchased = page.Data.IsPurchased;
                PurchaseVisible = PageDto.IsPurchased;
                NotPurchaseVisible = !PageDto.IsPurchased;
                page.Data.FlashCardTags.ForEach(ft => ft.ImageFileName = page.Data.FlashCardCategory.ImageFileName);
                PageDto = page.Data;
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
            }
            catch (ApiException apiException)
            {
                var exe = await apiException.GetContentAsAsync<ApiResult>();
                if (exe != null)
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
                else
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");

            }
            catch (Exception e)
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
            }
        }


        public override void InitializeCommand()
        {
            base.InitializeCommand();
            MyLienterBoxCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("LeitnerBoxPage"));
            AddRateCommand = new DelegateCommand(async () =>
            {
                var popUp = new RateActionPopUp(PageDto.FlashCardCategory);
                popUp.Rated += (sender, args) =>
                {
                    if (sender is FlashCardCategoryRate rate)
                        PageDto.Rates.Add(rate);
                };
                await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(popUp);

            });

            PurchaseCourseCommand = new DelegateCommand(async () =>
            {
                QuestionPopUp questionPopUp = new QuestionPopUp("ایا از خرید این فلش کارت مطمئن هستید ؟");
                questionPopUp.Accepted += async (sender, args) =>
                {
                    try
                    {
                        UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                        var rest = await RestWrapper.PurchaseRestApi.PurchaseFlashCardCategory(PageDto.FlashCardCategory.FlashCardCategoryId, UtilitiesWrapper.Instance.BearerToken);
                        await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                        UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("فلش کارت مورد نظر خریداری شد");
                        NotPurchaseVisible = false;
                        PurchaseVisible = true;
                    }
                    catch (ApiException apiException)
                    {
                        var exe = await apiException.GetContentAsAsync<ApiResult>();
                        if (exe != null)
                            UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
                        else
                            UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");

                    }
                    catch (Exception e)
                    {
                        UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
                    }
                };
                await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(questionPopUp);
            });
        }
    }
}

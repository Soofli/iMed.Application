using iMed.Domain.Dtos.LargDtos;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iMed.App.ViewModels.Pages
{
    public class LeitnerBoxPageViewModel : ViewModelBase<LeitnerBoxPageDto>
    {
        public bool IsRunning { get; set; }
        public ICommand ReviewAllTodayFlashCardCommand { get; set; }
        public ICommand ShowFlashCardCommand { get; set; }
        public ICommand SelectFlashCardTagCommand { get; set; }
        public ICommand SelectLeitnerBoxCommand { get; set; }
        public ICommand ReviewAllSelectedFlashCardCommand { get; set; }
        private FlashCardCategorySDto _selectedFlashCardCategory;
        private LeitnerBoxDto _selectedLeitnerBox;
        public LeitnerBoxPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public LeitnerBoxPageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
        {
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            try
            {
                if (IsRunning)
                    return;
                IsRunning = true;
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                _selectedFlashCardCategory = null;
                _selectedLeitnerBox = null;

                var rest = await RestWrapper.PageRestApi.GetLeitnerBoxPage(UtilitiesWrapper.Instance.BearerToken);
                PageDto.FlashCardsTag.Clear();
                PageDto.FlashCardCategories.Clear();
                PageDto.FlashCards.Clear();
                PageDto.TodayFlashCards.Clear();
                PageDto.LeitnerBoxes.Clear();
                PageDto.SelectedFlashCards.Clear();
                PageDto.TotalFlashCard = rest.Data.TotalFlashCard;
                PageDto.TotalDoneFlashCard = rest.Data.TotalDoneFlashCard;
                PageDto.TotalTodayFlashCard = rest.Data.TotalTodayFlashCard;
                _selectedFlashCardCategory = new FlashCardCategorySDto
                {
                    Name = "همه",
                    IsSelected = true,
                    FlashCardCount = rest.Data.TotalTodayFlashCard
                };
                PageDto.FlashCardCategories.Add(_selectedFlashCardCategory);
                rest.Data.FlashCardCategories.ForEach(fcc => PageDto.FlashCardCategories.Add(fcc));
                rest.Data.FlashCards.OrderBy(f => f.NextReviewAt).ForEach(fcc => PageDto.FlashCards.Add(fcc));
                rest.Data.FlashCardsTag.ForEach(fcc => PageDto.FlashCardsTag.Add(fcc));
                rest.Data.TodayFlashCards.ForEach(fcc => PageDto.TodayFlashCards.Add(fcc));
                rest.Data.LeitnerBoxes.OrderBy(b => b.Status).ForEach(box => PageDto.LeitnerBoxes.Add(box));
                if(PageDto.LeitnerBoxes.Any(lb=>lb.Status==Domain.Enums.FlashCardStatus.Archived))
                {
                    var archiveBox = PageDto.LeitnerBoxes.FirstOrDefault(lb => lb.Status == Domain.Enums.FlashCardStatus.Archived);
                    PageDto.LeitnerBoxes.Remove(archiveBox);
                    PageDto.LeitnerBoxes.Add(archiveBox);
                }
                PageDto.TodayFlashCards.ForEach(fs => PageDto.SelectedFlashCards.Add(fs));
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
            finally
            {
                IsRunning = false;
            }
        }

        public override void InitializeCommand()
        {
            base.InitializeCommand();
            ShowFlashCardCommand = new DelegateCommand<UserFlashCardStatusLDto>(async (flashCard) =>
            {
                await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(new FlashCardPopUp(flashCard), true);
            });
            ReviewAllSelectedFlashCardCommand = new DelegateCommand(async () =>
            {
                try
                {
                    var flashCards = new List<UserFlashCardStatusLDto>();
                    if (_selectedFlashCardCategory != null)
                    {
                        flashCards = PageDto.FlashCards.Where(f => f.FlashCardCategoryId == _selectedFlashCardCategory.FlashCardCategoryId).ToList();
                        if (_selectedLeitnerBox != null)
                            flashCards = flashCards.Where(f => f.FlashCardStatus == _selectedLeitnerBox.Status).ToList();
                    }
                    else
                        throw new AppException("درس موردنظر خود را انتخاب کنید");
                    flashCards = flashCards.Where(fc => fc.NextReviewAt.Date <= DateTime.Now.Date).ToList();
                    flashCards = flashCards.Where(fc => fc.FlashCardStatus != Domain.Enums.FlashCardStatus.Done && fc.FlashCardStatus != Domain.Enums.FlashCardStatus.Archived).ToList();
                    if (flashCards.Count <= 0)
                        throw new AppException("در درس های انتخابی شما فلش کارتی برای مرور نمی باشد");
                    var parms = new NavigationParameters();

                    if (flashCards.Any(t => t.FlashCardStatus == Domain.Enums.FlashCardStatus.Step0))
                    {
                        if (flashCards.Any(t => t.FlashCardStatus != Domain.Enums.FlashCardStatus.Step0))
                        {
                            var dialog = new StartReviewQuestionPopUp();
                            dialog.AllFlashCardSelected += async (sender, e) =>
                            {
                                parms.Add("FlashCards", flashCards.ToList());
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            dialog.OldFlashCardSelected += async (sender, e) =>
                            {
                                parms.Add("FlashCards", flashCards.Where(f => f.FlashCardStatus != Domain.Enums.FlashCardStatus.Step0).ToList());
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            dialog.NewFlashCardSelected += async (sender, e) =>
                            {
                                parms.Add("FlashCards", flashCards.Where(f => f.FlashCardStatus == Domain.Enums.FlashCardStatus.Step0).ToList());
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(dialog);
                        }
                        else
                        {
                            parms.Add("FlashCards", flashCards.ToList());
                            var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                        }
                    }
                    else
                    {
                        parms.Add("FlashCards", flashCards.ToList());
                        var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                    }
                }
                catch (Exception ex)
                {
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError(ex.Message);
                }

            });
            SelectLeitnerBoxCommand = new DelegateCommand<LeitnerBoxDto>(box =>
            {
                PageDto.LeitnerBoxes.ForEach(ft => ft.IsSelected = false);
                box.IsSelected = true;
                _selectedLeitnerBox = box;
                PageDto.SelectedFlashCards.Clear();
                if (_selectedFlashCardCategory == null || _selectedFlashCardCategory.Name == "همه")
                    PageDto.FlashCards.Where(fc => fc.FlashCardStatus == _selectedLeitnerBox.Status)
                        .ForEach(fc => PageDto.SelectedFlashCards.Add(fc));
                else
                    PageDto.FlashCards.Where(fc => fc.FlashCardCategoryId == _selectedFlashCardCategory.FlashCardCategoryId && fc.FlashCardStatus == _selectedLeitnerBox.Status)
                        .ForEach(fc => PageDto.SelectedFlashCards.Add(fc));
            });
            SelectFlashCardTagCommand = new DelegateCommand<FlashCardCategorySDto>(cat =>
            {
                PageDto.LeitnerBoxes.ForEach(ft => ft.IsSelected = false);
                PageDto.FlashCardCategories.ForEach(ft => ft.IsSelected = false);
                cat.IsSelected = true;
                _selectedLeitnerBox = null;
                _selectedFlashCardCategory = cat;
                PageDto.SelectedFlashCards.Clear();
                if (cat.Name == "همه")
                {
                    PageDto.FlashCards.ForEach(fc => PageDto.SelectedFlashCards.Add(fc));
                    foreach (var leitnerBox in PageDto.LeitnerBoxes)
                        leitnerBox.FlashCardCount = PageDto.FlashCards.Count(fc => fc.FlashCardStatus == leitnerBox.Status);
                }
                else
                {
                    PageDto.FlashCards.Where(fc => fc.FlashCardCategoryId == _selectedFlashCardCategory.FlashCardCategoryId).ForEach(fc => PageDto.SelectedFlashCards.Add(fc));

                    var flashCards = new List<UserFlashCardStatusLDto>();
                    flashCards.AddRange(PageDto.FlashCards.Where(fc => fc.FlashCardCategoryId == _selectedFlashCardCategory.FlashCardCategoryId).ToList());
                    foreach (var leitnerBox in PageDto.LeitnerBoxes)
                        leitnerBox.FlashCardCount = flashCards.Count(fc => fc.FlashCardStatus == leitnerBox.Status);
                }
            });
            ReviewAllTodayFlashCardCommand = new DelegateCommand(async () =>
            {
                try
                {

                    if (PageDto.TodayFlashCards.Count == 0)
                        throw new AppException("امروز فلش کارتی برای مرور ندارید");
                    var parms = new NavigationParameters();
                    if (PageDto.TodayFlashCards.Any(t => t.FlashCardStatus == Domain.Enums.FlashCardStatus.Step0))
                    {
                        if (PageDto.TodayFlashCards.Any(t => t.FlashCardStatus != Domain.Enums.FlashCardStatus.Step0))
                        {
                            var dialog = new StartReviewQuestionPopUp();
                            dialog.AllFlashCardSelected += async (sender, e) =>
                            {
                                var list = PageDto.TodayFlashCards.Where(f=>f.FlashCardStatus != Domain.Enums.FlashCardStatus.Archived).ToList();
                                parms.Add("FlashCards", list);
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            dialog.OldFlashCardSelected += async (sender, e) =>
                            {
                                var list = PageDto.TodayFlashCards.Where(f => f.FlashCardStatus != Domain.Enums.FlashCardStatus.Archived && f.FlashCardStatus != Domain.Enums.FlashCardStatus.Step0).ToList();
                                parms.Add("FlashCards", list);
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            dialog.NewFlashCardSelected += async (sender, e) =>
                            {
                                var list = PageDto.TodayFlashCards.Where(f => f.FlashCardStatus != Domain.Enums.FlashCardStatus.Archived && f.FlashCardStatus == Domain.Enums.FlashCardStatus.Step0).ToList();
                                parms.Add("FlashCards", list);
                                var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                            };
                            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(dialog);
                        }
                        else
                        {
                            parms.Add("FlashCards", PageDto.TodayFlashCards.ToList());
                            var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                        }
                    }
                    else
                    {
                        parms.Add("FlashCards", PageDto.TodayFlashCards.ToList());
                        var res = await NavigationService.NavigateAsync("ReviewPage", parms);
                    }
                }
                catch (Exception ex)
                {
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError(ex.Message);
                }
            });
        }
    }
}

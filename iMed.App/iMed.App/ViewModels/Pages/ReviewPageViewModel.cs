using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using iMed.Domain.Dtos.LargDtos;
using iMed.Domain.Enums;
using Refit;

namespace iMed.App.ViewModels.Pages
{
    public class ReviewPageViewModel : ViewModelBase<ReviewPageDto>
    {
        public ICommand StartReviewCommand { get; set; }
        public ICommand PauseReviewCommand { get; set; }
        public ICommand StopReviewCommand { get; set; }
        public ICommand SubmitAnswerCommand { get; set; }
        public ICommand SelectAnswerCommand { get; set; }
        public ICommand NextFlashCardCommand { get; set; }
        public ICommand FinishReviewCommand { get; set; }
        public ICommand ArchiveFlashCardCommand { get; set; }
        public ObservableCollection<FlashCardSubmittedAnswer> SubmittedAnswers { get; set; } = new();
        public bool DontStartIsVisible { get; set; } = true;
        public bool FinishReviewIsVisible { get; set; } = false;
        public int FlashCardsPosition { get; set; } = 0;
        public long TimeElapsedMS { get; set; } = 0;
        public string TimeElapsedCounter { get; set; } = "00:00";
        public long TimeElapsedSecond => TimeElapsedMS / 1000;
        public bool NextAnswerVisible { get; set; } = false;
        public bool SubmitAnswerVisible { get; set; } = false;
        public bool ShowResult { get; set; } = false;
        public int AllFlashCardCount { get; set; }
        public int TrueAnswerCount { get; set; }
        public int FalseAnswerCount { get; set; }
        public double TotalScore { get; set; } = 0;
        public bool IsPause { get; set; } = false;

        private Timer _timer = new Timer(1000);
        private long _timeForAnswer = 0;
        public ReviewPageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
        {
            _timer.Elapsed += (sender, args) =>
            {
                TimeElapsedMS += 1000;
                _timeForAnswer += 1000;
                var second = (TimeElapsedMS / 1000) % 60;
                var minutes = (TimeElapsedMS / 1000) / 60;
                TimeElapsedCounter = string.Format("{0}:{1}", minutes < 10 ? $"0{minutes}" : minutes, second < 10 ? $"0{second}" : second);
            };
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var flashCards = parameters.GetValue<List<UserFlashCardStatusLDto>>("FlashCards");
            flashCards.ForEach(fc =>
            {
                fc.FlashCardAnswers = fc.FlashCardAnswers.OrderBy(an => an.Row).ToList();
                PageDto.FlashCards.Add(fc);
            });
            FlashCardsPosition = flashCards.Count - 1;
            PageDto.FlashCards[(PageDto.FlashCards.Count - 1) - FlashCardsPosition].IsSelected = true;
            AllFlashCardCount++;
            SubmitAnswerVisible = flashCards[FlashCardsPosition].FlashCardType == FlashCardType.MultiAnswer;
        }

        public override void InitializeCommand()
        {
            base.InitializeCommand();
            ArchiveFlashCardCommand = new DelegateCommand(async() =>
            {
                var ufs = PageDto.FlashCards[FlashCardsPosition];
                var dialog = new QuestionPopUp("آیا از ارشیو کردن این فلش کارت مطمئن هستید ؟");
                dialog.Accepted += (ss, ee) =>
                {
                    Task.Run(async () => { await RestWrapper.LeitnerRestApi.ArchiveFlashCard(ufs.UserFlashCardStatusId,UtilitiesWrapper.Instance.BearerToken); });
                    NextFlashCardCommand.Execute(null);
                };

                await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(dialog);

            });
            FinishReviewCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
            NextFlashCardCommand = new DelegateCommand(() =>
            {
                if (FlashCardsPosition > 0)
                {
                    PageDto.FlashCards[(PageDto.FlashCards.Count - 1) - FlashCardsPosition].IsSelected = false;
                    FlashCardsPosition--;
                    PageDto.FlashCards[(PageDto.FlashCards.Count - 1) - FlashCardsPosition].IsSelected = true;
                    AllFlashCardCount++;
                    SubmitAnswerVisible = PageDto.FlashCards[FlashCardsPosition].FlashCardType == FlashCardType.MultiAnswer;
                    NextAnswerVisible = false;
                }
                else
                {
                    ShowResult = true;
                    _timer.Stop();
                }
            });
            SelectAnswerCommand = new DelegateCommand<FlashCardAnswerSDto>(answer =>
            {

                var flashCard = PageDto.FlashCards.FirstOrDefault(fc => fc.FlashCardId == answer.FlashCardId);
                if (flashCard.FlashCardType == FlashCardType.SingleAnswer)
                {
                    flashCard.FlashCardAnswers.ForEach(fca => fca.IsSelected = false);
                    answer.IsSelected = true;
                    SubmitAnswerCommand.Execute(flashCard);
                }
                else
                {
                    answer.IsSelected = !answer.IsSelected;
                }
            });
            SubmitAnswerCommand = new DelegateCommand<UserFlashCardStatusLDto>(flashCard =>
            {
                flashCard.FlashCardAnswers.Where(fca => fca.IsSelected).ForEach(fca =>
                {
                    SubmittedAnswers.Add(new FlashCardSubmittedAnswer
                    {
                        FlashCardId = fca.FlashCardId,
                        FlashCardAnswerId = fca.FlashCardAnswerId,
                        IsTrue = fca.IsTrue,
                        ElapsedTimePerSecond = _timeForAnswer / 1000
                    });
                    fca.ShowResult = true;
                    if (!flashCard.IsMultiAnswer)
                    {
                        flashCard.FlashCardAnswers.ForEach(fca => fca.IsEnabled = false);
                        Task.Run(async () =>
                        {
                            var rest = await RestWrapper.LeitnerRestApi.SubmitAnswer(fca.FlashCardAnswerId, _timeForAnswer, UtilitiesWrapper.Instance.BearerToken);
                            if (rest.IsSuccess)
                            {
                                if (rest.Data.FirstOrDefault().IsTrue)
                                    TrueAnswerCount++;
                                else
                                    FalseAnswerCount++;
                                TotalScore += rest.Data.FirstOrDefault().Score;
                            }
                        });
                    }
                });
                flashCard.FlashCardAnswers.Where(fca => fca.IsSelected).ForEach(fca => fca.ShowResult = true);
                flashCard.FlashCardAnswers.Where(fca => fca.IsTrue).ForEach(fca => fca.ShowResult = true);
                if (flashCard.IsMultiAnswer)
                {
                    Task.Run(async () =>
                    {
                        var rest = await RestWrapper.LeitnerRestApi
                        .SubmitMultiAnswer(flashCard.FlashCardId, flashCard.FlashCardAnswers.Where(fca => fca.IsSelected)
                                .Select(fca => new SubmitAnswerRequest { AnswerId = fca.FlashCardAnswerId, ElapsedTime = _timeForAnswer }).ToList(), UtilitiesWrapper.Instance.BearerToken);
                        if (rest.IsSuccess)
                        {
                            if (rest.Data.IsTrue)
                                TrueAnswerCount++;
                            else
                                FalseAnswerCount++;
                            TotalScore += rest.Data.Score;
                        }
                    });
                }
                flashCard.ShowLongAnswer = true;
                NextAnswerVisible = true;
                SubmitAnswerVisible = false;
                _timeForAnswer = 0;

            });
            StopReviewCommand = new DelegateCommand(async () =>
            {
                if (AllFlashCardCount > 0)
                {
                    _timer.Stop();
                    ShowResult = true;
                }
                else
                    await NavigationService.GoBackAsync();
            });
            StartReviewCommand = new DelegateCommand(() =>
            {
                DontStartIsVisible = false;
                IsPause = false;
                _timer.Start();
            });
            PauseReviewCommand = new DelegateCommand(() =>
            {
                _timer.Stop();
                DontStartIsVisible = true;
                IsPause = true;
            });
        }
    }
}

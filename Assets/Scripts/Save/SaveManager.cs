using System;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using DeliveryRushExam.UGS;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class SaveManager : MonoBehaviour
    {
        public PlayerProgressData CurrentProgress { get; private set; } = new PlayerProgressData();
        public event Action<PlayerProgressData> ProgressLoaded;

        private ISaveService saveService;

        [SerializeField] private UgsInitializer ugsInitializer;

        private async void Start()
        {
            if (ugsInitializer != null)
            {
                await WaitForUgs();
            }

            saveService = ServiceLocator.Get<ISaveService>();
            await LoadProgressAsync();
        }

        private async Task WaitForUgs()
        {
            while (!ugsInitializer.IsReady)
            {
                await Task.Yield();
            }
        }

        public async Task LoadProgressAsync()
        {
            CurrentProgress = await saveService.LoadAsync();
            ProgressLoaded?.Invoke(CurrentProgress);
        }

        public async Task SaveMatchResultAsync(int score, int coins, int completedOrders)
        {
            CurrentProgress.bestScore = Mathf.Max(CurrentProgress.bestScore, score);
            CurrentProgress.totalCoins += coins;
            CurrentProgress.completedOrders += completedOrders;
            CurrentProgress.unlockedLevel = Mathf.Max(CurrentProgress.unlockedLevel, 1 + CurrentProgress.completedOrders / 10);
            await saveService.SaveAsync(CurrentProgress);
        }
    }
}
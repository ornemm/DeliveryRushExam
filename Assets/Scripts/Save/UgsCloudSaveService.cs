#if DELIVERY_RUSH_UGS
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using Unity.Services.CloudSave;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class UgsCloudSaveService : ISaveService
    {
        private const string ProgressKey = "delivery_rush_progress";

        public async Task<PlayerProgressData> LoadAsync()
        {
            try
            {
                var keys = new HashSet<string> { ProgressKey };
                var result = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

                if (result.TryGetValue(ProgressKey, out var item))
                {
                    string json = item.Value.GetAsString();
                    return JsonUtility.FromJson<PlayerProgressData>(json) ?? new PlayerProgressData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Cloud Load failed: " + e.Message);
            }

            return new PlayerProgressData();
        }

        public async Task SaveAsync(PlayerProgressData progressData)
        {
            try
            {
                progressData.TouchSaveDate();
                string json = JsonUtility.ToJson(progressData);
                var data = new Dictionary<string, object> { { ProgressKey, json } };
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
                Debug.Log("Cloud Save: data saved successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Cloud Save failed: " + e.Message);
            }
        }
    }
}
#else
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class UgsCloudSaveService : ISaveService
    {
        public async Task<PlayerProgressData> LoadAsync()
        {
            Debug.LogWarning("UGS Cloud Save is not enabled.");
            await Task.Yield();
            return new PlayerProgressData();
        }

        public async Task SaveAsync(PlayerProgressData progressData)
        {
            Debug.LogWarning("UGS Cloud Save is not enabled.");
            await Task.Yield();
        }
    }
}
#endif
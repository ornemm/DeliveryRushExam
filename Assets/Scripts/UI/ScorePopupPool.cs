using System.Collections.Generic;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupPool : MonoBehaviour
    {
        [SerializeField] private ScorePopupView prefab;
        [SerializeField] private Transform container;
        [SerializeField] private int initialSize = 10;

        private Queue<ScorePopupView> pool = new Queue<ScorePopupView>();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                ScorePopupView instance = Instantiate(prefab, container);
                instance.gameObject.SetActive(false);
                pool.Enqueue(instance);
            }
        }

        public ScorePopupView Get()
        {
            if (pool.Count > 0)
            {
                ScorePopupView popup = pool.Dequeue();
                popup.gameObject.SetActive(true);
                return popup;
            }
            ScorePopupView newPopup = Instantiate(prefab, container);
            return newPopup;
        }

        public void ReturnToPool(ScorePopupView popup)
        {
            popup.gameObject.SetActive(false);
            popup.transform.localPosition = Vector3.zero;
            pool.Enqueue(popup);
        }
    }
}
using TMPro;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float lifetime = 1.1f;
        [SerializeField] private float moveSpeed = 55f;

        private float age;
        private ScorePopupPool pool;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(string message, ScorePopupPool pool)
        {
            age = 0f;
            this.pool = pool;
            messageText.text = message;
            transform.localPosition = Vector3.zero;
            if (canvasGroup != null) canvasGroup.alpha = 1f;
        }

        private void Update()
        {
            age += Time.deltaTime;
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - age / lifetime;
            }

            if (age >= lifetime)
            {
                if (pool != null)
                    pool.ReturnToPool(this);
                else
                    Destroy(gameObject);
            }
        }
    }
}


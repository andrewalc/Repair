using UnityEngine;

namespace Ui
{
    public class HideUntilGameStart : MonoBehaviour
    {
        public void Start()
        {
            gameObject.SetActive(false);
            Game.Instance.BeginPlay += OnBeginPlay;
        }

        private void OnDestroy()
        {
            Game.Instance.BeginPlay -= OnBeginPlay;
        }

        private void OnBeginPlay()
        {
            Game.Instance.BeginPlay -= OnBeginPlay;
            gameObject.SetActive(true);
        }
    }
}
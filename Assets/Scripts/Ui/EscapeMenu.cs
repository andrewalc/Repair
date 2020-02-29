using UnityEngine;

namespace Ui
{
    public class EscapeMenu : MonoBehaviour
    {
        [SerializeField] private GameObject creditsMenu;
        [SerializeField] private GameObject settingsMenu;
        
        public void Quit()
        {
            Application.Quit();
        }
        
        public void Click()
        {
            SoundManager.Instance.PlaySound(SoundNames.click);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void OnDisable()
        {
            Tick.Instance.UnPause();
        }

        public void Update()
        {
            if (Input.GetKeyUp("escape"))
            {
                if (null != creditsMenu)
                {
                    creditsMenu.SetActive(false);
                }

                if (null != settingsMenu)
                {
                    settingsMenu.SetActive(false);
                }
                
                
                gameObject.SetActive(false);
            }
        }
        
    }
}
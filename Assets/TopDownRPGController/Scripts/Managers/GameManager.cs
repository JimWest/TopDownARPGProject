using UnityEngine;
using System.Collections;

namespace TopDown
{

    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        GameObject _endGameScreen;

        public static GameManager gameManagerInstance;       

        void Awake()
        {

            if (gameManagerInstance == null)
            {
                gameManagerInstance = this;
            }
            else if (gameManagerInstance != this)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLostGame()
        {
            if (_endGameScreen)
            {
                Instantiate(_endGameScreen);
            }

        }
    } 
}

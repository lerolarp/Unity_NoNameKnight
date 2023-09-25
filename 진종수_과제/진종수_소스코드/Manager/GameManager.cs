using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;


    private GameStage currentStage = null;
    public GameStage CurrentStage => currentStage;

    private UserData _userData = null;
    public UserData userData => _userData;


    private int mapIndex = 1;
    public int MapIndex => mapIndex;

    public bool isAutoSkill = false;
    public float timeScale = 1;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if(GameManager.Instance != null && this != GameManager.Instance)
        {
            Destroy(this.gameObject);
        }

        if (null == instance)
        {
#if UNITY_EDITOR
            Application.targetFrameRate = 25;
#elif UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            _userData = new UserData();
            _userData.Intialize(new int[] { 1000, 2000, 3000, 4000, 5000, 6000 });

            _userData.SetBattleLine(Enum.BattleLineType.Front, new int[] { 1000});
            _userData.SetBattleLine(Enum.BattleLineType.Middle, new int[] { 4000, 3000 });
            _userData.SetBattleLine(Enum.BattleLineType.Back, new int[] { });

            isAutoSkill = PlayerPrefs.GetInt("IsAutoSkill") == 0 ? false : true;
            timeScale = PlayerPrefs.GetFloat("FastPlaySpeed");
            if (timeScale == 0)
            {
                PlayerPrefs.SetFloat("FastPlaySpeed", 1f);
                timeScale = 1;
            }
        }

    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
       
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (_userData != null)
        {
            _userData.Dispose();
            _userData = null;
        }

        if(currentStage != null)
            currentStage = null;
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            currentStage = GameObject.Find("BattleStage").GetComponent<GameStage>();
            currentStage.OnSceneLoaded();
        }

        if (scene.name == "ChapterScene")
        {
            currentStage = GameObject.Find("ChapterStage").GetComponent<GameStage>();
            currentStage.OnSceneLoaded();
        }
    }

    public void SetAutoSkill(bool isEnable)
    {
        PlayerPrefs.SetInt("IsAutoSkill", isEnable ? 1 : 0);
        isAutoSkill = isEnable;
    }

    public void SetFastPlaySpeed(bool isEnable)
    {
        PlayerPrefs.SetFloat("FastPlaySpeed", isEnable ? 1.5f : 1f);
        Time.timeScale = isEnable ? 1.5f : 1f;

        timeScale = Time.timeScale;
    }

    public void SelectMapIndex(int mapIndex)
    {
        this.mapIndex = mapIndex;
    }
}

using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Init => _init;

    private static GameManager _init;

    public int AnimalCount = 5;

    public Animal[] AnimalPrefabs;

    public Animal[] WorldAnimals;

    public Transform[] AnimalWalkPoints;

    private void Awake()
    {
        _init = this;

        GenerateAnimals(AnimalCount);
    }

    private void GenerateAnimals(int count)
    {
        WorldAnimals = new Animal[count];

        for (int i = 0; i < count; i++)
        {
            var prefab = Helper.ÑhoiceArray<Animal>(AnimalPrefabs);

            WorldAnimals[i] = Instantiate<Animal>(prefab, Helper.ÑhoiceArray<Transform>(AnimalWalkPoints).position, Quaternion.identity);

            WorldAnimals[i].Init();
        }
    }

    public void ChangeSceneToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDisable()
    {
        _init = null;
    }
}

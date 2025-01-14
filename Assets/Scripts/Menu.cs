using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string sceneName;
    public SceneTransition sceneTransition;

    public void DificuldadeDaRodada(int size)
    {
        PlayerPrefs.SetInt("PuzzleSize", size); 
    }

    public void EscolherImagem(int imageIndex)
    {
        PlayerPrefs.SetInt("PuzzleImageIndex", imageIndex); 
        sceneTransition.TransitionToScene(sceneName);
    }
}

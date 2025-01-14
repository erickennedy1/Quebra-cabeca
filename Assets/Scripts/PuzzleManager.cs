using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private List<Transform> piecePrefabs;
    [SerializeField] private int puzzleSize = 2; 
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private ParticleSystem completionParticles; 

    private string audioSourceBackgroundTag = "AudioBackground";
    public AudioClip audioClip;
    private AudioSource audioSourceBackground;

    private List<Transform> pieces;
    private int emptyLocation;
    private int size;
    private bool shuffling = false;
    private bool isPuzzleCompleted = false; 
    private bool isGameActive = true;
    private Transform currentPiecePrefab;

private void CreateGamePieces(float gapThickness)
{
    float width = 1 / (float)size; 
    for (int row = 0; row < size; row++)
    {
        for (int col = 0; col < size; col++)
        {
            Transform piece = Instantiate(currentPiecePrefab, gameTransform);
            pieces.Add(piece);

            piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                              +1 - (2 * width * row) - width,
                                              0);
            piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
            piece.name = $"{(row * size) + col}";

            if ((row == size - 1) && (col == size - 1))
            {
                emptyLocation = (size * size) - 1;
                piece.gameObject.SetActive(false);
            }
            else
            {
                float gap = gapThickness / 2;
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];

                uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap)); // Inferior esquerdo
                uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap)); // Inferior direito
                uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap)); // Superior esquerdo
                uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap)); // Superior direito

                mesh.uv = uv;
            }
        }
    }
}


    void Start()
    {
        pieces = new List<Transform>();

        GameObject backgroundAudioObject = GameObject.FindGameObjectWithTag(audioSourceBackgroundTag);
        if (backgroundAudioObject != null)
        {
            audioSourceBackground = backgroundAudioObject.GetComponent<AudioSource>();
        }

        size = PlayerPrefs.GetInt("PuzzleSize", 2);
        int imageIndex = PlayerPrefs.GetInt("PuzzleImageIndex", 0);

        if (imageIndex >= 0 && imageIndex < piecePrefabs.Count)
        {
            currentPiecePrefab = piecePrefabs[imageIndex];
        }
        else
        {
            currentPiecePrefab = piecePrefabs[0]; 
        }

        CreateGamePieces(0.01f);
        Shuffle();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!isGameActive) return; 

        if (!shuffling && !isPuzzleCompleted && CheckCompletion())
        {
            isPuzzleCompleted = true;
            isGameActive = false; 
            audioSourceBackground.PlayOneShot(audioClip);
            completionParticles.Play(); 

            StartCoroutine(VictoryPanel(3));
            Debug.Log("Puzzle completo!");
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size, size)) { break; }
                        if (SwapIfValid(i, +size, size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, size - 1)) { break; }
                    }
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator VictoryPanel(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        isGameActive = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            int rnd = Random.Range(0, size * size);
            if (rnd == last) { continue; }
            last = emptyLocation;
            if (SwapIfValid(rnd, -size, size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +size, size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }
}

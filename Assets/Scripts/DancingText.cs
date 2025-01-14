using UnityEngine;
using TMPro;
using System.Collections;

public class DancingText : MonoBehaviour
{
    public TMP_Text textMeshPro; // Referência ao componente TextMeshPro
    public float amplitude = 5f; // Distância que as letras se movem
    public float speed = 2f;     // Velocidade do movimento
    public float delay = 0.1f;   // Atraso entre os movimentos das letras

    private Coroutine animationCoroutine;

    void Start()
    {
        // Obtém o componente TextMeshPro se não estiver configurado
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }
    }

    void OnEnable()
    {
        // Inicia ou reinicia a animação ao ativar o GameObject
        if (animationCoroutine == null)
        {
            animationCoroutine = StartCoroutine(AnimateText());
        }
    }

    void OnDisable()
    {
        // Interrompe a animação ao desativar o GameObject
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            textMeshPro.ForceMeshUpdate(); // Atualiza o texto

            TMP_TextInfo textInfo = textMeshPro.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                // Obtém o índice dos vértices do caractere
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Obtém os vértices do caractere
                Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

                // Calcula o deslocamento
                float offset = Mathf.Sin((Time.time * speed) + (i * delay)) * amplitude;

                // Move os vértices
                vertices[vertexIndex + 0].y += offset;
                vertices[vertexIndex + 1].y += offset;
                vertices[vertexIndex + 2].y += offset;
                vertices[vertexIndex + 3].y += offset;

                // Aplica as alterações
                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            }

            yield return null; // Espera o próximo frame
        }
    }
}

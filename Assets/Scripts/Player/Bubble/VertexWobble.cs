using TMPro;
using UnityEngine;
public class VertexWobble : MonoBehaviour
{
	TMP_Text textMesh;
	Mesh mesh;
	Vector3[] vertices;
	Color32[] vertexColors;
	// Start is called before the first frame update
	void Start()
	{
		textMesh = GetComponent<TMP_Text>();
	}

	// Update is called once per frame
	void Update()
	{
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		vertices = mesh.vertices;

		for (int i = 0; i < textMesh.textInfo.characterCount; i++)
		{

			TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

			int index = c.vertexIndex;

			index %= vertices.Length;

			Vector3 offset = Wobble(Time.time + i);

			vertices[index] += offset;
			vertices[index + 1] += offset;
			vertices[index + 2] += offset;
			vertices[index + 3] += offset;
			mesh.vertices = vertices;
			textMesh.canvasRenderer.SetMesh(mesh);
		}

	}

	Vector2 Wobble(float time)
	{
		return new Vector2(Mathf.Sin(time * 5f) * 2, Mathf.Cos(time * 5f) * 2);
	}

	public void RemoveCharacter(char characterToRemove)
	{
		string originalText = textMesh.text;
		string updatedText = originalText.Replace(characterToRemove.ToString(), "\u00A0");
		textMesh.text = updatedText;
	}

	public void MakeCharacterDisappear(char characterToHide)
	{
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		vertexColors = mesh.colors32;

		for (int i = 0; i < textMesh.textInfo.characterCount; i++)
		{
			TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

			// Check if the character matches
			if (c.character == characterToHide && c.isVisible)
			{
				int vertexIndex = c.vertexIndex;

				// Ensure the vertex index is within bounds
				if (vertexIndex >= vertexColors.Length) continue;

				// Set alpha to 0 for all 4 vertices of the character
				vertexColors[vertexIndex] = new Color32(0, 0, 0, 0); // Top-left
				vertexColors[vertexIndex + 1] = new Color32(0, 0, 0, 0); // Top-right
				vertexColors[vertexIndex + 2] = new Color32(0, 0, 0, 0); // Bottom-right
				vertexColors[vertexIndex + 3] = new Color32(0, 0, 0, 0); // Bottom-left
			}
		}

		// Apply the updated colors to the mesh
		mesh.colors32 = vertexColors;
		textMesh.canvasRenderer.SetMesh(mesh);
		Debug.Log("Here");
	}
}

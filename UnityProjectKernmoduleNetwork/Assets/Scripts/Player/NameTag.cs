using UnityEngine;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameTagText;

    public void SetText(string text)
    {
        nameTagText.text = text;
    }

    private void Update()
    {
        if (SessionVariables.instance.playerDictionary[SessionVariables.instance.myPlayerId].playerObject != null)
            transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(SessionVariables.instance.playerDictionary[SessionVariables.instance.myPlayerId].playerObject.transform.position - transform.position).eulerAngles.y, 0);
    }
}

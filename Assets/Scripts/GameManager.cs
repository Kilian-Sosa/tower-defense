using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] Texture2D cursorTarget;
    [SerializeField] TextMeshProUGUI shotsText, killsText;
    [SerializeField] GameObject dialoguesObject;
    [SerializeField] GameObject HPBar;
    [SerializeField] float reduceSpawnCut = 10, timeSpawnReduce = 0.1f, velocityIncrease = 0.5f;

    public int shots = 0, kills = 0;
    bool canShoot = true;
    public float life = 100, maxLife = 100;

    void Start() {
        Vector2 hotspot = new Vector2 (cursorTarget.width/2, cursorTarget.height/2);
        Cursor.SetCursor(cursorTarget, hotspot, CursorMode.Auto);

        UpdateShots();
        UpdateKills();
    }


    void Update() {
        HPBar.GetComponent<Image>().fillAmount = life / maxLife;

        if (Input.GetMouseButton(0)) {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null) {
                if (hit.collider.CompareTag("spikeball_item") && !dialoguesObject.activeSelf) {
                    Debug.Log("��BOLA CON PINCHOS!!");
                    DisableFire();
                    Destroy(hit.collider.gameObject);
                    dialoguesObject.SetActive(true);
                    dialoguesObject.GetComponent<DialogueController>().StartDialogue("spikedball_item");
                }
                if (hit.collider.CompareTag("sawblade_item") && !dialoguesObject.activeSelf) {
                    Debug.Log("��DISCO DE SIERRA!!");
                    DisableFire();
                    Destroy(hit.collider.gameObject);
                    dialoguesObject.SetActive(true);
                    dialoguesObject.GetComponent<DialogueController>().StartDialogue("sawblade_item");
                }
                if (hit.collider.CompareTag("Enemy")) EnableFire();
            }else EnableFire();
        }
    }

    public void UpdateShots() {
        shotsText.text = $"Disparos: {shots}";
    }

    public void UpdateKills() {
        killsText.text = $"Muertes: {kills}";
        if (kills != 0 && kills % reduceSpawnCut == 0) 
            GameObject.Find("SpawnPoint").GetComponent<Spawner>().increaseDifficulty(timeSpawnReduce, velocityIncrease);
    }

    public void DisableFire() { canShoot = false; }

    public void EnableFire() { canShoot = true; }

    public bool GetShootingStatus() { return canShoot; }

    public void TakeDamage(int damage) {
        life = Mathf.Clamp(life - damage, 0, maxLife);
    }

    public void Heal(int lifeRecovered) {
        life = Mathf.Clamp(life + lifeRecovered, 0, maxLife);
    }
}

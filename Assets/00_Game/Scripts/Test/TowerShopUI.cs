using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class TowerData
{
    public string towerName;
    public Sprite icon;
    public int cost;
    public GameObject prefab;
}

public class TowerShopUI : MonoBehaviour
{
    [Header("Tower Data List")]
    public List<TowerData> towers = new List<TowerData>();

    [Header("UI References")]
    public GameObject towerButtonPrefab; // Prefab chứa Button + Image + Text
    public Transform contentPanel;       // Panel để chứa các button (Vertical Layout / Grid Layout)

    void Start()
    {
        PopulateShop();
    }

    void PopulateShop()
    {
        foreach (TowerData tower in towers)
        {
            GameObject newButton = Instantiate(towerButtonPrefab, contentPanel);

            // Lấy các thành phần UI
            Image iconImage = newButton.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI costText = newButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            Button buyButton = newButton.GetComponent<Button>();

            // Gán dữ liệu
            iconImage.sprite = tower.icon;
            costText.text = tower.cost.ToString();

            // Lưu reference để truyền vào hàm khi click
            TowerData selectedTower = tower;
            buyButton.onClick.AddListener(() => OnTowerSelected(selectedTower));
        }
    }

    void OnTowerSelected(TowerData tower)
    {
        Debug.Log("Tower selected: " + tower.towerName);
        // Gọi BuildManager hoặc hệ thống đặt tháp ở đây
        // BuildManager.instance.SelectTower(tower.prefab, tower.cost);
    }
}

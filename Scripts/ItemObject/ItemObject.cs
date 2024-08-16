using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private Dictionary<ItemID, InventoryItemInfoData> Inventory => GameManager.Instance.NowPlayerData.Inventory;

    [Header("Data")]
    [SerializeField] private ItemID ItemID;
    public ItemData ItemData;

    private float _waitTime = 1.5f;
    private float _moveSpeed = 30f;
    private float _startTime = 0f;

    private float _effectTime = 1f;
    private float _effectForce = 2f;
    private int _effectCount = 1;
    private int _effectTurnCount = 3;

    private bool _isDroppedItem = false;
    private WaitForSeconds _maxActiveTime = new WaitForSeconds(10f);

    private void OnEnable()
    {
        // 현재 시간으로 초기화
        _startTime = Time.time;

        _isDroppedItem = false;
    }

    private void Start()
    {
        ItemData = GM.GetItemData(ItemID);
    }

    private void Update()
    {
        // 대기 시간이 지난 후 아이템 이동
        if (!_isDroppedItem && Time.time - _startTime >= _waitTime)
        {
            MoveItem();
        }
    }

    private void MoveItem()
    {
        // 플레이어 쪽으로 이동
        Vector3 targetPosition = GM.Player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 대기 시간 확인
        if (Time.time - _startTime < _waitTime) return;

        if (collision.CompareTag(Tag.Player))
        {
            // 인벤토리에 해당 아이템 존재하는지 검사
            if (Inventory.ContainsKey(ItemID))
            {
                if (Inventory[ItemID].Amount >= 9999)
                {
                    _isDroppedItem = true;
                    GM.ShowAlert("해당 아이템은 더 이상 습득할 수 없습니다!");
                    StartCoroutine(SetActiveDelay());
                    return;
                }
                else
                {
                    Inventory[ItemID].Amount++;
                }
            }
            else
            {
                // 인벤토리 빈칸 검사
                if (Inventory.Count == GM.UIInventory.Slots.Length)
                {
                    _isDroppedItem = true;
                    GM.ShowAlert("인벤토리가 가득 찼습니다!");
                    StartCoroutine(SetActiveDelay());
                    return;
                }
                else
                { 
                    InventoryItemInfoData data = new InventoryItemInfoData();
                    data.ID = ItemID;
                    data.Amount = 1;
                    data.OrderInInventory = Inventory.Count;
                    Inventory.Add(ItemID, data);
                }
            }

            GM.UIInventory.UpdateUI();

            // 아이템 타입이 퀵슬롯 아이템 타입과 같으면 퀵슬롯 업데이트
            if (ItemID == GM.NowPlayerData.QuickSlotItemData)
            {
                GM.UIInventory.QuickSlotUI.UpdateUI();
            }

            // 오브젝트 비활성화
            gameObject.SetActive(false);

            GM.PlaySFX(SFX.Pickup);
        }
    }

    public void DropEffect(Vector3 targerPos)
    {
        transform.DOJump(targerPos, _effectForce, _effectCount, _effectTime).SetEase(Ease.InQuad);
        transform.DORotate(new Vector3(0f, 0f, 360f * _effectTurnCount), _effectTime, RotateMode.FastBeyond360);
    }

    private IEnumerator SetActiveDelay()
    {
        yield return _maxActiveTime;

        gameObject.SetActive(false);
    }
}
